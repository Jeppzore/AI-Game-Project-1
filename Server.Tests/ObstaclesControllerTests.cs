using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MongoDB.Driver;
using Server.Controllers;
using Server.Models;
using Server.Services;
using Xunit;

namespace Server.Tests;

public class ObstaclesControllerTests
{
    private readonly Mock<IMongoDbService> _mockMongoDbService;
    private readonly Mock<IObstacleService> _mockObstacleService;
    private readonly Mock<IMongoCollection<Obstacle>> _mockObstacleCollection;
    private readonly Mock<ILogger<ObstaclesController>> _mockLogger;
    private readonly ObstaclesController _controller;

    public ObstaclesControllerTests()
    {
        _mockMongoDbService = new Mock<IMongoDbService>();
        _mockObstacleService = new Mock<IObstacleService>();
        _mockObstacleCollection = new Mock<IMongoCollection<Obstacle>>();
        _mockLogger = new Mock<ILogger<ObstaclesController>>();

        _mockMongoDbService
            .Setup(s => s.GetObstaclesCollection())
            .Returns(_mockObstacleCollection.Object);

        _controller = new ObstaclesController(_mockMongoDbService.Object, _mockObstacleService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllObstacles_ReturnsPagedObstacles()
    {
        // Arrange
        var obstacles = new List<Obstacle>
        {
            new() { Id = "1", Name = "Sphinx Riddle", Type = "riddle", Difficulty = 3 },
            new() { Id = "2", Name = "Ancient Chest", Type = "locked_chest", Difficulty = 5 }
        };

        var filter = Builders<Obstacle>.Filter.Empty;
        var sort = Builders<Obstacle>.Sort.Ascending(o => o.Name);

        var mockAsyncCursor = new Mock<IAsyncCursor<Obstacle>>();
        mockAsyncCursor
            .Setup(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        mockAsyncCursor
            .Setup(x => x.Current)
            .Returns(obstacles);

        _mockObstacleCollection
            .Setup(c => c.FindAsync(filter, It.IsAny<FindOptions<Obstacle>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockAsyncCursor.Object);

        _mockObstacleCollection
            .Setup(c => c.CountDocumentsAsync(filter, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        // Act
        var result = await _controller.GetAllObstacles();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<PaginatedResponse<List<Obstacle>>>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Items.Count);
    }

    [Fact]
    public async Task GetObstacleById_ReturnsObstacle_WhenExists()
    {
        // Arrange
        var obstacle = new Obstacle
        {
            Id = "1",
            Name = "Sphinx Riddle",
            Type = "riddle",
            Difficulty = 3,
            Description = "A riddle challenge"
        };

        _mockObstacleService
            .Setup(s => s.GetObstacleAsync("1"))
            .ReturnsAsync(obstacle);

        // Act
        var result = await _controller.GetObstacleById("1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<Obstacle>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal("Sphinx Riddle", response.Data.Name);
        Assert.Equal(3, response.Data.Difficulty);
    }

    [Fact]
    public async Task GetObstacleById_ReturnsNotFound_WhenDoesNotExist()
    {
        // Arrange
        _mockObstacleService
            .Setup(s => s.GetObstacleAsync("999"))
            .ReturnsAsync((Obstacle?)null);

        // Act
        var result = await _controller.GetObstacleById("999");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(notFoundResult.Value);
        Assert.Contains("not found", response.Error?.ToLower() ?? "");
    }

    [Fact]
    public async Task AttemptObstacle_SubmitsRiddleSolution_ReturnsSuccess()
    {
        // Arrange
        var request = new AttemptObstacleRequest
        {
            PlayerId = "player1",
            Solution = "river"
        };

        var expectedResult = new ObstacleResult
        {
            Success = true,
            Message = "Success! You have overcome the Sphinx's Riddle.",
            ExperienceGained = 100,
            GoldGained = 50,
            ItemsGained = new List<ItemReward>()
        };

        _mockObstacleService
            .Setup(s => s.AttemptObstacleAsync("player1", "1", "river"))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AttemptObstacle("1", request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<ObstacleResult>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.True(response.Data.Success);
        Assert.Equal(100, response.Data.ExperienceGained);
    }

    [Fact]
    public async Task AttemptObstacle_SubmitsRiddleSolution_ReturnsFailure()
    {
        // Arrange
        var request = new AttemptObstacleRequest
        {
            PlayerId = "player1",
            Solution = "wrong_answer"
        };

        var expectedResult = new ObstacleResult
        {
            Success = false,
            Message = "Attempt 1 failed. Try again (attempts remaining: 9)",
            ExperienceGained = 0,
            GoldGained = 0,
            ItemsGained = new List<ItemReward>()
        };

        _mockObstacleService
            .Setup(s => s.AttemptObstacleAsync("player1", "1", "wrong_answer"))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AttemptObstacle("1", request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<ObstacleResult>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.False(response.Data.Success);
        Assert.Equal(0, response.Data.ExperienceGained);
    }

    [Fact]
    public async Task GetPlayerAttempts_ReturnsAttemptHistory()
    {
        // Arrange
        var attempts = new List<ObstacleAttempt>
        {
            new()
            {
                Id = "att1",
                PlayerId = "player1",
                ObstacleId = "1",
                Success = false,
                AttemptNumber = 1,
                Timestamp = DateTime.UtcNow
            },
            new()
            {
                Id = "att2",
                PlayerId = "player1",
                ObstacleId = "1",
                Success = true,
                AttemptNumber = 2,
                Timestamp = DateTime.UtcNow.AddMinutes(5)
            }
        };

        _mockObstacleService
            .Setup(s => s.GetPlayerAttemptsAsync("player1", "1"))
            .ReturnsAsync(attempts);

        // Act
        var result = await _controller.GetPlayerAttempts("1", "player1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<List<ObstacleAttempt>>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Count);
        Assert.True(response.Data[0].Success);
    }

    [Fact]
    public async Task AttemptObstacle_ReturnsSuccess_WhenChestUnlocked()
    {
        // Arrange
        var request = new AttemptObstacleRequest
        {
            PlayerId = "player1",
            Solution = "bronze_key"
        };

        var expectedResult = new ObstacleResult
        {
            Success = true,
            Message = "Success! You have opened the Ancient Chest.",
            ExperienceGained = 250,
            GoldGained = 100,
            ItemsGained = new List<ItemReward>
            {
                new() { ItemId = "gold_coins", ItemName = "Gold Coins", Quantity = 200 }
            }
        };

        _mockObstacleService
            .Setup(s => s.AttemptObstacleAsync("player1", "chest1", "bronze_key"))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AttemptObstacle("chest1", request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<ObstacleResult>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.True(response.Data.Success);
        Assert.Single(response.Data.ItemsGained);
    }

    [Fact]
    public async Task GetObstacleChallenge_ReturnsRiddleChallenge()
    {
        // Arrange
        var obstacle = new Obstacle
        {
            Id = "1",
            Name = "Sphinx Riddle",
            Type = "riddle",
            Difficulty = 3,
            ChallengeData = new RiddleChallenge
            {
                Question = "What has a mouth but cannot speak?",
                CorrectAnswer = "river",
                Hints = new List<string> { "Think of something natural...", "It flows..." }
            }
        };

        _mockObstacleService
            .Setup(s => s.GetObstacleAsync("1"))
            .ReturnsAsync(obstacle);

        // Act
        var result = await _controller.GetObstacleChallenge("1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.NotNull(response.Data);
    }

    [Fact]
    public async Task GetObstacleHints_ReturnsHints_ForRiddle()
    {
        // Arrange
        var obstacle = new Obstacle
        {
            Id = "1",
            Name = "Sphinx Riddle",
            Type = "riddle",
            ChallengeData = new RiddleChallenge
            {
                Question = "What has a mouth but cannot speak?",
                CorrectAnswer = "river",
                Hints = new List<string>
                {
                    "Think of something natural...",
                    "It flows and moves...",
                    "Often found in valleys..."
                }
            }
        };

        _mockObstacleService
            .Setup(s => s.GetObstacleAsync("1"))
            .ReturnsAsync(obstacle);

        // Act
        var result = await _controller.GetObstacleHints("1", 0);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<List<string>>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("Think of something natural...", response.Data[0]);
    }

    [Fact]
    public async Task CreateObstacle_CreatesNewObstacle()
    {
        // Arrange
        var request = new CreateObstacleRequest
        {
            Name = "Test Riddle",
            Type = "riddle",
            Difficulty = 3,
            Description = "A test riddle",
            Location = "Test Location"
        };

        _mockObstacleCollection
            .Setup(c => c.InsertOneAsync(It.IsAny<Obstacle>(), null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateObstacle(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(ObstaclesController.GetObstacleById), createdResult.ActionName);
        
        var response = Assert.IsType<ApiResponse<Obstacle>>(createdResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal("Test Riddle", response.Data.Name);
        Assert.Equal("riddle", response.Data.Type);
    }

    [Fact]
    public async Task AttemptObstacle_ReturnsFailure_WhenInvalidChest()
    {
        // Arrange
        var request = new AttemptObstacleRequest
        {
            PlayerId = "player1",
            Solution = "wrong_key"
        };

        var expectedResult = new ObstacleResult
        {
            Success = false,
            Message = "Attempt 1 failed. Try again (attempts remaining: 9)"
        };

        _mockObstacleService
            .Setup(s => s.AttemptObstacleAsync("player1", "chest1", "wrong_key"))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AttemptObstacle("chest1", request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<ObstacleResult>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.False(response.Data.Success);
    }
}
