using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MongoDB.Bson;
using MongoDB.Driver;
using Server.Controllers;
using Server.Models;
using Server.Services;
using Xunit;

namespace Server.Tests;

public class QuestsControllerTests
{
    private readonly Mock<IQuestService> _mockQuestService;
    private readonly Mock<IMongoDbService> _mockMongoDbService;
    private readonly Mock<IMongoCollection<QuestProgress>> _mockQuestProgressCollection;
    private readonly Mock<ILogger<QuestsController>> _mockLogger;
    private readonly QuestsController _controller;

    public QuestsControllerTests()
    {
        _mockQuestService = new Mock<IQuestService>();
        _mockMongoDbService = new Mock<IMongoDbService>();
        _mockQuestProgressCollection = new Mock<IMongoCollection<QuestProgress>>();
        _mockLogger = new Mock<ILogger<QuestsController>>();

        _mockMongoDbService
            .Setup(s => s.GetQuestProgressCollection())
            .Returns(_mockQuestProgressCollection.Object);

        _controller = new QuestsController(
            _mockQuestService.Object,
            _mockMongoDbService.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetAvailableQuests_ReturnsOk_WithListOfQuests()
    {
        // Arrange
        var quest = new Quest
        {
            Id = ObjectId.GenerateNewId(),
            QuestId = "test_quest",
            Title = "Test Quest",
            Description = "A test quest",
            LevelRequirement = 5
        };

        var questDetails = new QuestDetailsDto
        {
            Id = quest.Id,
            QuestId = quest.QuestId,
            Title = quest.Title,
            Description = quest.Description,
            LevelRequirement = quest.LevelRequirement,
            Objectives = new List<ObjectiveDto>(),
            Rewards = new List<QuestRewardDto>()
        };

        _mockQuestService
            .Setup(s => s.GetAvailableQuestsAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Quest> { quest });

        _mockQuestService
            .Setup(s => s.GetQuestDetailsAsync(quest.Id))
            .ReturnsAsync(questDetails);

        // Act
        var result = await _controller.GetAvailableQuests();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);
        var response = Assert.IsType<ApiResponse<List<QuestDetailsDto>>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Single(response.Data);
    }

    [Fact]
    public async Task GetQuestById_ReturnsQuest_WhenValidIdProvided()
    {
        // Arrange
        var questId = ObjectId.GenerateNewId();
        var questDetails = new QuestDetailsDto
        {
            Id = questId,
            QuestId = "test_quest",
            Title = "Test Quest",
            Description = "A test quest",
            LevelRequirement = 5,
            Objectives = new List<ObjectiveDto>(),
            Rewards = new List<QuestRewardDto>()
        };

        _mockQuestService
            .Setup(s => s.GetQuestDetailsAsync(questId))
            .ReturnsAsync(questDetails);

        // Act
        var result = await _controller.GetQuestById(questId.ToString());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<QuestDetailsDto>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal("Test Quest", response.Data.Title);
    }

    [Fact]
    public async Task GetQuestById_ReturnsBadRequest_WhenInvalidIdProvided()
    {
        // Act
        var result = await _controller.GetQuestById("invalid_id");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<QuestDetailsDto>>(badRequestResult.Value);
        Assert.False(response.Success);
    }

    [Fact]
    public async Task GetQuestById_ReturnsNotFound_WhenQuestDoesNotExist()
    {
        // Arrange
        var questId = ObjectId.GenerateNewId();
        _mockQuestService
            .Setup(s => s.GetQuestDetailsAsync(questId))
            .ReturnsAsync((QuestDetailsDto)null);

        // Act
        var result = await _controller.GetQuestById(questId.ToString());

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<QuestDetailsDto>>(notFoundResult.Value);
        Assert.False(response.Success);
    }

    [Fact]
    public async Task AcceptQuest_ReturnsCreated_WhenQuestAccepted()
    {
        // Arrange
        var questId = ObjectId.GenerateNewId();
        var questProgress = new QuestProgress
        {
            Id = ObjectId.GenerateNewId(),
            PlayerId = "player_1",
            QuestId = questId,
            QuestName = "Test Quest",
            Status = "accepted",
            ObjectivesProgress = new List<ObjectiveProgress>(),
            StartedAt = DateTime.UtcNow
        };

        _mockQuestService
            .Setup(s => s.ValidateQuestAccessAsync(It.IsAny<string>(), questId))
            .ReturnsAsync(true);

        _mockQuestService
            .Setup(s => s.AcceptQuestAsync(It.IsAny<string>(), questId))
            .ReturnsAsync(questProgress);

        // Act
        var result = await _controller.AcceptQuest(questId.ToString());

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.NotNull(createdResult);
        var response = Assert.IsType<ApiResponse<QuestProgressDto>>(createdResult.Value);
        Assert.True(response.Success);
        Assert.Equal("accepted", response.Data.Status);
    }

    [Fact]
    public async Task AcceptQuest_ReturnsBadRequest_WhenCannotAccess()
    {
        // Arrange
        var questId = ObjectId.GenerateNewId();
        _mockQuestService
            .Setup(s => s.ValidateQuestAccessAsync(It.IsAny<string>(), questId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.AcceptQuest(questId.ToString());

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<QuestProgressDto>>(badRequestResult.Value);
        Assert.False(response.Success);
    }

    [Fact]
    public async Task UpdateQuestProgress_ReturnsOk_WhenProgressUpdated()
    {
        // Arrange
        var questId = ObjectId.GenerateNewId();
        var objectiveId = ObjectId.GenerateNewId();
        var request = new UpdateQuestProgressRequest
        {
            PlayerId = "player_1",
            ObjectiveId = objectiveId,
            ProgressAmount = 1
        };

        var questProgress = new QuestProgress
        {
            Id = ObjectId.GenerateNewId(),
            PlayerId = "player_1",
            QuestId = questId,
            QuestName = "Test Quest",
            Status = "in_progress",
            ObjectivesProgress = new List<ObjectiveProgress>
            {
                new ObjectiveProgress
                {
                    ObjectiveId = objectiveId,
                    ObjectiveType = "kill_enemy",
                    Completed = false,
                    Progress = 1,
                    Target = 5
                }
            }
        };

        _mockQuestService
            .Setup(s => s.UpdateObjectiveProgressAsync(It.IsAny<string>(), questId, objectiveId, 1))
            .Returns(Task.CompletedTask);

        var asyncCursor = new Mock<IAsyncCursor<QuestProgress>>();
        asyncCursor.Setup(c => c.Current).Returns(new List<QuestProgress> { questProgress });
        asyncCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockQuestProgressCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<QuestProgress>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursor.Object);

        // Act
        var result = await _controller.UpdateQuestProgress(questId.ToString(), request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<QuestProgressDto>>(okResult.Value);
        Assert.True(response.Success);
    }

    [Fact]
    public async Task CompleteQuest_ReturnsOk_WhenQuestCompleted()
    {
        // Arrange
        var questId = ObjectId.GenerateNewId();
        var objectiveId = ObjectId.GenerateNewId();
        var questProgress = new QuestProgress
        {
            Id = ObjectId.GenerateNewId(),
            PlayerId = "player_1",
            QuestId = questId,
            QuestName = "Test Quest",
            Status = "completed",
            ObjectivesProgress = new List<ObjectiveProgress>
            {
                new ObjectiveProgress
                {
                    ObjectiveId = objectiveId,
                    ObjectiveType = "kill_enemy",
                    Completed = true,
                    Progress = 5,
                    Target = 5
                }
            },
            CompletedAt = DateTime.UtcNow
        };

        _mockQuestService
            .Setup(s => s.CompleteQuestAsync(It.IsAny<string>(), questId))
            .ReturnsAsync(true);

        var asyncCursor = new Mock<IAsyncCursor<QuestProgress>>();
        asyncCursor.Setup(c => c.Current).Returns(new List<QuestProgress> { questProgress });
        asyncCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockQuestProgressCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<QuestProgress>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursor.Object);

        // Act
        var result = await _controller.CompleteQuest(questId.ToString());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<QuestProgressDto>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal("completed", response.Data.Status);
    }

    [Fact]
    public async Task CompleteQuest_ReturnsBadRequest_WhenNotAllObjectivesComplete()
    {
        // Arrange
        var questId = ObjectId.GenerateNewId();
        _mockQuestService
            .Setup(s => s.CompleteQuestAsync(It.IsAny<string>(), questId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.CompleteQuest(questId.ToString());

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<QuestProgressDto>>(badRequestResult.Value);
        Assert.False(response.Success);
    }

    [Fact]
    public async Task GetPlayerQuestLog_ReturnsOk_WithPlayerQuests()
    {
        // Arrange
        var questProgress = new QuestProgress
        {
            Id = ObjectId.GenerateNewId(),
            PlayerId = "player_1",
            QuestId = ObjectId.GenerateNewId(),
            QuestName = "Test Quest",
            Status = "in_progress",
            ObjectivesProgress = new List<ObjectiveProgress>()
        };

        _mockQuestService
            .Setup(s => s.GetPlayerQuestProgressAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<QuestProgress> { questProgress });

        // Act
        var result = await _controller.GetPlayerQuestLog("player_1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<List<QuestProgressDto>>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Single(response.Data);
    }

    [Fact]
    public async Task GetQuestProgress_ReturnsOk_WhenProgressExists()
    {
        // Arrange
        var questId = ObjectId.GenerateNewId();
        var questProgress = new QuestProgress
        {
            Id = ObjectId.GenerateNewId(),
            PlayerId = "player_1",
            QuestId = questId,
            QuestName = "Test Quest",
            Status = "in_progress",
            ObjectivesProgress = new List<ObjectiveProgress>()
        };

        var asyncCursor = new Mock<IAsyncCursor<QuestProgress>>();
        asyncCursor.Setup(c => c.Current).Returns(new List<QuestProgress> { questProgress });
        asyncCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockQuestProgressCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<QuestProgress>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursor.Object);

        // Act
        var result = await _controller.GetQuestProgress(questId.ToString());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<QuestProgressDto>>(okResult.Value);
        Assert.True(response.Success);
    }
}

public class QuestServiceTests
{
    private readonly Mock<IMongoDbService> _mockMongoDbService;
    private readonly Mock<ILogger<QuestService>> _mockLogger;
    private readonly Mock<IMongoCollection<Quest>> _mockQuestsCollection;
    private readonly Mock<IMongoCollection<Objective>> _mockObjectivesCollection;
    private readonly Mock<IMongoCollection<QuestProgress>> _mockProgressCollection;
    private readonly Mock<IMongoCollection<QuestReward>> _mockRewardsCollection;
    private readonly QuestService _service;

    public QuestServiceTests()
    {
        _mockMongoDbService = new Mock<IMongoDbService>();
        _mockLogger = new Mock<ILogger<QuestService>>();
        _mockQuestsCollection = new Mock<IMongoCollection<Quest>>();
        _mockObjectivesCollection = new Mock<IMongoCollection<Objective>>();
        _mockProgressCollection = new Mock<IMongoCollection<QuestProgress>>();
        _mockRewardsCollection = new Mock<IMongoCollection<QuestReward>>();

        _mockMongoDbService.Setup(s => s.GetQuestsCollection()).Returns(_mockQuestsCollection.Object);
        _mockMongoDbService.Setup(s => s.GetObjectivesCollection()).Returns(_mockObjectivesCollection.Object);
        _mockMongoDbService.Setup(s => s.GetQuestProgressCollection()).Returns(_mockProgressCollection.Object);
        _mockMongoDbService.Setup(s => s.GetQuestRewardsCollection()).Returns(_mockRewardsCollection.Object);

        _service = new QuestService(_mockMongoDbService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task AcceptQuestAsync_CreatesQuestProgress_WhenQuestExists()
    {
        // Arrange
        var playerId = "player_1";
        var questId = ObjectId.GenerateNewId();
        var quest = new Quest
        {
            Id = questId,
            QuestId = "test_quest",
            Title = "Test Quest",
            LevelRequirement = 5,
            ObjectiveIds = new List<ObjectId>()
        };

        var objectives = new List<Objective>();

        var asyncQuestCursor = new Mock<IAsyncCursor<Quest>>();
        asyncQuestCursor.Setup(c => c.Current).Returns(new List<Quest> { quest });
        asyncQuestCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        var asyncProgressCursor = new Mock<IAsyncCursor<QuestProgress>>();
        asyncProgressCursor.Setup(c => c.Current).Returns(new List<QuestProgress>());
        asyncProgressCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var asyncObjectivesCursor = new Mock<IAsyncCursor<Objective>>();
        asyncObjectivesCursor.Setup(c => c.Current).Returns(objectives);
        asyncObjectivesCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockQuestsCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Quest>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncQuestCursor.Object);

        _mockProgressCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<QuestProgress>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncProgressCursor.Object);

        _mockObjectivesCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Objective>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncObjectivesCursor.Object);

        _mockProgressCollection
            .Setup(c => c.InsertOneAsync(It.IsAny<QuestProgress>(), null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AcceptQuestAsync(playerId, questId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(playerId, result.PlayerId);
        Assert.Equal(questId, result.QuestId);
        Assert.Equal("accepted", result.Status);
    }

    [Fact]
    public async Task UpdateObjectiveProgressAsync_IncreasesProgress()
    {
        // Arrange
        var playerId = "player_1";
        var questId = ObjectId.GenerateNewId();
        var objectiveId = ObjectId.GenerateNewId();
        var questProgress = new QuestProgress
        {
            Id = ObjectId.GenerateNewId(),
            PlayerId = playerId,
            QuestId = questId,
            Status = "accepted",
            ObjectivesProgress = new List<ObjectiveProgress>
            {
                new ObjectiveProgress
                {
                    ObjectiveId = objectiveId,
                    Progress = 0,
                    Target = 5
                }
            }
        };

        var asyncCursor = new Mock<IAsyncCursor<QuestProgress>>();
        asyncCursor.Setup(c => c.Current).Returns(new List<QuestProgress> { questProgress });
        asyncCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockProgressCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<QuestProgress>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursor.Object);

        _mockProgressCollection
            .Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<QuestProgress>>(), It.IsAny<QuestProgress>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, null));

        // Act
        await _service.UpdateObjectiveProgressAsync(playerId, questId, objectiveId, 1);

        // Assert
        _mockProgressCollection.Verify(c => c.ReplaceOneAsync(
            It.IsAny<FilterDefinition<QuestProgress>>(),
            It.IsAny<QuestProgress>(),
            null,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ArePrerequisitesMetAsync_ReturnsFalse_WhenPrerequisiteNotCompleted()
    {
        // Arrange
        var playerId = "player_1";
        var questId = ObjectId.GenerateNewId();
        var prerequisiteQuestId = ObjectId.GenerateNewId();
        var quest = new Quest
        {
            Id = questId,
            PrerequisiteQuestIds = new List<ObjectId> { prerequisiteQuestId }
        };

        var asyncQuestCursor = new Mock<IAsyncCursor<Quest>>();
        asyncQuestCursor.Setup(c => c.Current).Returns(new List<Quest> { quest });
        asyncQuestCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        var asyncProgressCursor = new Mock<IAsyncCursor<QuestProgress>>();
        asyncProgressCursor.Setup(c => c.Current).Returns(new List<QuestProgress>());
        asyncProgressCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockQuestsCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Quest>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncQuestCursor.Object);

        _mockProgressCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<QuestProgress>>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncProgressCursor.Object);

        // Act
        var result = await _service.ArePrerequisitesMetAsync(playerId, questId);

        // Assert
        Assert.False(result);
    }
}
