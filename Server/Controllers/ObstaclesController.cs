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
    private readonly ILogger<ObstaclesController> _logger;

    public ObstaclesController(IMongoDbService mongoDbService, ILogger<ObstaclesController> logger)
    {
        _mongoDbService = mongoDbService;
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
        [FromQuery] string? difficulty = null,
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

            if (!string.IsNullOrEmpty(difficulty))
                filter &= Builders<Obstacle>.Filter.Eq(o => o.Difficulty, difficulty);

            if (!string.IsNullOrEmpty(location))
                filter &= Builders<Obstacle>.Filter.Eq(o => o.Location, location);

            if (!string.IsNullOrEmpty(search))
                filter &= Builders<Obstacle>.Filter.Text(search);

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
            var collection = _mongoDbService.GetObstaclesCollection();
            var obstacle = await collection.Find(o => o.Id == id).FirstOrDefaultAsync();

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
                Location = request.Location,
                SolveMethod = request.SolveMethod
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

            if (!string.IsNullOrEmpty(request.Difficulty))
                updateDefinition = updateDefinition.Set(o => o.Difficulty, request.Difficulty);

            if (!string.IsNullOrEmpty(request.Description))
                updateDefinition = updateDefinition.Set(o => o.Description, request.Description);

            if (!string.IsNullOrEmpty(request.ImageUrl))
                updateDefinition = updateDefinition.Set(o => o.ImageUrl, request.ImageUrl);

            if (!string.IsNullOrEmpty(request.Location))
                updateDefinition = updateDefinition.Set(o => o.Location, request.Location);

            if (!string.IsNullOrEmpty(request.SolveMethod))
                updateDefinition = updateDefinition.Set(o => o.SolveMethod, request.SolveMethod);

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
