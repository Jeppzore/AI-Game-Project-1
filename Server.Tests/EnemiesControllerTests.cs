using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MongoDB.Driver;
using Server.Controllers;
using Server.Models;
using Server.Services;
using Xunit;

namespace Server.Tests;

public class EnemiesControllerTests
{
    private readonly Mock<IMongoDbService> _mockMongoDbService;
    private readonly Mock<IMongoCollection<Enemy>> _mockEnemyCollection;
    private readonly Mock<ILogger<EnemiesController>> _mockLogger;
    private readonly EnemiesController _controller;

    public EnemiesControllerTests()
    {
        _mockMongoDbService = new Mock<IMongoDbService>();
        _mockEnemyCollection = new Mock<IMongoCollection<Enemy>>();
        _mockLogger = new Mock<ILogger<EnemiesController>>();

        _mockMongoDbService
            .Setup(s => s.GetEnemiesCollection())
            .Returns(_mockEnemyCollection.Object);

        _controller = new EnemiesController(_mockMongoDbService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateEnemy_ReturnCreatedAtAction_WhenValidDataProvided()
    {
        // Arrange
        var request = new CreateEnemyRequest
        {
            Name = "Dragon",
            Description = "A fierce dragon",
            Health = 100,
            Attack = 20,
            Defense = 15,
            Experience = 500
        };

        _mockEnemyCollection.Setup(c => c.InsertOneAsync(It.IsAny<Enemy>(), null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateEnemy(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(EnemiesController.GetEnemyById), createdResult.ActionName);
        
        var response = Assert.IsType<ApiResponse<Enemy>>(createdResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal("Dragon", response.Data.Name);
        Assert.Equal(100, response.Data.Health);
        Assert.Equal(20, response.Data.Attack);
        Assert.Equal(15, response.Data.Defense);
        Assert.Equal(500, response.Data.Experience);
    }

    [Fact]
    public async Task CreateEnemy_SetsValidTimestamps()
    {
        // Arrange
        var request = new CreateEnemyRequest
        {
            Name = "Goblin",
            Description = "A small goblin",
            Health = 10,
            Attack = 5,
            Defense = 2,
            Experience = 25
        };

        Enemy? capturedEnemy = null;
        _mockEnemyCollection.Setup(c => c.InsertOneAsync(It.IsAny<Enemy>(), null, It.IsAny<CancellationToken>()))
            .Callback<Enemy, InsertOneOptions, CancellationToken>((enemy, opts, ct) => capturedEnemy = enemy)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateEnemy(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<ApiResponse<Enemy>>(createdResult.Value);
        
        var enemy = response.Data;
        Assert.NotNull(enemy);
        Assert.True(enemy.CreatedAt > DateTime.UtcNow.AddSeconds(-5));
        Assert.True(enemy.UpdatedAt > DateTime.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public async Task CreateEnemy_WithDrops_PreservesLootData()
    {
        // Arrange
        var drops = new List<LootDrop>
        {
            new LootDrop { ItemName = "Gold", DropRate = 0.8m, Quantity = 50 },
            new LootDrop { ItemName = "Dragon Scale", DropRate = 0.1m, Quantity = 1 }
        };

        var request = new CreateEnemyRequest
        {
            Name = "Dragon",
            Description = "A fierce dragon",
            Health = 100,
            Attack = 20,
            Defense = 15,
            Experience = 500,
            Drops = drops
        };

        _mockEnemyCollection.Setup(c => c.InsertOneAsync(It.IsAny<Enemy>(), null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateEnemy(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<ApiResponse<Enemy>>(createdResult.Value);
        
        var enemy = response.Data;
        Assert.NotNull(enemy);
        Assert.Equal(2, enemy.Drops.Count);
        Assert.Equal("Gold", enemy.Drops[0].ItemName);
        Assert.Equal(0.8m, enemy.Drops[0].DropRate);
        Assert.Equal("Dragon Scale", enemy.Drops[1].ItemName);
    }

    [Fact]
    public async Task EnemyModel_HasRequiredFields()
    {
        // Arrange & Act
        var enemy = new Enemy
        {
            Id = "test-id",
            Name = "TestEnemy",
            Description = "A test enemy",
            Health = 50,
            Attack = 10,
            Defense = 5,
            Experience = 100
        };

        // Assert
        Assert.NotNull(enemy.Name);
        Assert.NotNull(enemy.Description);
        Assert.True(enemy.Health > 0);
        Assert.True(enemy.Attack > 0);
        Assert.True(enemy.Defense >= 0);
        Assert.True(enemy.Experience >= 0);
        Assert.NotNull(enemy.Drops);
    }
}

