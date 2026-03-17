using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// Manages all obstacle-related API endpoints for the wiki.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ObstaclesController : ControllerBase
{
    private readonly IMongoDbService _mongoDbService;
    private readonly IObstacleService _obstacleService;
    private readonly ILogger<ObstaclesController> _logger;

    public ObstaclesController(IMongoDbService mongoDbService, IObstacleService obstacleService, ILogger<ObstaclesController> logger)
    {
        _mongoDbService = mongoDbService;
        _obstacleService = obstacleService;
        _logger = logger;
    }

    /// <summary>
    /// Get all obstacles with pagination, filtering, and sorting.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<List<Obstacle>>>>> GetAllObstacles(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = "name",
        [FromQuery] string? sortOrder = "asc",
        [FromQuery] string? type = null,
        [FromQuery] int? difficulty = null,
        [FromQuery] string? location = null,
        [FromQuery] string? search = null)
    {
        try
        {
            if (pageSize > 100) pageSize = 100;
            if (page < 1) page = 1;

            var collection = _mongoDbService.GetObstaclesCollection();
            var filter = Builders<Obstacle>.Filter.Empty;

            // Apply filters
            if (!string.IsNullOrEmpty(type))
                filter &= Builders<Obstacle>.Filter.Eq(o => o.Type, type);

            if (difficulty.HasValue)
                filter &= Builders<Obstacle>.Filter.Eq(o => o.Difficulty, difficulty.Value);

            if (!string.IsNullOrEmpty(location))
                filter &= Builders<Obstacle>.Filter.Eq(o => o.Location, location);

            if (!string.IsNullOrEmpty(search))
                filter &= Builders<Obstacle>.Filter.Or(
                    Builders<Obstacle>.Filter.Regex(o => o.Name, $"/{search}/i"),
                    Builders<Obstacle>.Filter.Regex(o => o.Description, $"/{search}/i")
                );

            // Apply sorting
            var sortDef = sortOrder?.ToLower() == "desc"
                ? Builders<Obstacle>.Sort.Descending(sort ?? "name")
                : Builders<Obstacle>.Sort.Ascending(sort ?? "name");

            var obstacles = await collection
                .Find(filter)
                .Sort(sortDef)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalCount = await collection.CountDocumentsAsync(filter);

            return Ok(new ApiResponse<PaginatedResponse<List<Obstacle>>>
            {
                Data = new PaginatedResponse<List<Obstacle>>
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = obstacles
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching obstacles");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching obstacles"
            });
        }
    }

    /// <summary>
    /// Get a single obstacle by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Obstacle>>> GetObstacleById(string id)
    {
        try
        {
            var obstacle = await _obstacleService.GetObstacleAsync(id);

            if (obstacle == null)
                return NotFound(new ApiResponse<object> { Error = "Obstacle not found" });

            return Ok(new ApiResponse<Obstacle> { Data = obstacle });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching obstacle with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching the obstacle"
            });
        }
    }

    /// <summary>
    /// Get challenge details for an obstacle (question, difficulty, etc.).
    /// </summary>
    [HttpGet("{id}/challenge")]
    public async Task<ActionResult<ApiResponse<object>>> GetObstacleChallenge(string id)
    {
        try
        {
            var obstacle = await _obstacleService.GetObstacleAsync(id);

            if (obstacle == null)
                return NotFound(new ApiResponse<object> { Error = "Obstacle not found" });

            var challengeData = new
            {
                obstacle.Type,
                obstacle.Name,
                obstacle.Description,
                obstacle.Difficulty,
                challenge = obstacle.Type.ToLower() switch
                {
                    "riddle" => new
                    {
                        question = (obstacle.ChallengeData as RiddleChallenge)?.Question,
                        hintsAvailable = ((obstacle.ChallengeData as RiddleChallenge)?.Hints?.Count) ?? 0
                    },
                    "locked_chest" => new
                    {
                        lockDifficulty = (obstacle.ChallengeData as ChestChallenge)?.LockDifficulty,
                        requiresKey = !string.IsNullOrEmpty((obstacle.ChallengeData as ChestChallenge)?.RequiredKeyId)
                    },
                    "trapped_door" => new
                    {
                        triggerType = (obstacle.ChallengeData as TrapChallenge)?.TriggerType,
                        detectionDifficulty = (obstacle.ChallengeData as TrapChallenge)?.DetectionDifficulty,
                        disarmDifficulty = (obstacle.ChallengeData as TrapChallenge)?.DisarmDifficulty
                    },
                    "puzzle" => new
                    {
                        stepCount = ((obstacle.ChallengeData as PuzzleChallenge)?.Steps?.Count) ?? 0,
                        requiredItems = (obstacle.ChallengeData as PuzzleChallenge)?.RequiredItems
                    },
                    _ => null
                }
            };

            return Ok(new ApiResponse<object> { Data = challengeData });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching challenge for obstacle {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching the challenge"
            });
        }
    }

    /// <summary>
    /// Get hints for a riddle obstacle.
    /// </summary>
    [HttpGet("{id}/hints")]
    public async Task<ActionResult<ApiResponse<List<string>>>> GetObstacleHints(string id, [FromQuery] int hintIndex = 0)
    {
        try
        {
            var obstacle = await _obstacleService.GetObstacleAsync(id);

            if (obstacle == null)
                return NotFound(new ApiResponse<object> { Error = "Obstacle not found" });

            if (obstacle.Type.ToLower() != "riddle")
                return BadRequest(new ApiResponse<object> { Error = "This obstacle does not support hints" });

            var riddle = obstacle.ChallengeData as RiddleChallenge;
            if (riddle?.Hints == null || riddle.Hints.Count == 0)
                return Ok(new ApiResponse<List<string>> { Data = new List<string>() });

            var hints = riddle.Hints
                .Take(Math.Max(1, hintIndex + 1))
                .ToList();

            return Ok(new ApiResponse<List<string>> { Data = hints });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching hints for obstacle {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching hints"
            });
        }
    }

    /// <summary>
    /// Submit an attempt/solution for an obstacle.
    /// </summary>
    [HttpPost("{id}/attempt")]
    public async Task<ActionResult<ApiResponse<ObstacleResult>>> AttemptObstacle(string id, [FromBody] AttemptObstacleRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.PlayerId))
                return BadRequest(new ApiResponse<object> { Error = "PlayerId is required" });

            if (string.IsNullOrWhiteSpace(request.Solution))
                return BadRequest(new ApiResponse<object> { Error = "Solution is required" });

            var result = await _obstacleService.AttemptObstacleAsync(request.PlayerId, id, request.Solution);

            return Ok(new ApiResponse<ObstacleResult> { Data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error attempting obstacle {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while processing the attempt"
            });
        }
    }

    /// <summary>
    /// Get a player's attempt history for an obstacle.
    /// </summary>
    [HttpGet("{id}/attempts/{playerId}")]
    public async Task<ActionResult<ApiResponse<List<ObstacleAttempt>>>> GetPlayerAttempts(string id, string playerId)
    {
        try
        {
            var attempts = await _obstacleService.GetPlayerAttemptsAsync(playerId, id);

            return Ok(new ApiResponse<List<ObstacleAttempt>> { Data = attempts });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching attempts for player {PlayerId} on obstacle {Id}", playerId, id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching attempts"
            });
        }
    }

    /// <summary>
    /// Get all player attempts across all obstacles.
    /// </summary>
    [HttpGet("player/{playerId}/history")]
    public async Task<ActionResult<ApiResponse<List<ObstacleAttempt>>>> GetPlayerAttemptHistory(string playerId)
    {
        try
        {
            var attempts = await _obstacleService.GetPlayerAllAttemptsAsync(playerId);

            return Ok(new ApiResponse<List<ObstacleAttempt>> { Data = attempts });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching attempt history for player {PlayerId}", playerId);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching attempt history"
            });
        }
    }

    /// <summary>
    /// Create a new obstacle.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Obstacle>>> CreateObstacle([FromBody] CreateObstacleRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    Error = "Invalid request data"
                });

            var obstacle = new Obstacle
            {
                Name = request.Name,
                Type = request.Type,
                Difficulty = request.Difficulty,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Location = request.Location
            };

            var collection = _mongoDbService.GetObstaclesCollection();
            await collection.InsertOneAsync(obstacle);

            return CreatedAtAction(nameof(GetObstacleById), new { id = obstacle.Id }, new ApiResponse<Obstacle> { Data = obstacle });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating obstacle");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while creating the obstacle"
            });
        }
    }

    /// <summary>
    /// Update an existing obstacle.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Obstacle>>> UpdateObstacle(string id, [FromBody] UpdateObstacleRequest request)
    {
        try
        {
            var collection = _mongoDbService.GetObstaclesCollection();
            var obstacle = await collection.Find(o => o.Id == id).FirstOrDefaultAsync();

            if (obstacle == null)
                return NotFound(new ApiResponse<object> { Error = "Obstacle not found" });

            var updateDefinition = Builders<Obstacle>.Update
                .Set(o => o.UpdatedAt, DateTime.UtcNow);

            if (!string.IsNullOrEmpty(request.Name))
                updateDefinition = updateDefinition.Set(o => o.Name, request.Name);

            if (!string.IsNullOrEmpty(request.Type))
                updateDefinition = updateDefinition.Set(o => o.Type, request.Type);

            if (request.Difficulty.HasValue)
                updateDefinition = updateDefinition.Set(o => o.Difficulty, request.Difficulty.Value);

            if (!string.IsNullOrEmpty(request.Description))
                updateDefinition = updateDefinition.Set(o => o.Description, request.Description);

            if (!string.IsNullOrEmpty(request.ImageUrl))
                updateDefinition = updateDefinition.Set(o => o.ImageUrl, request.ImageUrl);

            if (!string.IsNullOrEmpty(request.Location))
                updateDefinition = updateDefinition.Set(o => o.Location, request.Location);

            var updatedObstacle = await collection.FindOneAndUpdateAsync(
                o => o.Id == id,
                updateDefinition,
                new FindOneAndUpdateOptions<Obstacle> { ReturnDocument = ReturnDocument.After }
            );

            return Ok(new ApiResponse<Obstacle> { Data = updatedObstacle });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating obstacle with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while updating the obstacle"
            });
        }
    }

    /// <summary>
    /// Delete an obstacle.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteObstacle(string id)
    {
        try
        {
            var collection = _mongoDbService.GetObstaclesCollection();
            var result = await collection.DeleteOneAsync(o => o.Id == id);

            if (result.DeletedCount == 0)
                return NotFound(new ApiResponse<object> { Error = "Obstacle not found" });

            return Ok(new ApiResponse<object> { Data = new { message = "Obstacle deleted successfully" } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting obstacle with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while deleting the obstacle"
            });
        }
    }
}

/// <summary>
/// Request to attempt an obstacle.
/// </summary>
public class AttemptObstacleRequest
{
    public string PlayerId { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
}
