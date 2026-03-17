using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// Manages all creature-related API endpoints for the wiki.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CreaturesController : ControllerBase
{
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<CreaturesController> _logger;

    public CreaturesController(IMongoDbService mongoDbService, ILogger<CreaturesController> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    /// <summary>
    /// Get all creatures with pagination, filtering, and sorting.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<List<Creature>>>>> GetAllCreatures(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = "name",
        [FromQuery] string? sortOrder = "asc",
        [FromQuery] string? type = null,
        [FromQuery] string? location = null,
        [FromQuery] int? minHealth = null,
        [FromQuery] int? maxHealth = null,
        [FromQuery] string? search = null)
    {
        try
        {
            if (pageSize > 100) pageSize = 100;
            if (page < 1) page = 1;

            var collection = _mongoDbService.GetCreaturesCollection();
            var filter = Builders<Creature>.Filter.Empty;

            // Apply filters
            if (!string.IsNullOrEmpty(type))
                filter &= Builders<Creature>.Filter.Eq(c => c.Type, type);

            if (!string.IsNullOrEmpty(location))
                filter &= Builders<Creature>.Filter.Eq(c => c.Location, location);

            if (minHealth.HasValue)
                filter &= Builders<Creature>.Filter.Gte(c => c.Health, minHealth.Value);

            if (maxHealth.HasValue)
                filter &= Builders<Creature>.Filter.Lte(c => c.Health, maxHealth.Value);

            if (!string.IsNullOrEmpty(search))
                filter &= Builders<Creature>.Filter.Text(search);

            // Apply sorting
            var sortDef = sortOrder?.ToLower() == "desc"
                ? Builders<Creature>.Sort.Descending(sort ?? "name")
                : Builders<Creature>.Sort.Ascending(sort ?? "name");

            var creatures = await collection
                .Find(filter)
                .Sort(sortDef)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalCount = await collection.CountDocumentsAsync(filter);

            return Ok(new ApiResponse<PaginatedResponse<List<Creature>>>
            {
                Data = new PaginatedResponse<List<Creature>>
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = creatures
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching creatures");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching creatures"
            });
        }
    }

    /// <summary>
    /// Get a single creature by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Creature>>> GetCreatureById(string id)
    {
        try
        {
            var collection = _mongoDbService.GetCreaturesCollection();
            var creature = await collection.Find(c => c.Id == id).FirstOrDefaultAsync();

            if (creature == null)
                return NotFound(new ApiResponse<object> { Error = "Creature not found" });

            return Ok(new ApiResponse<Creature> { Data = creature });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching creature with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching the creature"
            });
        }
    }

    /// <summary>
    /// Get creatures by location.
    /// </summary>
    [HttpGet("by-location/{location}")]
    public async Task<ActionResult<ApiResponse<List<Creature>>>> GetCreaturesByLocation(string location)
    {
        try
        {
            var collection = _mongoDbService.GetCreaturesCollection();
            var creatures = await collection
                .Find(c => c.Location == location)
                .ToListAsync();

            return Ok(new ApiResponse<List<Creature>> { Data = creatures });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching creatures by location: {Location}", location);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching creatures"
            });
        }
    }

    /// <summary>
    /// Get loot drops from a creature.
    /// </summary>
    [HttpGet("{id}/loot")]
    public async Task<ActionResult<ApiResponse<List<LootDrop>>>> GetCreatureLoot(string id)
    {
        try
        {
            var collection = _mongoDbService.GetCreaturesCollection();
            var creature = await collection.Find(c => c.Id == id).FirstOrDefaultAsync();

            if (creature == null)
                return NotFound(new ApiResponse<object> { Error = "Creature not found" });

            return Ok(new ApiResponse<List<LootDrop>> { Data = creature.Drops });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching loot for creature: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching creature loot"
            });
        }
    }

    /// <summary>
    /// Create a new creature.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Creature>>> CreateCreature([FromBody] CreateCreatureRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    Error = "Invalid request data"
                });

            var creature = new Creature
            {
                Name = request.Name,
                Type = request.Type,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Health = request.Health,
                Attack = request.Attack,
                Defense = request.Defense,
                Experience = request.Experience,
                Location = request.Location,
                Abilities = request.Abilities ?? new(),
                Weaknesses = request.Weaknesses ?? new(),
                Drops = request.Drops ?? new()
            };

            var collection = _mongoDbService.GetCreaturesCollection();
            await collection.InsertOneAsync(creature);

            return CreatedAtAction(nameof(GetCreatureById), new { id = creature.Id }, new ApiResponse<Creature> { Data = creature });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating creature");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while creating the creature"
            });
        }
    }

    /// <summary>
    /// Update an existing creature.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Creature>>> UpdateCreature(string id, [FromBody] UpdateCreatureRequest request)
    {
        try
        {
            var collection = _mongoDbService.GetCreaturesCollection();
            var creature = await collection.Find(c => c.Id == id).FirstOrDefaultAsync();

            if (creature == null)
                return NotFound(new ApiResponse<object> { Error = "Creature not found" });

            var updateDefinition = Builders<Creature>.Update
                .Set(c => c.UpdatedAt, DateTime.UtcNow);

            if (!string.IsNullOrEmpty(request.Name))
                updateDefinition = updateDefinition.Set(c => c.Name, request.Name);

            if (!string.IsNullOrEmpty(request.Type))
                updateDefinition = updateDefinition.Set(c => c.Type, request.Type);

            if (!string.IsNullOrEmpty(request.Description))
                updateDefinition = updateDefinition.Set(c => c.Description, request.Description);

            if (!string.IsNullOrEmpty(request.ImageUrl))
                updateDefinition = updateDefinition.Set(c => c.ImageUrl, request.ImageUrl);

            if (request.Health.HasValue)
                updateDefinition = updateDefinition.Set(c => c.Health, request.Health.Value);

            if (request.Attack.HasValue)
                updateDefinition = updateDefinition.Set(c => c.Attack, request.Attack.Value);

            if (request.Defense.HasValue)
                updateDefinition = updateDefinition.Set(c => c.Defense, request.Defense.Value);

            if (request.Experience.HasValue)
                updateDefinition = updateDefinition.Set(c => c.Experience, request.Experience.Value);

            if (!string.IsNullOrEmpty(request.Location))
                updateDefinition = updateDefinition.Set(c => c.Location, request.Location);

            if (request.Abilities != null)
                updateDefinition = updateDefinition.Set(c => c.Abilities, request.Abilities);

            if (request.Weaknesses != null)
                updateDefinition = updateDefinition.Set(c => c.Weaknesses, request.Weaknesses);

            if (request.Drops != null)
                updateDefinition = updateDefinition.Set(c => c.Drops, request.Drops);

            var updatedCreature = await collection.FindOneAndUpdateAsync(
                c => c.Id == id,
                updateDefinition,
                new FindOneAndUpdateOptions<Creature> { ReturnDocument = ReturnDocument.After }
            );

            return Ok(new ApiResponse<Creature> { Data = updatedCreature });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating creature with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while updating the creature"
            });
        }
    }

    /// <summary>
    /// Delete a creature.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteCreature(string id)
    {
        try
        {
            var collection = _mongoDbService.GetCreaturesCollection();
            var result = await collection.DeleteOneAsync(c => c.Id == id);

            if (result.DeletedCount == 0)
                return NotFound(new ApiResponse<object> { Error = "Creature not found" });

            return Ok(new ApiResponse<object> { Data = new { message = "Creature deleted successfully" } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting creature with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while deleting the creature"
            });
        }
    }
}
