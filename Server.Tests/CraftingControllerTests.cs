using MongoDB.Bson;
using Moq;
using NightmaresWiki.Models;
using Server.Controllers;
using Server.Services;
using Xunit;

namespace Server.Tests;

/// <summary>
/// Unit tests for CraftingController and CraftingService.
/// </summary>
public class CraftingControllerTests
{
    private readonly Mock<ICraftingService> _mockCraftingService;
    private readonly Mock<ILogger<CraftingController>> _mockLogger;
    private readonly CraftingController _controller;

    public CraftingControllerTests()
    {
        _mockCraftingService = new Mock<ICraftingService>();
        _mockLogger = new Mock<ILogger<CraftingController>>();
        _controller = new CraftingController(_mockCraftingService.Object, _mockLogger.Object);
    }

    #region GetAllRecipes Tests

    [Fact]
    public async Task GetAllRecipes_ReturnsOkResult_WithRecipeList()
    {
        // Arrange
        var recipes = new List<GameModels.CraftingRecipe>
        {
            new GameModels.CraftingRecipe
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Iron Sword",
                Description = "A basic sword",
                LevelRequirement = 5,
                SkillRequirement = "Smithing",
                SkillLevel = 10,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _mockCraftingService.Setup(s => s.GetAllRecipesAsync())
            .ReturnsAsync(recipes);

        // Act
        var result = await _controller.GetAllRecipes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var response = Assert.IsType<ApiResponse<List<GameModels.CraftingRecipe>>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("Iron Sword", response.Data[0].Name);
    }

    [Fact]
    public async Task GetAllRecipes_ReturnsEmptyList_WhenNoRecipes()
    {
        // Arrange
        var recipes = new List<GameModels.CraftingRecipe>();
        _mockCraftingService.Setup(s => s.GetAllRecipesAsync())
            .ReturnsAsync(recipes);

        // Act
        var result = await _controller.GetAllRecipes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<List<GameModels.CraftingRecipe>>>(okResult.Value);
        Assert.Empty(response.Data);
    }

    [Fact]
    public async Task GetAllRecipes_ReturnServerError_OnException()
    {
        // Arrange
        _mockCraftingService.Setup(s => s.GetAllRecipesAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetAllRecipes();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    #endregion

    #region GetRecipeById Tests

    [Fact]
    public async Task GetRecipeById_ReturnsOkResult_WhenRecipeExists()
    {
        // Arrange
        var recipeId = ObjectId.GenerateNewId().ToString();
        var recipe = new GameModels.CraftingRecipe
        {
            Id = ObjectId.Parse(recipeId),
            Name = "Health Potion",
            Description = "Restores health",
            LevelRequirement = 1,
            SkillRequirement = "Alchemy",
            SkillLevel = 5,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mockCraftingService.Setup(s => s.GetRecipeByIdAsync(recipeId))
            .ReturnsAsync(recipe);

        // Act
        var result = await _controller.GetRecipeById(recipeId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var response = Assert.IsType<ApiResponse<GameModels.CraftingRecipe>>(okResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal("Health Potion", response.Data.Name);
    }

    [Fact]
    public async Task GetRecipeById_ReturnsNotFound_WhenRecipeDoesNotExist()
    {
        // Arrange
        var recipeId = ObjectId.GenerateNewId().ToString();
        _mockCraftingService.Setup(s => s.GetRecipeByIdAsync(recipeId))
            .ReturnsAsync((GameModels.CraftingRecipe)null);

        // Act
        var result = await _controller.GetRecipeById(recipeId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);

        var response = Assert.IsType<ApiResponse<GameModels.CraftingRecipe>>(notFoundResult.Value);
        Assert.Null(response.Data);
        Assert.Contains("not found", response.Error);
    }

    [Fact]
    public async Task GetRecipeById_ReturnsBadRequest_WithInvalidIdFormat()
    {
        // Arrange
        var invalidId = "invalid-id-format";
        _mockCraftingService.Setup(s => s.GetRecipeByIdAsync(invalidId))
            .ThrowsAsync(new ArgumentException("Invalid recipe ID format"));

        // Act
        var result = await _controller.GetRecipeById(invalidId);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badResult.StatusCode);
    }

    #endregion

    #region CreateRecipe Tests

    [Fact]
    public async Task CreateRecipe_ReturnsCreatedAtAction_WithNewRecipe()
    {
        // Arrange
        var newRecipe = new GameModels.CraftingRecipe
        {
            Name = "Enchanted Ring",
            Description = "A magical ring",
            LevelRequirement = 15,
            SkillRequirement = "Enchanting",
            SkillLevel = 20,
            RequiredItems = new List<GameModels.CraftingRequirement>
            {
                new GameModels.CraftingRequirement { ItemId = ObjectId.GenerateNewId(), Quantity = 1 }
            },
            OutputItem = new GameModels.CraftingOutput { ItemId = ObjectId.GenerateNewId(), Quantity = 1 },
            XpReward = 250
        };

        var createdRecipe = new GameModels.CraftingRecipe
        {
            Id = ObjectId.GenerateNewId(),
            Name = newRecipe.Name,
            Description = newRecipe.Description,
            LevelRequirement = newRecipe.LevelRequirement,
            SkillRequirement = newRecipe.SkillRequirement,
            SkillLevel = newRecipe.SkillLevel,
            RequiredItems = newRecipe.RequiredItems,
            OutputItem = newRecipe.OutputItem,
            XpReward = newRecipe.XpReward,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mockCraftingService.Setup(s => s.CreateRecipeAsync(It.IsAny<GameModels.CraftingRecipe>()))
            .ReturnsAsync(createdRecipe);

        // Act
        var result = await _controller.CreateRecipe(newRecipe);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(nameof(CraftingController.GetRecipeById), createdResult.ActionName);

        var response = Assert.IsType<ApiResponse<GameModels.CraftingRecipe>>(createdResult.Value);
        Assert.NotNull(response.Data);
        Assert.Equal("Enchanted Ring", response.Data.Name);
    }

    [Fact]
    public async Task CreateRecipe_ReturnsBadRequest_WithNullRecipe()
    {
        // Act
        var result = await _controller.CreateRecipe(null);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badResult.StatusCode);
    }

    [Fact]
    public async Task CreateRecipe_ReturnsBadRequest_WithInvalidData()
    {
        // Arrange
        var invalidRecipe = new GameModels.CraftingRecipe
        {
            Name = "", // Empty name is invalid
            RequiredItems = new List<GameModels.CraftingRequirement>()
        };

        _mockCraftingService.Setup(s => s.CreateRecipeAsync(It.IsAny<GameModels.CraftingRecipe>()))
            .ThrowsAsync(new ArgumentException("Recipe name is required"));

        // Act
        var result = await _controller.CreateRecipe(invalidRecipe);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badResult.StatusCode);
    }

    #endregion

    #region GetRecipesBySkill Tests

    [Fact]
    public async Task GetRecipesBySkill_ReturnsOkResult_WithSkillRecipes()
    {
        // Arrange
        var skill = "Smithing";
        var recipes = new List<GameModels.CraftingRecipe>
        {
            new GameModels.CraftingRecipe
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Iron Sword",
                SkillRequirement = "Smithing",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new GameModels.CraftingRecipe
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Iron Shield",
                SkillRequirement = "Smithing",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _mockCraftingService.Setup(s => s.GetRecipesBySkillAsync(skill))
            .ReturnsAsync(recipes);

        // Act
        var result = await _controller.GetRecipesBySkill(skill);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<List<GameModels.CraftingRecipe>>>(okResult.Value);
        Assert.Equal(2, response.Data.Count);
        Assert.All(response.Data, r => Assert.Equal(skill, r.SkillRequirement));
    }

    #endregion

    #region GetRecipesByLevel Tests

    [Fact]
    public async Task GetRecipesByLevel_ReturnsOkResult_WithLevelRecipes()
    {
        // Arrange
        int level = 10;
        var recipes = new List<GameModels.CraftingRecipe>
        {
            new GameModels.CraftingRecipe
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Basic Potion",
                LevelRequirement = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new GameModels.CraftingRecipe
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Advanced Potion",
                LevelRequirement = 8,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _mockCraftingService.Setup(s => s.GetAvailableRecipesForLevelAsync(level))
            .ReturnsAsync(recipes);

        // Act
        var result = await _controller.GetRecipesByLevel(level);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<List<GameModels.CraftingRecipe>>>(okResult.Value);
        Assert.Equal(2, response.Data.Count);
    }

    [Fact]
    public async Task GetRecipesByLevel_ReturnsBadRequest_WithNegativeLevel()
    {
        // Act
        var result = await _controller.GetRecipesByLevel(-1);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badResult.StatusCode);
    }

    #endregion

    #region PerformCraft Tests

    [Fact]
    public async Task PerformCraft_ReturnsOkResult_WhenCraftingSucceeds()
    {
        // Arrange
        var request = new CraftRequest
        {
            PlayerId = "player-123",
            RecipeId = ObjectId.GenerateNewId().ToString()
        };

        var craftResult = new GameModels.CraftingResult
        {
            Success = true,
            Message = "Successfully crafted Iron Sword!",
            ResultItem = new Item { Id = ObjectId.GenerateNewId(), Name = "Iron Sword" },
            XpGained = 100
        };

        _mockCraftingService.Setup(s => s.PerformCraftAsync(request.PlayerId, request.RecipeId))
            .ReturnsAsync(craftResult);

        // Act
        var result = await _controller.PerformCraft(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var response = Assert.IsType<ApiResponse<GameModels.CraftingResult>>(okResult.Value);
        Assert.True(response.Data.Success);
        Assert.Equal("Iron Sword", response.Data.ResultItem.Name);
        Assert.Equal(100, response.Data.XpGained);
    }

    [Fact]
    public async Task PerformCraft_ReturnsBadRequest_WhenMissingIngredients()
    {
        // Arrange
        var request = new CraftRequest
        {
            PlayerId = "player-123",
            RecipeId = ObjectId.GenerateNewId().ToString()
        };

        var craftResult = new GameModels.CraftingResult
        {
            Success = false,
            Message = "Missing ingredients: Iron Ore",
            XpGained = 0
        };

        _mockCraftingService.Setup(s => s.PerformCraftAsync(request.PlayerId, request.RecipeId))
            .ReturnsAsync(craftResult);

        // Act
        var result = await _controller.PerformCraft(request);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<GameModels.CraftingResult>>(badResult.Value);
        Assert.False(response.Data.Success);
        Assert.Contains("Missing ingredients", response.Data.Message);
    }

    [Fact]
    public async Task PerformCraft_ReturnsBadRequest_WithNullRequest()
    {
        // Act
        var result = await _controller.PerformCraft(null);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badResult.StatusCode);
    }

    [Fact]
    public async Task PerformCraft_ReturnsBadRequest_WithMissingPlayerId()
    {
        // Arrange
        var request = new CraftRequest
        {
            PlayerId = "",
            RecipeId = ObjectId.GenerateNewId().ToString()
        };

        // Act
        var result = await _controller.PerformCraft(request);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badResult.StatusCode);
    }

    [Fact]
    public async Task PerformCraft_ReturnsBadRequest_WithMissingRecipeId()
    {
        // Arrange
        var request = new CraftRequest
        {
            PlayerId = "player-123",
            RecipeId = ""
        };

        // Act
        var result = await _controller.PerformCraft(request);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badResult.StatusCode);
    }

    #endregion
}

/// <summary>
/// Unit tests for CraftingService.
/// </summary>
public class CraftingServiceTests
{
    private readonly Mock<IMongoDbService> _mockMongoDbService;
    private readonly Mock<ILogger<CraftingService>> _mockLogger;
    private readonly CraftingService _service;

    public CraftingServiceTests()
    {
        _mockMongoDbService = new Mock<IMongoDbService>();
        _mockLogger = new Mock<ILogger<CraftingService>>();
        _service = new CraftingService(_mockMongoDbService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateRecipeAsync_ThrowsException_WithEmptyName()
    {
        // Arrange
        var recipe = new GameModels.CraftingRecipe
        {
            Name = "",
            RequiredItems = new List<GameModels.CraftingRequirement>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateRecipeAsync(recipe));
    }

    [Fact]
    public async Task CreateRecipeAsync_ThrowsException_WithNoRequiredItems()
    {
        // Arrange
        var recipe = new GameModels.CraftingRecipe
        {
            Name = "Test Recipe",
            RequiredItems = new List<GameModels.CraftingRequirement>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateRecipeAsync(recipe));
    }

    [Fact]
    public async Task CreateRecipeAsync_ThrowsException_WithNullOutputItem()
    {
        // Arrange
        var recipe = new GameModels.CraftingRecipe
        {
            Name = "Test Recipe",
            RequiredItems = new List<GameModels.CraftingRequirement>
            {
                new GameModels.CraftingRequirement { ItemId = ObjectId.GenerateNewId(), Quantity = 1 }
            },
            OutputItem = null
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateRecipeAsync(recipe));
    }
}
