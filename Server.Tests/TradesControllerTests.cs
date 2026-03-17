using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MongoDB.Driver;
using Server.Controllers;
using Server.Models;
using Server.Services;
using Xunit;

namespace Server.Tests;

public class TradesControllerTests
{
    private readonly Mock<ITradingService> _mockTradingService;
    private readonly Mock<IMongoDbService> _mockMongoDbService;
    private readonly Mock<IMongoCollection<NPC>> _mockNpcCollection;
    private readonly Mock<IMongoCollection<Transaction>> _mockTransactionCollection;
    private readonly Mock<ILogger<TradesController>> _mockLogger;
    private readonly TradesController _controller;

    public TradesControllerTests()
    {
        _mockTradingService = new Mock<ITradingService>();
        _mockMongoDbService = new Mock<IMongoDbService>();
        _mockNpcCollection = new Mock<IMongoCollection<NPC>>();
        _mockTransactionCollection = new Mock<IMongoCollection<Transaction>>();
        _mockLogger = new Mock<ILogger<TradesController>>();

        _mockMongoDbService
            .Setup(s => s.GetNpcsCollection())
            .Returns(_mockNpcCollection.Object);

        _mockMongoDbService
            .Setup(s => s.GetTransactionsCollection())
            .Returns(_mockTransactionCollection.Object);

        _controller = new TradesController(_mockTradingService.Object, _mockMongoDbService.Object, _mockLogger.Object);
    }

    #region Get NPC Shops

    [Fact]
    public async Task GetNpcShops_ReturnsList_WhenShopsExist()
    {
        // Arrange
        var npcs = new List<NPC>
        {
            new NPC
            {
                Id = "npc_001",
                Name = "Blacksmith",
                Role = "vendor",
                Location = new NPCLocation { Area = "Keep" }
            }
        };

        var shopInventory = new ShopInventory
        {
            NpcId = "npc_001",
            NpcName = "Blacksmith",
            Items = new List<ShopItem>
            {
                new ShopItem { ItemId = "item_001", ItemName = "Sword", Price = 100, StockQuantity = 5 }
            }
        };

        _mockTradingService
            .Setup(s => s.GetNPCShopsAsync())
            .ReturnsAsync(npcs);

        _mockTradingService
            .Setup(s => s.GetShopInventoryAsync(It.IsAny<string>()))
            .ReturnsAsync(shopInventory);

        // Act
        var result = await _controller.GetNpcShops();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<List<NpcShopResponse>>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("Blacksmith", response.Data[0].Name);
    }

    [Fact]
    public async Task GetNpcShops_ReturnsEmpty_WhenNoShops()
    {
        // Arrange
        _mockTradingService
            .Setup(s => s.GetNPCShopsAsync())
            .ReturnsAsync(new List<NPC>());

        // Act
        var result = await _controller.GetNpcShops();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<List<NpcShopResponse>>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Empty(response.Data);
    }

    #endregion

    #region Get NPC Inventory

