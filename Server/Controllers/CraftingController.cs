using Microsoft.AspNetCore.Mvc;
using NightmaresWiki.Models;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// API controller for crafting system operations.
/// Handles recipe management and crafting execution.
/// </summary>
[ApiController]
[Route("api/crafting")]
public class CraftingController : ControllerBase
{
    private readonly ICraftingService _craftingService;
    private readonly ILogger<CraftingController> _logger;

    public CraftingController(ICraftingService craftingService, ILogger<CraftingController> logger)
    {
        _craftingService = craftingService;
        _logger = logger;
    }

    /// <summary>
    /// GET: /api/crafting/recipes
    /// Retrieves all available crafting recipes.
    /// </summary>
    [HttpGet("recipes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<List<GameModels.CraftingRecipe>>>> GetAllRecipes()
    {
        try
        {
            var recipes = await _craftingService.GetAllRecipesAsync();
            return Ok(new ApiResponse<List<GameModels.CraftingRecipe>>
            {
                Data = recipes,
                Error = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all crafting recipes");
            return StatusCode(500, new ApiResponse<List<GameModels.CraftingRecipe>>
            {
                Data = null,
                Error = "Failed to retrieve crafting recipes"
            });
        }
    }

    /// <summary>
    /// GET: /api/crafting/recipes/{id}
    /// Retrieves a single crafting recipe by ID.
    /// </summary>
    [HttpGet("recipes/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<GameModels.CraftingRecipe>>> GetRecipeById(string id)
    {
        try
        {
            var recipe = await _craftingService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound(new ApiResponse<GameModels.CraftingRecipe>
                {
                    Data = null,
                    Error = $"Recipe with ID {id} not found"
                });
            }

            return Ok(new ApiResponse<GameModels.CraftingRecipe>
            {
                Data = recipe,
                Error = null
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid recipe ID format: {RecipeId}", id);
            return BadRequest(new ApiResponse<GameModels.CraftingRecipe>
            {
                Data = null,
                Error = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recipe with ID: {RecipeId}", id);
            return StatusCode(500, new ApiResponse<GameModels.CraftingRecipe>
            {
                Data = null,
                Error = "Failed to retrieve recipe"
            });
        }
    }

    /// <summary>
    /// POST: /api/crafting/recipes
    /// Creates a new crafting recipe (admin only in production).
    /// </summary>
    [HttpPost("recipes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<GameModels.CraftingRecipe>>> CreateRecipe(
        [FromBody] GameModels.CraftingRecipe recipe)
    {
        try
        {
            if (recipe == null)
            {
                return BadRequest(new ApiResponse<GameModels.CraftingRecipe>
                {
                    Data = null,
                    Error = "Recipe data is required"
                });
            }

            var createdRecipe = await _craftingService.CreateRecipeAsync(recipe);

            return CreatedAtAction(nameof(GetRecipeById), new { id = createdRecipe.Id.ToString() },
                new ApiResponse<GameModels.CraftingRecipe>
                {
                    Data = createdRecipe,
                    Error = null
                });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid recipe data");
            return BadRequest(new ApiResponse<GameModels.CraftingRecipe>
            {
                Data = null,
                Error = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating crafting recipe");
            return StatusCode(500, new ApiResponse<GameModels.CraftingRecipe>
            {
                Data = null,
                Error = "Failed to create recipe"
            });
        }
    }

    /// <summary>
    /// GET: /api/crafting/recipes/by-skill/{skill}
    /// Retrieves all recipes for a specific crafting skill.
    /// </summary>
    [HttpGet("recipes/by-skill/{skill}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<List<GameModels.CraftingRecipe>>>> GetRecipesBySkill(string skill)
    {
        try
        {
            var recipes = await _craftingService.GetRecipesBySkillAsync(skill);
            return Ok(new ApiResponse<List<GameModels.CraftingRecipe>>
            {
                Data = recipes,
                Error = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recipes for skill: {Skill}", skill);
            return StatusCode(500, new ApiResponse<List<GameModels.CraftingRecipe>>
            {
                Data = null,
                Error = $"Failed to retrieve recipes for skill {skill}"
            });
        }
    }

    /// <summary>
    /// GET: /api/crafting/recipes/by-level/{level}
    /// Retrieves all recipes available for a player at a specific level.
    /// </summary>
    [HttpGet("recipes/by-level/{level}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<List<GameModels.CraftingRecipe>>>> GetRecipesByLevel(int level)
    {
        try
        {
            if (level < 0)
            {
                return BadRequest(new ApiResponse<List<GameModels.CraftingRecipe>>
                {
                    Data = null,
                    Error = "Level must be non-negative"
                });
            }

            var recipes = await _craftingService.GetAvailableRecipesForLevelAsync(level);
            return Ok(new ApiResponse<List<GameModels.CraftingRecipe>>
            {
                Data = recipes,
                Error = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recipes for level: {Level}", level);
            return StatusCode(500, new ApiResponse<List<GameModels.CraftingRecipe>>
            {
                Data = null,
                Error = $"Failed to retrieve recipes for level {level}"
            });
        }
    }

    /// <summary>
    /// POST: /api/crafting/craft
    /// Performs a crafting operation for a player.
    /// Body: { playerId: string, recipeId: string }
    /// </summary>
    [HttpPost("craft")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<GameModels.CraftingResult>>> PerformCraft(
        [FromBody] CraftRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrWhiteSpace(request.PlayerId) || 
                string.IsNullOrWhiteSpace(request.RecipeId))
            {
                return BadRequest(new ApiResponse<GameModels.CraftingResult>
                {
                    Data = null,
                    Error = "PlayerId and RecipeId are required"
                });
            }

            var result = await _craftingService.PerformCraftAsync(request.PlayerId, request.RecipeId);

            if (!result.Success)
            {
                return BadRequest(new ApiResponse<GameModels.CraftingResult>
                {
                    Data = result,
                    Error = result.Message
                });
            }

            return Ok(new ApiResponse<GameModels.CraftingResult>
            {
                Data = result,
                Error = null
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid craft request");
            return BadRequest(new ApiResponse<GameModels.CraftingResult>
            {
                Data = null,
                Error = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing craft");
            return StatusCode(500, new ApiResponse<GameModels.CraftingResult>
            {
                Data = null,
                Error = "Failed to perform craft operation"
            });
        }
    }
}

/// <summary>
/// Request model for crafting operations.
/// </summary>
public class CraftRequest
{
    /// <summary>
    /// The ID of the player performing the craft.
    /// </summary>
    public string PlayerId { get; set; }

    /// <summary>
    /// The ID of the recipe to craft.
    /// </summary>
    public string RecipeId { get; set; }
}
