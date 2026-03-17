using MongoDB.Driver;
using Server.Models;

namespace Server.Services;

/// <summary>
/// Service for handling all trading operations including buy/sell logic, inventory management, and transaction logging.
/// </summary>
public interface ITradingService
{
    Task<List<NPC>> GetNPCShopsAsync();
    Task<ShopInventory?> GetShopInventoryAsync(string npcId);
    Task<PlayerInventory?> GetPlayerInventoryAsync(string playerId);
    Task<bool> ValidateShopStockAsync(string npcId, string itemId, int quantity);
    Task<Result<Transaction>> ProcessPurchaseAsync(string playerId, string npcId, string itemId, int quantity, int price);
    Task<Result<Transaction>> ProcessSaleAsync(string playerId, string npcId, string itemId, int quantity, int price);
    Task<bool> UpdateShopStockAsync(string npcId, string itemId, int quantityChange);
    Task<bool> LogTransactionAsync(Transaction transaction);
    Task InitializePlayerInventoryAsync(string playerId);
}

public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }

    public static Result<T> FromSuccess(T data) => new() { Success = true, Data = data };
    public static Result<T> FromError(string error) => new() { Success = false, Error = error };
}

public class TradingService : ITradingService
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<TradingService> _logger;

    public TradingService(IMongoDbService mongoDbService, ILogger<TradingService> logger)
    {
        _database = mongoDbService.GetDatabase();
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all NPCs that have shops.
    /// </summary>
    public async Task<List<NPC>> GetNPCShopsAsync()
    {
        try
        {
            var npcsCollection = _database.GetCollection<NPC>("npcs");
            var filter = Builders<NPC>.Filter.Ne(n => n.Trades, null);
            return await npcsCollection.Find(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching NPC shops");
            return new List<NPC>();
        }
    }

    /// <summary>
    /// Retrieves shop inventory for a specific NPC.
    /// </summary>
    public async Task<ShopInventory?> GetShopInventoryAsync(string npcId)
    {
        try
        {
            var inventoryCollection = _database.GetCollection<ShopInventory>("shop_inventory");
            return await inventoryCollection.Find(s => s.NpcId == npcId).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching shop inventory for NPC: {NpcId}", npcId);
            return null;
        }
    }

    /// <summary>
    /// Retrieves a player's inventory.
    /// </summary>
    public async Task<PlayerInventory?> GetPlayerInventoryAsync(string playerId)
    {
        try
        {
            var inventoryCollection = _database.GetCollection<PlayerInventory>("player_inventory");
            return await inventoryCollection.Find(p => p.PlayerId == playerId).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching player inventory for player: {PlayerId}", playerId);
            return null;
        }
    }

    /// <summary>
    /// Validates if an item is in stock at a shop.
    /// </summary>
    public async Task<bool> ValidateShopStockAsync(string npcId, string itemId, int quantity)
    {
        try
        {
            var inventory = await GetShopInventoryAsync(npcId);
            if (inventory == null)
                return false;

            var item = inventory.Items.FirstOrDefault(i => i.ItemId == itemId);
            return item != null && item.StockQuantity >= quantity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating shop stock");
            return false;
        }
    }

    /// <summary>
    /// Processes a purchase transaction: deduct gold from player, add item to inventory, deduct from shop stock.
    /// </summary>
    public async Task<Result<Transaction>> ProcessPurchaseAsync(string playerId, string npcId, string itemId, int quantity, int price)
    {
        try
        {
            // Validate player inventory exists
            var playerInventory = await GetPlayerInventoryAsync(playerId);
            if (playerInventory == null)
            {
                await InitializePlayerInventoryAsync(playerId);
                playerInventory = await GetPlayerInventoryAsync(playerId);
            }

            // Validate sufficient gold
            int totalCost = price * quantity;
            if (playerInventory.Gold < totalCost)
            {
                return Result<Transaction>.FromError($"Insufficient gold. Required: {totalCost}, Have: {playerInventory.Gold}");
            }

            // Validate shop stock
            var shopInventory = await GetShopInventoryAsync(npcId);
            if (shopInventory == null)
            {
                return Result<Transaction>.FromError("Shop not found");
            }

            var shopItem = shopInventory.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (shopItem == null || shopItem.StockQuantity < quantity)
            {
                return Result<Transaction>.FromError("Item out of stock or not available");
            }

            // Get NPC info
            var npcsCollection = _database.GetCollection<NPC>("npcs");
            var npc = await npcsCollection.Find(n => n.Id == npcId).FirstOrDefaultAsync();
            if (npc == null)
            {
                return Result<Transaction>.FromError("NPC not found");
            }

            // Update player inventory: deduct gold and add item
            var playerInventoryCollection = _database.GetCollection<PlayerInventory>("player_inventory");
            var inventoryItem = playerInventory.Items.FirstOrDefault(i => i.ItemId == itemId);
            
            if (inventoryItem != null)
            {
                // Update quantity if item exists
                await playerInventoryCollection.UpdateOneAsync(
                    p => p.PlayerId == playerId && p.Items.Any(i => i.ItemId == itemId),
                    Builders<PlayerInventory>.Update
                        .Set(p => p.Gold, playerInventory.Gold - totalCost)
                        .Set(p => p.LastUpdated, DateTime.UtcNow)
                        .Inc("items.$.quantity", quantity)
                );
            }
            else
            {
                // Add new item
                await playerInventoryCollection.UpdateOneAsync(
                    p => p.PlayerId == playerId,
                    Builders<PlayerInventory>.Update
                        .Set(p => p.Gold, playerInventory.Gold - totalCost)
                        .Set(p => p.LastUpdated, DateTime.UtcNow)
                        .Push(p => p.Items, new InventoryItem 
                        { 
                            ItemId = itemId, 
                            ItemName = shopItem.ItemName, 
                            Quantity = quantity 
                        })
                );
            }

            // Update shop inventory: deduct stock
            await UpdateShopStockAsync(npcId, itemId, -quantity);

            // Create and log transaction
            var transaction = new Transaction
            {
                BuyerId = playerId,
                SellerId = npc.Id,
                SellerName = npc.Name,
                ItemId = itemId,
                ItemName = shopItem.ItemName,
                Quantity = quantity,
                UnitPrice = price,
                TotalPrice = totalCost,
                TransactionType = "purchase",
                Timestamp = DateTime.UtcNow
            };

            await LogTransactionAsync(transaction);

            return Result<Transaction>.FromSuccess(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing purchase for player: {PlayerId}", playerId);
            return Result<Transaction>.FromError($"Error processing purchase: {ex.Message}");
        }
    }

    /// <summary>
    /// Processes a sale transaction: add gold to player, remove item from inventory, add to shop stock.
    /// </summary>
    public async Task<Result<Transaction>> ProcessSaleAsync(string playerId, string npcId, string itemId, int quantity, int price)
    {
        try
        {
            // Validate player inventory exists and has item
            var playerInventory = await GetPlayerInventoryAsync(playerId);
            if (playerInventory == null)
            {
                return Result<Transaction>.FromError("Player inventory not found");
            }

            var inventoryItem = playerInventory.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (inventoryItem == null || inventoryItem.Quantity < quantity)
            {
                return Result<Transaction>.FromError("Item not in inventory or insufficient quantity");
            }

            // Get NPC and shop info
            var npcsCollection = _database.GetCollection<NPC>("npcs");
            var npc = await npcsCollection.Find(n => n.Id == npcId).FirstOrDefaultAsync();
            if (npc == null)
            {
                return Result<Transaction>.FromError("NPC not found");
            }

            var shopInventory = await GetShopInventoryAsync(npcId);
            if (shopInventory == null)
            {
                return Result<Transaction>.FromError("Shop not found");
            }

            // Verify NPC buys this item
            var tradeItem = npc.Trades.Buys.FirstOrDefault(t => t.ItemId == itemId);
            if (tradeItem == null)
            {
                return Result<Transaction>.FromError("NPC does not buy this item");
            }

            // Update player inventory: add gold and remove item
            int totalGold = price * quantity;
            var playerInventoryCollection = _database.GetCollection<PlayerInventory>("player_inventory");

            if (inventoryItem.Quantity == quantity)
            {
                // Remove item entirely
                await playerInventoryCollection.UpdateOneAsync(
                    p => p.PlayerId == playerId,
                    Builders<PlayerInventory>.Update
                        .Set(p => p.Gold, playerInventory.Gold + totalGold)
                        .Set(p => p.LastUpdated, DateTime.UtcNow)
                        .PullFilter(p => p.Items, i => i.ItemId == itemId)
                );
            }
            else
            {
                // Reduce quantity
                await playerInventoryCollection.UpdateOneAsync(
                    p => p.PlayerId == playerId && p.Items.Any(i => i.ItemId == itemId),
                    Builders<PlayerInventory>.Update
                        .Set(p => p.Gold, playerInventory.Gold + totalGold)
                        .Set(p => p.LastUpdated, DateTime.UtcNow)
                        .Inc("items.$.quantity", -quantity)
                );
            }

            // Update shop inventory: add stock
            await UpdateShopStockAsync(npcId, itemId, quantity);

            // Create and log transaction
            var transaction = new Transaction
            {
                BuyerId = playerId,
                SellerId = npc.Id,
                SellerName = npc.Name,
                ItemId = itemId,
                ItemName = inventoryItem.ItemName,
                Quantity = quantity,
                UnitPrice = price,
                TotalPrice = totalGold,
                TransactionType = "sale",
                Timestamp = DateTime.UtcNow
            };

            await LogTransactionAsync(transaction);

            return Result<Transaction>.FromSuccess(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing sale for player: {PlayerId}", playerId);
            return Result<Transaction>.FromError($"Error processing sale: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates shop stock quantity for an item.
    /// </summary>
    public async Task<bool> UpdateShopStockAsync(string npcId, string itemId, int quantityChange)
    {
        try
        {
            var inventoryCollection = _database.GetCollection<ShopInventory>("shop_inventory");
            var result = await inventoryCollection.UpdateOneAsync(
                s => s.NpcId == npcId && s.Items.Any(i => i.ItemId == itemId),
                Builders<ShopInventory>.Update
                    .Inc("items.$.stock_quantity", quantityChange)
                    .Set(s => s.UpdatedAt, DateTime.UtcNow)
            );

            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating shop stock for NPC: {NpcId}", npcId);
            return false;
        }
    }

    /// <summary>
    /// Logs a transaction for audit trail.
    /// </summary>
    public async Task<bool> LogTransactionAsync(Transaction transaction)
    {
        try
        {
            var transactionsCollection = _database.GetCollection<Transaction>("transactions");
            await transactionsCollection.InsertOneAsync(transaction);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging transaction");
            return false;
        }
    }

    /// <summary>
    /// Initializes a new player inventory with starting gold.
    /// </summary>
    public async Task InitializePlayerInventoryAsync(string playerId)
    {
        try
        {
            var inventoryCollection = _database.GetCollection<PlayerInventory>("player_inventory");
            var newInventory = new PlayerInventory
            {
                PlayerId = playerId,
                Gold = 1000, // Starting gold
                Level = 1,
                Experience = 0,
                Items = new List<InventoryItem>(),
                LastUpdated = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await inventoryCollection.InsertOneAsync(newInventory);
            _logger.LogInformation("Initialized player inventory for player: {PlayerId}", playerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing player inventory for player: {PlayerId}", playerId);
        }
    }
}
