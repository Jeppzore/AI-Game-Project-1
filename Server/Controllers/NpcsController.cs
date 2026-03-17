using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// Manages all NPC-related API endpoints for the wiki.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NpcsController : ControllerBase
{
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<NpcsController> _logger;

    public NpcsController(IMongoDbService mongoDbService, ILogger<NpcsController> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    /// <summary>
    /// Get all NPCs with pagination, filtering, and sorting.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<List<NPC>>>>> GetAllNpcs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = "name",
        [FromQuery] string? sortOrder = "asc",
        [FromQuery] string? role = null,
        [FromQuery] string? location = null,
        [FromQuery] string? search = null)
    {
        try
        {
            if (pageSize > 100) pageSize = 100;
            if (page < 1) page = 1;

            var collection = _mongoDbService.GetNpcsCollection();
            var filter = Builders<NPC>.Filter.Empty;

            // Apply filters
            if (!string.IsNullOrEmpty(role))
                filter &= Builders<NPC>.Filter.Eq(n => n.Role, role);

            if (!string.IsNullOrEmpty(location))
                filter &= Builders<NPC>.Filter.Eq(n => n.Location.Area, location);

            if (!string.IsNullOrEmpty(search))
                filter &= Builders<NPC>.Filter.Text(search);

            // Apply sorting
            var sortDef = sortOrder?.ToLower() == "desc"
                ? Builders<NPC>.Sort.Descending(sort ?? "name")
                : Builders<NPC>.Sort.Ascending(sort ?? "name");

            var npcs = await collection
                .Find(filter)
                .Sort(sortDef)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalCount = await collection.CountDocumentsAsync(filter);

            return Ok(new ApiResponse<PaginatedResponse<List<NPC>>>
            {
                Data = new PaginatedResponse<List<NPC>>
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = npcs
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching NPCs");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching NPCs"
            });
        }
    }

    /// <summary>
    /// Get a single NPC by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<NPC>>> GetNpcById(string id)
    {
        try
        {
            var collection = _mongoDbService.GetNpcsCollection();
            var npc = await collection.Find(n => n.Id == id).FirstOrDefaultAsync();

            if (npc == null)
                return NotFound(new ApiResponse<object> { Error = "NPC not found" });

            return Ok(new ApiResponse<NPC> { Data = npc });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching NPC with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching the NPC"
            });
        }
    }

    /// <summary>
    /// Get an NPC's buy/sell lists.
    /// </summary>
    [HttpGet("{id}/trades")]
    public async Task<ActionResult<ApiResponse<NPCTrades>>> GetNpcTrades(string id)
    {
        try
        {
            var collection = _mongoDbService.GetNpcsCollection();
            var npc = await collection.Find(n => n.Id == id).FirstOrDefaultAsync();

            if (npc == null)
                return NotFound(new ApiResponse<object> { Error = "NPC not found" });

            return Ok(new ApiResponse<NPCTrades> { Data = npc.Trades });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching trades for NPC: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching NPC trades"
            });
        }
    }

    /// <summary>
    /// Create a new NPC.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<NPC>>> CreateNpc([FromBody] CreateNpcRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    Error = "Invalid request data"
                });

            var npc = new NPC
            {
                Name = request.Name,
                Role = request.Role,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Location = request.Location ?? new(),
                Trades = request.Trades ?? new(),
                Quests = request.Quests ?? new()
            };

            var collection = _mongoDbService.GetNpcsCollection();
            await collection.InsertOneAsync(npc);

            return CreatedAtAction(nameof(GetNpcById), new { id = npc.Id }, new ApiResponse<NPC> { Data = npc });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating NPC");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while creating the NPC"
            });
        }
    }

    /// <summary>
    /// Update an existing NPC.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<NPC>>> UpdateNpc(string id, [FromBody] UpdateNpcRequest request)
    {
        try
        {
            var collection = _mongoDbService.GetNpcsCollection();
            var npc = await collection.Find(n => n.Id == id).FirstOrDefaultAsync();

            if (npc == null)
                return NotFound(new ApiResponse<object> { Error = "NPC not found" });

            var updateDefinition = Builders<NPC>.Update
                .Set(n => n.UpdatedAt, DateTime.UtcNow);

            if (!string.IsNullOrEmpty(request.Name))
                updateDefinition = updateDefinition.Set(n => n.Name, request.Name);

            if (!string.IsNullOrEmpty(request.Role))
                updateDefinition = updateDefinition.Set(n => n.Role, request.Role);

            if (!string.IsNullOrEmpty(request.Description))
                updateDefinition = updateDefinition.Set(n => n.Description, request.Description);

            if (!string.IsNullOrEmpty(request.ImageUrl))
                updateDefinition = updateDefinition.Set(n => n.ImageUrl, request.ImageUrl);

            if (request.Location != null)
                updateDefinition = updateDefinition.Set(n => n.Location, request.Location);

            if (request.Trades != null)
                updateDefinition = updateDefinition.Set(n => n.Trades, request.Trades);

            if (request.Quests != null)
                updateDefinition = updateDefinition.Set(n => n.Quests, request.Quests);

            var updatedNpc = await collection.FindOneAndUpdateAsync(
                n => n.Id == id,
                updateDefinition,
                new FindOneAndUpdateOptions<NPC> { ReturnDocument = ReturnDocument.After }
            );

            return Ok(new ApiResponse<NPC> { Data = updatedNpc });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating NPC with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while updating the NPC"
            });
        }
    }

    /// <summary>
    /// Delete an NPC.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteNpc(string id)
    {
        try
        {
            var collection = _mongoDbService.GetNpcsCollection();
            var result = await collection.DeleteOneAsync(n => n.Id == id);

            if (result.DeletedCount == 0)
                return NotFound(new ApiResponse<object> { Error = "NPC not found" });

            return Ok(new ApiResponse<object> { Data = new { message = "NPC deleted successfully" } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting NPC with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while deleting the NPC"
            });
        }
    }
}
