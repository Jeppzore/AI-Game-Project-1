using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers;

/// <summary>
/// Manages enumeration and category endpoints for wiki entities.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get all creature types.
    /// </summary>
    [HttpGet("creature-types")]
    public ActionResult<ApiResponse<List<string>>> GetCreatureTypes()
    {
        try
        {
            var types = new List<string>
            {
                "normal",
                "elite",
                "boss",
                "miniboss",
                "unique"
            };

            return Ok(new ApiResponse<List<string>> { Data = types });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching creature types");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching creature types"
            });
        }
    }

    /// <summary>
    /// Get all creature difficulties.
    /// </summary>
    [HttpGet("creature-difficulties")]
    public ActionResult<ApiResponse<List<string>>> GetCreatureDifficulties()
    {
        try
        {
            var difficulties = new List<string>
            {
                "easy",
                "medium",
                "hard",
                "expert",
                "nightmare"
            };

            return Ok(new ApiResponse<List<string>> { Data = difficulties });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching creature difficulties");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching creature difficulties"
            });
        }
    }

    /// <summary>
    /// Get all item types.
    /// </summary>
    [HttpGet("item-types")]
    public ActionResult<ApiResponse<List<string>>> GetItemTypes()
    {
        try
        {
            var types = new List<string>
            {
                "weapon",
                "armor",
                "tool",
                "consumable",
                "quest",
                "accessory"
            };

            return Ok(new ApiResponse<List<string>> { Data = types });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching item types");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching item types"
            });
        }
    }

    /// <summary>
    /// Get all item categories.
    /// </summary>
    [HttpGet("item-categories")]
    public ActionResult<ApiResponse<List<string>>> GetItemCategories()
    {
        try
        {
            var categories = new List<string>
            {
                "helmets",
                "chests",
                "legs",
                "feet",
                "hands",
                "back",
                "swords",
                "axes",
                "hammers",
                "maces",
                "spears",
                "bows",
                "crossbows",
                "rings",
                "amulets",
                "potions",
                "gems",
                "valuables",
                "tools",
                "miscellaneous"
            };

            return Ok(new ApiResponse<List<string>> { Data = categories });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching item categories");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching item categories"
            });
        }
    }

    /// <summary>
    /// Get all item rarities.
    /// </summary>
    [HttpGet("item-rarities")]
    public ActionResult<ApiResponse<List<string>>> GetItemRarities()
    {
        try
        {
            var rarities = new List<string>
            {
                "common",
                "uncommon",
                "rare",
                "epic",
                "legendary"
            };

            return Ok(new ApiResponse<List<string>> { Data = rarities });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching item rarities");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching item rarities"
            });
        }
    }

    /// <summary>
    /// Get all NPC roles.
    /// </summary>
    [HttpGet("npc-roles")]
    public ActionResult<ApiResponse<List<string>>> GetNpcRoles()
    {
        try
        {
            var roles = new List<string>
            {
                "merchant",
                "quest-giver",
                "trainer",
                "vendor",
                "story",
                "guide"
            };

            return Ok(new ApiResponse<List<string>> { Data = roles });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching NPC roles");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching NPC roles"
            });
        }
    }

    /// <summary>
    /// Get all obstacle types.
    /// </summary>
    [HttpGet("obstacle-types")]
    public ActionResult<ApiResponse<List<string>>> GetObstacleTypes()
    {
        try
        {
            var types = new List<string>
            {
                "trap",
                "wall",
                "platform",
                "puzzle",
                "container",
                "node",
                "vegetation",
                "barrier"
            };

            return Ok(new ApiResponse<List<string>> { Data = types });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching obstacle types");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching obstacle types"
            });
        }
    }

    /// <summary>
    /// Get all obstacle difficulties.
    /// </summary>
    [HttpGet("obstacle-difficulties")]
    public ActionResult<ApiResponse<List<string>>> GetObstacleDifficulties()
    {
        try
        {
            var difficulties = new List<string>
            {
                "easy",
                "medium",
                "hard"
            };

            return Ok(new ApiResponse<List<string>> { Data = difficulties });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching obstacle difficulties");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching obstacle difficulties"
            });
        }
    }
}