    [Fact]
    public async Task GetNpcInventory_ReturnsShop_WhenFound()
    {
        // Arrange
        var shopInventory = new ShopInventory
        {
            NpcId = "npc_001",
            Items = new List<ShopItem>
            {
                new ShopItem { ItemId = "item_001", ItemName = "Sword", Price = 100, StockQuantity = 5 },
                new ShopItem { ItemId = "item_002", ItemName = "Shield", Price = 75, StockQuantity = 3 }
            }
        };

        _mockTradingService
            .Setup(s => s.GetShopInventoryAsync("npc_001"))
            .ReturnsAsync(shopInventory);

        // Act
        var result = await _controller.GetNpcInventory("npc_001");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<ShopInventoryResponse>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Items.Count);
        Assert.Equal("Sword", response.Data.Items[0].ItemName);
        Assert.Equal(100, response.Data.Items[0].Price);
    }

    [Fact]
    public async Task GetNpcInventory_ReturnsNotFound_WhenShopDoesNotExist()
    {
        // Arrange
        _mockTradingService
            .Setup(s => s.GetShopInventoryAsync("invalid_id"))
            .ReturnsAsync((ShopInventory?)null);

        // Act
        var result = await _controller.GetNpcInventory("invalid_id");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(notFoundResult.Value);
        Assert.NotNull(response.Error);
        Assert.Contains("Shop not found", response.Error);
    }

    #endregion

    #region Get Player Inventory

    [Fact]
    public async Task GetPlayerInventory_ReturnsInventory_WhenExists()
    {
        // Arrange
        var playerInventory = new PlayerInventory
        {
            PlayerId = "player_001",
            Gold = 500,
            Level = 1,
            Experience = 100,
            Items = new List<InventoryItem>
            {
                new InventoryItem { ItemId = "item_005", ItemName = "Health Potion", Quantity = 5 }
            }
        };

        _mockTradingService
            .Setup(s => s.GetPlayerInventoryAsync("player_001"))
            .ReturnsAsync(playerInventory);

        // Act
        var result = await _controller.GetPlayerInventory();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<PlayerInventoryResponse>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal("player_001", response.Data.PlayerId);
        Assert.Equal(500, response.Data.Gold);
        Assert.Single(response.Data.Items);
    }

    [Fact]
    public async Task GetPlayerInventory_InitializesInventory_WhenDoesNotExist()
    {
        // Arrange
        var playerInventory = new PlayerInventory
        {
            PlayerId = "player_001",
            Gold = 1000,
            Items = new List<InventoryItem>()
        };

        _mockTradingService
            .Setup(s => s.GetPlayerInventoryAsync("player_001"))
            .ReturnsAsync((PlayerInventory?)null)
            .Then(() => _mockTradingService
                .Setup(s => s.GetPlayerInventoryAsync("player_001"))
                .ReturnsAsync(playerInventory));

        _mockTradingService
            .Setup(s => s.InitializePlayerInventoryAsync("player_001"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.GetPlayerInventory();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<PlayerInventoryResponse>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal(1000, response.Data.Gold);
        _mockTradingService.Verify(s => s.InitializePlayerInventoryAsync("player_001"), Times.Once);
    }

    #endregion

    #region Buy Item

    [Fact]
    public async Task BuyItem_SuccessfulPurchase_WhenSufficientGoldAndStock()
    {
        // Arrange
        var request = new BuyItemRequest
        {
            NpcId = "npc_001",
            ItemId = "item_001",
            Quantity = 1
        };

        var shop = new ShopInventory
        {
            NpcId = "npc_001",
            Items = new List<ShopItem>
            {
                new ShopItem { ItemId = "item_001", ItemName = "Sword", Price = 100, StockQuantity = 5 }
            }
        };

        var transaction = new Transaction
        {
            Id = "txn_001",
            BuyerId = "player_001",
            ItemName = "Sword",
            Quantity = 1,
            UnitPrice = 100,
            TotalPrice = 100,
            TransactionType = "purchase"
        };

        _mockTradingService
            .Setup(s => s.GetShopInventoryAsync("npc_001"))
            .ReturnsAsync(shop);

        _mockTradingService
            .Setup(s => s.ProcessPurchaseAsync("player_001", "npc_001", "item_001", 1, 100))
            .ReturnsAsync(Result<Transaction>.FromSuccess(transaction));

        // Act
        var result = await _controller.BuyItem(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<TransactionResponse>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal("Sword", response.Data.ItemName);
        Assert.Equal(1, response.Data.Quantity);
        Assert.Equal(100, response.Data.TotalPrice);
    }

    [Fact]
    public async Task BuyItem_ReturnsBadRequest_WhenInsufficientGold()
    {
        // Arrange
        var request = new BuyItemRequest
        {
            NpcId = "npc_001",
            ItemId = "item_001",
            Quantity = 10
        };

        var shop = new ShopInventory
        {
            NpcId = "npc_001",
            Items = new List<ShopItem>
            {
                new ShopItem { ItemId = "item_001", ItemName = "Sword", Price = 500, StockQuantity = 10 }
            }
        };

        _mockTradingService
            .Setup(s => s.GetShopInventoryAsync("npc_001"))
            .ReturnsAsync(shop);

        _mockTradingService
            .Setup(s => s.ProcessPurchaseAsync("player_001", "npc_001", "item_001", 10, 500))
            .ReturnsAsync(Result<Transaction>.FromError("Insufficient gold"));

        // Act
        var result = await _controller.BuyItem(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(badRequestResult.Value);
        Assert.NotNull(response.Error);
        Assert.Contains("Insufficient gold", response.Error);
    }

    [Fact]
    public async Task BuyItem_ReturnsNotFound_WhenShopNotFound()
    {
        // Arrange
        var request = new BuyItemRequest
        {
            NpcId = "invalid_npc",
            ItemId = "item_001",
            Quantity = 1
        };

        _mockTradingService
            .Setup(s => s.GetShopInventoryAsync("invalid_npc"))
            .ReturnsAsync((ShopInventory?)null);

        // Act
        var result = await _controller.BuyItem(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(notFoundResult.Value);
        Assert.NotNull(response.Error);
        Assert.Contains("Shop not found", response.Error);
    }

    [Fact]
    public async Task BuyItem_ReturnsNotFound_WhenItemNotInShop()
    {
        // Arrange
        var request = new BuyItemRequest
        {
            NpcId = "npc_001",
            ItemId = "invalid_item",
            Quantity = 1
        };

        var shop = new ShopInventory
        {
            NpcId = "npc_001",
            Items = new List<ShopItem>()
        };

        _mockTradingService
            .Setup(s => s.GetShopInventoryAsync("npc_001"))
            .ReturnsAsync(shop);

        // Act
        var result = await _controller.BuyItem(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(notFoundResult.Value);
        Assert.NotNull(response.Error);
        Assert.Contains("Item not found", response.Error);
    }

    [Fact]
    public async Task BuyItem_ReturnsOutOfStock_WhenQuantityExceeded()
    {
        // Arrange
        var request = new BuyItemRequest
        {
            NpcId = "npc_001",
            ItemId = "item_001",
            Quantity = 10
        };

        var shop = new ShopInventory
        {
            NpcId = "npc_001",
            Items = new List<ShopItem>
            {
                new ShopItem { ItemId = "item_001", ItemName = "Sword", Price = 100, StockQuantity = 2 }
            }
        };

        _mockTradingService
            .Setup(s => s.GetShopInventoryAsync("npc_001"))
            .ReturnsAsync(shop);

        _mockTradingService
            .Setup(s => s.ProcessPurchaseAsync("player_001", "npc_001", "item_001", 10, 100))
            .ReturnsAsync(Result<Transaction>.FromError("Item out of stock"));

        // Act
        var result = await _controller.BuyItem(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(badRequestResult.Value);
        Assert.NotNull(response.Error);
        Assert.Contains("out of stock", response.Error);
    }

    #endregion

    #region Sell Item

    [Fact]
    public async Task SellItem_SuccessfulSale_WhenItemOwned()
    {
        // Arrange
        var request = new SellItemRequest
        {
            NpcId = "npc_001",
            ItemId = "item_001",
            Quantity = 1
        };

        var npc = new NPC
        {
            Id = "npc_001",
            Name = "Blacksmith",
            Trades = new NPCTrades
            {
                Buys = new List<TradeItem>
                {
                    new TradeItem { ItemId = "item_001", ItemName = "Ore", Price = 50 }
                }
            }
        };

        var transaction = new Transaction
        {
            Id = "txn_002",
            BuyerId = "player_001",
            ItemName = "Ore",
            Quantity = 1,
            UnitPrice = 50,
            TotalPrice = 50,
            TransactionType = "sale"
        };

        var asyncCursor = new Mock<IAsyncCursor<NPC>>();
        asyncCursor
            .Setup(c => c.Current)
            .Returns(new List<NPC> { npc });
        asyncCursor
            .SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockNpcCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<NPC>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursor.Object);

        _mockTradingService
            .Setup(s => s.ProcessSaleAsync("player_001", "npc_001", "item_001", 1, 50))
            .ReturnsAsync(Result<Transaction>.FromSuccess(transaction));

        // Act
        var result = await _controller.SellItem(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<TransactionResponse>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal("Ore", response.Data.ItemName);
        Assert.Equal(50, response.Data.TotalPrice);
    }

    [Fact]
    public async Task SellItem_ReturnsBadRequest_WhenNpcDoesNotBuyItem()
    {
        // Arrange
        var request = new SellItemRequest
        {
            NpcId = "npc_001",
            ItemId = "item_999",
            Quantity = 1
        };

        var npc = new NPC
        {
            Id = "npc_001",
            Name = "Blacksmith",
            Trades = new NPCTrades { Buys = new List<TradeItem>() }
        };

        var asyncCursor = new Mock<IAsyncCursor<NPC>>();
        asyncCursor
            .Setup(c => c.Current)
            .Returns(new List<NPC> { npc });
        asyncCursor
            .SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockNpcCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<NPC>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursor.Object);

        // Act
        var result = await _controller.SellItem(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(badRequestResult.Value);
        Assert.NotNull(response.Error);
        Assert.Contains("does not buy this item", response.Error);
    }

    #endregion

    #region Get Transactions

    [Fact]
    public async Task GetTransactionHistory_ReturnsTransactions_WhenExist()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = "txn_001",
                BuyerId = "player_001",
                ItemName = "Sword",
                Quantity = 1,
                UnitPrice = 100,
                TotalPrice = 100,
                TransactionType = "purchase",
                Timestamp = DateTime.UtcNow
            }
        };

        var mockCursor = new Mock<IAsyncCursor<Transaction>>();
        mockCursor
            .Setup(c => c.Current)
            .Returns(transactions);
        mockCursor
            .SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockTransactionCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Transaction>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _controller.GetTransactionHistory();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<List<TransactionResponse>>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("Sword", response.Data[0].ItemName);
    }

    #endregion
}
