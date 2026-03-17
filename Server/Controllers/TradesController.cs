using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// Manages all trading-related API endpoints including buying, selling, and inventory management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TradesController : ControllerBase
{
    private readonly ITradingService _tradingService;
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<TradesController> _logger;
    private const string DefaultPlayerId = "player_001"; // Hardcoded for now

    public TradesController(ITradingService tradingService, IMongoDbService mongoDbService, ILogger<TradesController> logger)
    {
        _tradingService = tradingService;
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    /// <summary>
    /// Get all NPCs that have shops.
    /// </summary>
    [HttpGet("npcs")]
    public async Task<ActionResult<ApiResponse<List<NpcShopResponse>>>> GetNpcShops()
    {
        try
        {
            var npcs = await _tradingService.GetNPCShopsAsync();
            var shopResponses = new List<NpcShopResponse>();

            foreach (var npc in npcs)
            {
                var shop = await _tradingService.GetShopInventoryAsync(npc.Id);
                shopResponses.Add(new NpcShopResponse
                {
                    Id = npc.Id,
                    Name = npc.Name,
                    Role = npc.Role,
                    Location = npc.Location,
                    Shop = shop != null ? new ShopInventoryResponse
                    {
                        Items = shop.Items.Select(i => new ShopItemResponse
                        {
                            ItemId = i.ItemId,
                            ItemName = i.ItemName,
                            Price = i.Price,
                            StockQuantity = i.StockQuantity
                        }).ToList(),
                        TotalItems = shop.Items.Count
                    } : null
                });
            }

            return Ok(new ApiResponse<List<NpcShopResponse>> { Data = shopResponses });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching NPC shops");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching NPC shops"
            });
        }
    }

    /// <summary>
    /// Get shop inventory for a specific NPC.
    /// </summary>
    [HttpGet("npcs/{npcId}/inventory")]
    public async Task<ActionResult<ApiResponse<ShopInventoryResponse>>> GetNpcInventory(string npcId)
    {
        try
        {
            var shop = await _tradingService.GetShopInventoryAsync(npcId);
            if (shop == null)
            {
                return NotFound(new ApiResponse<object> { Error = "Shop not found" });
            }

            var response = new ShopInventoryResponse
            {
                Items = shop.Items.Select(i => new ShopItemResponse
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Price = i.Price,
                    StockQuantity = i.StockQuantity
                }).ToList(),
                TotalItems = shop.Items.Count
            };

            return Ok(new ApiResponse<ShopInventoryResponse> { Data = response });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching NPC inventory for NPC: {NpcId}", npcId);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching NPC inventory"
            });
        }
    }

    /// <summary>
    /// Get player's current inventory.
    /// </summary>
    [HttpGet("player-inventory")]
    public async Task<ActionResult<ApiResponse<PlayerInventoryResponse>>> GetPlayerInventory()
    {
        try
        {
            var playerId = DefaultPlayerId;
            var inventory = await _tradingService.GetPlayerInventoryAsync(playerId);

            if (inventory == null)
            {
                await _tradingService.InitializePlayerInventoryAsync(playerId);
                inventory = await _tradingService.GetPlayerInventoryAsync(playerId);
            }

            var response = new PlayerInventoryResponse
            {
                PlayerId = inventory!.PlayerId,
                Gold = inventory.Gold,
                Level = inventory.Level,
                Experience = inventory.Experience,
                Items = inventory.Items.Select(i => new InventoryItemResponse
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Quantity = i.Quantity
                }).ToList()
            };

            return Ok(new ApiResponse<PlayerInventoryResponse> { Data = response });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching player inventory");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching player inventory"
            });
        }
    }

    /// <summary>
    /// Buy an item from an NPC shop.
    /// </summary>
    [HttpPost("buy")]
    public async Task<ActionResult<ApiResponse<TransactionResponse>>> BuyItem([FromBody] BuyItemRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object> { Error = "Invalid request data" });
            }

            var playerId = DefaultPlayerId;

            // Get shop and item price
            var shop = await _tradingService.GetShopInventoryAsync(request.NpcId);
            if (shop == null)
            {
                return NotFound(new ApiResponse<object> { Error = "Shop not found" });
            }

            var shopItem = shop.Items.FirstOrDefault(i => i.ItemId == request.ItemId);
            if (shopItem == null)
            {
                return NotFound(new ApiResponse<object> { Error = "Item not found in shop" });
            }

            // Process purchase
            var result = await _tradingService.ProcessPurchaseAsync(
                playerId,
                request.NpcId,
                request.ItemId,
                request.Quantity,
                shopItem.Price
            );

            if (!result.Success)
            {
                return BadRequest(new ApiResponse<object> { Error = result.Error });
            }

            var transaction = result.Data!;
            var response = new TransactionResponse
            {
                Id = transaction.Id,
                ItemName = transaction.ItemName,
                Quantity = transaction.Quantity,
                UnitPrice = transaction.UnitPrice,
                TotalPrice = transaction.TotalPrice,
                TransactionType = transaction.TransactionType,
                Timestamp = transaction.Timestamp
            };

            return Ok(new ApiResponse<TransactionResponse> { Data = response });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing purchase");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while processing the purchase"
            });
        }
    }

    /// <summary>
    /// Sell an item to an NPC.
    /// </summary>
    [HttpPost("sell")]
    public async Task<ActionResult<ApiResponse<TransactionResponse>>> SellItem([FromBody] SellItemRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object> { Error = "Invalid request data" });
            }

            var playerId = DefaultPlayerId;

            // Get NPC trading information
            var npcsCollection = _mongoDbService.GetNpcsCollection();
            var npc = await npcsCollection.Find(n => n.Id == request.NpcId).FirstOrDefaultAsync();

            if (npc == null)
            {
                return NotFound(new ApiResponse<object> { Error = "NPC not found" });
            }

            var tradeItem = npc.Trades.Buys.FirstOrDefault(t => t.ItemId == request.ItemId);
            if (tradeItem == null)
            {
                return BadRequest(new ApiResponse<object> { Error = "NPC does not buy this item" });
            }

            // Process sale
            var result = await _tradingService.ProcessSaleAsync(
                playerId,
                request.NpcId,
                request.ItemId,
                request.Quantity,
                tradeItem.Price
            );

            if (!result.Success)
            {
                return BadRequest(new ApiResponse<object> { Error = result.Error });
            }

            var transaction = result.Data!;
            var response = new TransactionResponse
            {
                Id = transaction.Id,
                ItemName = transaction.ItemName,
                Quantity = transaction.Quantity,
                UnitPrice = transaction.UnitPrice,
                TotalPrice = transaction.TotalPrice,
                TransactionType = transaction.TransactionType,
                Timestamp = transaction.Timestamp
            };

            return Ok(new ApiResponse<TransactionResponse> { Data = response });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing sale");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while processing the sale"
            });
        }
    }

    /// <summary>
    /// Get transaction history for the player.
    /// </summary>
    [HttpGet("transactions")]
    public async Task<ActionResult<ApiResponse<List<TransactionResponse>>>> GetTransactionHistory()
    {
        try
        {
            var playerId = DefaultPlayerId;
            var transactionsCollection = _mongoDbService.GetTransactionsCollection();

            var transactions = await transactionsCollection
                .Find(t => t.BuyerId == playerId)
                .SortByDescending(t => t.Timestamp)
                .Limit(100)
                .ToListAsync();

            var responses = transactions.Select(t => new TransactionResponse
            {
                Id = t.Id,
                ItemName = t.ItemName,
                Quantity = t.Quantity,
                UnitPrice = t.UnitPrice,
                TotalPrice = t.TotalPrice,
                TransactionType = t.TransactionType,
                Timestamp = t.Timestamp
            }).ToList();

            return Ok(new ApiResponse<List<TransactionResponse>> { Data = responses });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transaction history");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching transaction history"
            });
        }
    }
}
