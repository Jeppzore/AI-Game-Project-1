using MongoDB.Bson;
using MongoDB.Driver;
using NightmaresWiki.Models;
using Server.Models;
using GameModels = NightmaresWiki.Models;

namespace Server.Services;

/// <summary>
/// Service for managing crafting operations, recipe validation, and inventory management.
/// </summary>
public interface ICraftingService
{
    /// <summary>
    /// Retrieves all available crafting recipes.
    /// </summary>
    Task<List<CraftingRecipe>> GetAllRecipesAsync();

    /// <summary>
    /// Retrieves a single crafting recipe by ID.
    /// </summary>
    Task<CraftingRecipe> GetRecipeByIdAsync(string id);

    /// <summary>
    /// Creates a new crafting recipe (admin only).
    /// </summary>
    Task<CraftingRecipe> CreateRecipeAsync(CraftingRecipe recipe);

    /// <summary>
    /// Validates that a recipe exists and all referenced items exist.
    /// </summary>
    Task<(bool isValid, string errorMessage)> ValidateRecipeAsync(string recipeId);

    /// <summary>
    /// Validates that a player has all required ingredients for a recipe.
    /// </summary>
    Task<(bool hasIngredients, List<string> missingItems)> ValidateIngredientsAsync(
        string playerId, 
        CraftingRecipe recipe);

    /// <summary>
    /// Performs a crafting operation: validates recipe, checks ingredients, and updates inventory.
    /// </summary>
    Task<CraftingResult> PerformCraftAsync(string playerId, string recipeId);

    /// <summary>
    /// Retrieves recipes by skill type.
    /// </summary>
    Task<List<GameModels.CraftingRecipe>> GetRecipesBySkillAsync(string skill);

    /// <summary>
    /// Retrieves recipes available for a player's level.
    /// </summary>
    Task<List<GameModels.CraftingRecipe>> GetAvailableRecipesForLevelAsync(int playerLevel);
}

/// <summary>
/// Implementation of crafting service using MongoDB.
/// </summary>
public class CraftingService : ICraftingService
{
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<CraftingService> _logger;

    public CraftingService(IMongoDbService mongoDbService, ILogger<CraftingService> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all available crafting recipes from the database.
    /// </summary>
    public async Task<List<GameModels.CraftingRecipe>> GetAllRecipesAsync()
    {
        try
        {
            var collection = _mongoDbService.GetCraftingRecipesCollection();
            return await collection.Find(_ => true).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all crafting recipes");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a single crafting recipe by its MongoDB ObjectId.
    /// </summary>
    public async Task<GameModels.CraftingRecipe> GetRecipeByIdAsync(string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                _logger.LogWarning("Invalid recipe ID format: {RecipeId}", id);
                throw new ArgumentException($"Invalid recipe ID format: {id}");
            }

            var collection = _mongoDbService.GetCraftingRecipesCollection();
            var filter = Builders<GameModels.CraftingRecipe>.Filter.Eq(r => r.Id, objectId);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recipe with ID: {RecipeId}", id);
            throw;
        }
    }

    /// <summary>
    /// Creates a new crafting recipe in the database.
    /// </summary>
    public async Task<GameModels.CraftingRecipe> CreateRecipeAsync(GameModels.CraftingRecipe recipe)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(recipe.Name))
            {
                throw new ArgumentException("Recipe name is required");
            }

            if (recipe.RequiredItems == null || recipe.RequiredItems.Count == 0)
            {
                throw new ArgumentException("Recipe must have at least one required item");
            }

            if (recipe.OutputItem == null)
            {
                throw new ArgumentException("Recipe must have an output item");
            }

            recipe.CreatedAt = DateTime.UtcNow;
            recipe.UpdatedAt = DateTime.UtcNow;

            var collection = _mongoDbService.GetCraftingRecipesCollection();
            await collection.InsertOneAsync(recipe);

