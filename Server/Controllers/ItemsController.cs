using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// Manages all item-related API endpoints for the wiki.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(IMongoDbService mongoDbService, ILogger<ItemsController> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    /// <summary>
    /// Get all items with pagination, filtering, and sorting.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<List<Item>>>>> GetAllItems(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = "name",
        [FromQuery] string? sortOrder = "asc",
        [FromQuery] string? type = null,
        [FromQuery] string? category = null,
        [FromQuery] string? rarity = null,
        [FromQuery] int? minDamage = null,
        [FromQuery] int? maxDamage = null,
        [FromQuery] string? search = null)
    {
        try
        {
            if (pageSize > 100) pageSize = 100;
            if (page < 1) page = 1;

            var collection = _mongoDbService.GetItemsCollection();
            var filter = Builders<Item>.Filter.Empty;

            // Apply filters
            if (!string.IsNullOrEmpty(type))
                filter &= Builders<Item>.Filter.Eq(i => i.Type, type);

            if (!string.IsNullOrEmpty(category))
                filter &= Builders<Item>.Filter.Eq(i => i.Category, category);

            if (!string.IsNullOrEmpty(rarity))
                filter &= Builders<Item>.Filter.Eq(i => i.Rarity, rarity);

            if (minDamage.HasValue)
                filter &= Builders<Item>.Filter.Gte(i => i.Stats.Damage, minDamage.Value);

            if (maxDamage.HasValue)
                filter &= Builders<Item>.Filter.Lte(i => i.Stats.Damage, maxDamage.Value);

            if (!string.IsNullOrEmpty(search))
                filter &= Builders<Item>.Filter.Text(search);

            // Apply sorting
            var sortDef = sortOrder?.ToLower() == "desc"
                ? Builders<Item>.Sort.Descending(sort ?? "name")
                : Builders<Item>.Sort.Ascending(sort ?? "name");

            var items = await collection
                .Find(filter)
                .Sort(sortDef)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalCount = await collection.CountDocumentsAsync(filter);

            return Ok(new ApiResponse<PaginatedResponse<List<Item>>>
            {
                Data = new PaginatedResponse<List<Item>>
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = items
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching items");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching items"
            });
        }
    }

    /// <summary>
    /// Get a single item by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Item>>> GetItemById(string id)
    {
        try
        {
            var collection = _mongoDbService.GetItemsCollection();
            var item = await collection.Find(i => i.Id == id).FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new ApiResponse<object> { Error = "Item not found" });

            return Ok(new ApiResponse<Item> { Data = item });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching item with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching the item"
            });
        }
    }

    /// <summary>
    /// Get items by type.
    /// </summary>
    [HttpGet("by-type/{type}")]
    public async Task<ActionResult<ApiResponse<List<Item>>>> GetItemsByType(string type)
    {
        try
        {
            var collection = _mongoDbService.GetItemsCollection();
            var items = await collection
                .Find(i => i.Type == type)
                .ToListAsync();

            return Ok(new ApiResponse<List<Item>> { Data = items });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching items by type: {Type}", type);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching items"
            });
        }
    }

    /// <summary>
    /// Get creatures that drop a specific item.
    /// </summary>
    [HttpGet("{id}/dropped-by")]
    public async Task<ActionResult<ApiResponse<List<Creature>>>> GetCreaturesThatDropItem(string id)
    {
        try
        {
            var itemsCollection = _mongoDbService.GetItemsCollection();
            var item = await itemsCollection.Find(i => i.Id == id).FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new ApiResponse<object> { Error = "Item not found" });

            // Find creatures that have this item in their drops
            var creaturesCollection = _mongoDbService.GetCreaturesCollection();
            var creatures = await creaturesCollection
                .Find(c => c.Drops.Any(d => d.ItemName == item.Name))
                .ToListAsync();

            return Ok(new ApiResponse<List<Creature>> { Data = creatures });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching creatures that drop item: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching creatures"
            });
        }
    }

    /// <summary>
    /// Get creatures that use a specific item.
    /// </summary>
    [HttpGet("{id}/used-by")]
    public async Task<ActionResult<ApiResponse<List<Creature>>>> GetCreaturesThatUseItem(string id)
    {
        try
        {
            var itemsCollection = _mongoDbService.GetItemsCollection();
            var item = await itemsCollection.Find(i => i.Id == id).FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new ApiResponse<object> { Error = "Item not found" });

            // Find creatures that are in the UsedBy list
            var creaturesCollection = _mongoDbService.GetCreaturesCollection();
            var creatures = await creaturesCollection
                .Find(c => item.UsedBy.Contains(c.Id ?? string.Empty) || item.UsedBy.Contains(c.Name))
                .ToListAsync();

            return Ok(new ApiResponse<List<Creature>> { Data = creatures });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching creatures that use item: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching creatures"
            });
        }
    }

    /// <summary>
    /// Create a new item.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Item>>> CreateItem([FromBody] CreateItemRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    Error = "Invalid request data"
                });

            var item = new Item
            {
                Name = request.Name,
                Type = request.Type,
                Category = request.Category,
                Rarity = request.Rarity,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Stats = request.Stats ?? new(),
                Requirements = request.Requirements ?? new(),
                Acquisition = request.Acquisition ?? new(),
                UsedBy = request.UsedBy ?? new()
            };

            var collection = _mongoDbService.GetItemsCollection();
            await collection.InsertOneAsync(item);

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, new ApiResponse<Item> { Data = item });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating item");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while creating the item"
            });
        }
    }

    /// <summary>
    /// Update an existing item.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Item>>> UpdateItem(string id, [FromBody] UpdateItemRequest request)
    {
        try
        {
            var collection = _mongoDbService.GetItemsCollection();
            var item = await collection.Find(i => i.Id == id).FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new ApiResponse<object> { Error = "Item not found" });

            var updateDefinition = Builders<Item>.Update
                .Set(i => i.UpdatedAt, DateTime.UtcNow);

            if (!string.IsNullOrEmpty(request.Name))
                updateDefinition = updateDefinition.Set(i => i.Name, request.Name);

            if (!string.IsNullOrEmpty(request.Type))
                updateDefinition = updateDefinition.Set(i => i.Type, request.Type);

            if (!string.IsNullOrEmpty(request.Category))
                updateDefinition = updateDefinition.Set(i => i.Category, request.Category);

            if (!string.IsNullOrEmpty(request.Rarity))
                updateDefinition = updateDefinition.Set(i => i.Rarity, request.Rarity);

            if (!string.IsNullOrEmpty(request.Description))
                updateDefinition = updateDefinition.Set(i => i.Description, request.Description);

            if (!string.IsNullOrEmpty(request.ImageUrl))
                updateDefinition = updateDefinition.Set(i => i.ImageUrl, request.ImageUrl);

            if (request.Stats != null)
                updateDefinition = updateDefinition.Set(i => i.Stats, request.Stats);

            if (request.Requirements != null)
                updateDefinition = updateDefinition.Set(i => i.Requirements, request.Requirements);

            if (request.Acquisition != null)
                updateDefinition = updateDefinition.Set(i => i.Acquisition, request.Acquisition);

            if (request.UsedBy != null)
                updateDefinition = updateDefinition.Set(i => i.UsedBy, request.UsedBy);

            var updatedItem = await collection.FindOneAndUpdateAsync(
                i => i.Id == id,
                updateDefinition,
                new FindOneAndUpdateOptions<Item> { ReturnDocument = ReturnDocument.After }
            );

            return Ok(new ApiResponse<Item> { Data = updatedItem });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating item with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while updating the item"
            });
        }
    }

    /// <summary>
    /// Delete an item.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteItem(string id)
    {
        try
        {
            var collection = _mongoDbService.GetItemsCollection();
            var result = await collection.DeleteOneAsync(i => i.Id == id);

            if (result.DeletedCount == 0)
                return NotFound(new ApiResponse<object> { Error = "Item not found" });

            return Ok(new ApiResponse<object> { Data = new { message = "Item deleted successfully" } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting item with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while deleting the item"
            });
        }
    }
}