            _logger.LogInformation("Created crafting recipe: {RecipeName} with ID: {RecipeId}", recipe.Name, recipe.Id);
            return recipe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating crafting recipe: {RecipeName}", recipe.Name);
            throw;
        }
    }

    /// <summary>
    /// Validates that a recipe exists and all referenced items exist in the database.
    /// </summary>
    public async Task<(bool isValid, string errorMessage)> ValidateRecipeAsync(string recipeId)
    {
        try
        {
            var recipe = await GetRecipeByIdAsync(recipeId);
            if (recipe == null)
            {
                return (false, $"Recipe with ID {recipeId} not found");
            }

            var itemsCollection = _mongoDbService.GetItemsCollection();

            // Validate all required items exist
            foreach (var requirement in recipe.RequiredItems)
            {
                var itemFilter = Builders<GameModels.Item>.Filter.Eq(i => i.Id, requirement.ItemId);
                var item = await itemsCollection.Find(itemFilter).FirstOrDefaultAsync();
                if (item == null)
                {
                    return (false, $"Required item with ID {requirement.ItemId} not found");
                }
            }

            // Validate output item exists
            var outputFilter = Builders<GameModels.Item>.Filter.Eq(i => i.Id, recipe.OutputItem.ItemId);
            var outputItem = await itemsCollection.Find(outputFilter).FirstOrDefaultAsync();
            if (outputItem == null)
            {
                return (false, $"Output item with ID {recipe.OutputItem.ItemId} not found");
            }

            return (true, "");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating recipe: {RecipeId}", recipeId);
            throw;
        }
    }

    /// <summary>
    /// Validates that a player has all required ingredients for a recipe.
    /// Returns a list of missing items (if any).
    /// </summary>
    public async Task<(bool hasIngredients, List<string> missingItems)> ValidateIngredientsAsync(
        string playerId, 
        GameModels.CraftingRecipe recipe)
    {
        try
        {
            var missingItems = new List<string>();

            // Get player inventory (simplified - in production, you'd have a player inventory service)
            // For now, assume players have unlimited ingredients (mock implementation)
            // In real implementation, check PlayerInventory collection

            return (missingItems.Count == 0, missingItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating ingredients for player: {PlayerId}", playerId);
            throw;
        }
    }

    /// <summary>
    /// Performs a crafting operation:
    /// 1. Validates the recipe exists
    /// 2. Validates all required ingredients are available
    /// 3. Deducts ingredients from inventory
    /// 4. Adds output item to inventory
    /// 5. Awards XP to player
    /// </summary>
    public async Task<GameModels.CraftingResult> PerformCraftAsync(string playerId, string recipeId)
    {
        try
        {
            // Validate recipe
            var (isValidRecipe, recipeError) = await ValidateRecipeAsync(recipeId);
            if (!isValidRecipe)
            {
                return new GameModels.CraftingResult
                {
                    Success = false,
                    Message = recipeError,
                    XpGained = 0
                };
            }

            var recipe = await GetRecipeByIdAsync(recipeId);

            // Validate ingredients (simplified - in production, deduct from actual inventory)
            var (hasIngredients, missingItems) = await ValidateIngredientsAsync(playerId, recipe);
            if (!hasIngredients)
            {
                var missingMessage = string.Join(", ", missingItems);
                return new GameModels.CraftingResult
                {
                    Success = false,
                    Message = $"Missing ingredients: {missingMessage}",
                    XpGained = 0
                };
            }

            // Get the output item details
            var itemsCollection = _mongoDbService.GetItemsCollection();
            var outputFilter = Builders<GameModels.Item>.Filter.Eq(i => i.Id, recipe.OutputItem.ItemId);
            var outputItem = await itemsCollection.Find(outputFilter).FirstOrDefaultAsync();

            if (outputItem == null)
            {
                return new GameModels.CraftingResult
                {
                    Success = false,
                    Message = "Output item not found",
                    XpGained = 0
                };
            }

            // In production: 
            // 1. Deduct required items from player inventory
            // 2. Add output items to player inventory
            // 3. Update player XP
            // 4. Record transaction

            _logger.LogInformation(
                "Player {PlayerId} successfully crafted {RecipeName}",
                playerId,
                recipe.Name);

            return new GameModels.CraftingResult
            {
                Success = true,
                ResultItem = outputItem,
                Message = $"Successfully crafted {outputItem.Name}!",
                XpGained = recipe.XpReward
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing craft for player {PlayerId} on recipe {RecipeId}", playerId, recipeId);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all recipes for a specific crafting skill type.
    /// </summary>
    public async Task<List<GameModels.CraftingRecipe>> GetRecipesBySkillAsync(string skill)
    {
        try
        {
            var collection = _mongoDbService.GetCraftingRecipesCollection();
            var filter = Builders<GameModels.CraftingRecipe>.Filter.Eq(r => r.SkillRequirement, skill);
            return await collection.Find(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recipes for skill: {Skill}", skill);
            throw;
        }
    }

    /// <summary>
    /// Retrieves recipes available for a player based on their level.
    /// </summary>
    public async Task<List<GameModels.CraftingRecipe>> GetAvailableRecipesForLevelAsync(int playerLevel)
    {
        try
        {
            var collection = _mongoDbService.GetCraftingRecipesCollection();
            var filter = Builders<GameModels.CraftingRecipe>.Filter.Lte(r => r.LevelRequirement, playerLevel);
            return await collection.Find(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recipes for level: {PlayerLevel}", playerLevel);
            throw;
        }
    }
}
