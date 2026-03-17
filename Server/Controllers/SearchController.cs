using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// Manages global search functionality across all wiki entities.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<SearchController> _logger;

    public SearchController(IMongoDbService mongoDbService, ILogger<SearchController> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    /// <summary>
    /// Global search across all entities.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<SearchResults>>> GlobalSearch(
        [FromQuery] string? q = null,
        [FromQuery] string? entityTypes = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new ApiResponse<object>
                {
                    Error = "Search query (q) is required"
                });

            if (pageSize > 100) pageSize = 100;
            if (page < 1) page = 1;

            var results = new SearchResults();
            var searchFilter = Builders<Creature>.Filter.Text(q);
            var types = string.IsNullOrEmpty(entityTypes)
                ? new[] { "creatures", "items", "npcs", "obstacles" }
                : entityTypes.Split(',').Select(t => t.Trim().ToLower()).ToArray();

            var skip = (page - 1) * pageSize;
            var limit = pageSize;

            // Search creatures
            if (types.Contains("creatures"))
            {
                try
                {
                    var creaturesCollection = _mongoDbService.GetCreaturesCollection();
                    var creatures = await creaturesCollection
                        .Find(searchFilter)
                        .Skip(skip)
                        .Limit(limit)
                        .ToListAsync();

                    var totalCount = await creaturesCollection
                        .CountDocumentsAsync(searchFilter);

                    results.Creatures = creatures;
                    results.CreaturesTotalCount = totalCount;
                }
                catch
                {
                    results.Creatures = new();
                    results.CreaturesTotalCount = 0;
                }
            }

            // Search items
            if (types.Contains("items"))
            {
                try
                {
                    var itemsSearchFilter = Builders<Item>.Filter.Text(q);
                    var itemsCollection = _mongoDbService.GetItemsCollection();
                    var items = await itemsCollection
                        .Find(itemsSearchFilter)
                        .Skip(skip)
                        .Limit(limit)
                        .ToListAsync();

                    var totalCount = await itemsCollection
                        .CountDocumentsAsync(itemsSearchFilter);

                    results.Items = items;
                    results.ItemsTotalCount = totalCount;
                }
                catch
                {
                    results.Items = new();
                    results.ItemsTotalCount = 0;
                }
            }

            // Search NPCs
            if (types.Contains("npcs"))
            {
                try
                {
                    var npcsSearchFilter = Builders<NPC>.Filter.Text(q);
                    var npcsCollection = _mongoDbService.GetNpcsCollection();
                    var npcs = await npcsCollection
                        .Find(npcsSearchFilter)
                        .Skip(skip)
                        .Limit(limit)
                        .ToListAsync();

                    var totalCount = await npcsCollection
                        .CountDocumentsAsync(npcsSearchFilter);

                    results.NPCs = npcs;
                    results.NPCsTotalCount = totalCount;
                }
                catch
                {
                    results.NPCs = new();
                    results.NPCsTotalCount = 0;
                }
            }

            // Search obstacles
            if (types.Contains("obstacles"))
            {
                try
                {
                    var obstaclesSearchFilter = Builders<Obstacle>.Filter.Text(q);
                    var obstaclesCollection = _mongoDbService.GetObstaclesCollection();
                    var obstacles = await obstaclesCollection
                        .Find(obstaclesSearchFilter)
                        .Skip(skip)
                        .Limit(limit)
                        .ToListAsync();

                    var totalCount = await obstaclesCollection
                        .CountDocumentsAsync(obstaclesSearchFilter);

                    results.Obstacles = obstacles;
                    results.ObstaclesTotalCount = totalCount;
                }
                catch
                {
                    results.Obstacles = new();
                    results.ObstaclesTotalCount = 0;
                }
            }

            results.PageNumber = page;
            results.PageSize = pageSize;

            return Ok(new ApiResponse<SearchResults> { Data = results });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing global search");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while performing search"
            });
        }
    }

    /// <summary>
    /// Type-ahead autocomplete suggestions.
    /// </summary>
    [HttpGet("autocomplete")]
    public async Task<ActionResult<ApiResponse<List<AutocompleteResult>>>> GetAutocomplete(
        [FromQuery] string? q = null,
        [FromQuery] int limit = 10)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new ApiResponse<object>
                {
                    Error = "Search query (q) is required"
                });

            if (limit > 50) limit = 50;

            var results = new List<AutocompleteResult>();

            // Get creatures
            try
            {
                var creaturesCollection = _mongoDbService.GetCreaturesCollection();
                var creaturesFilter = Builders<Creature>.Filter.Text(q);
                var creatures = await creaturesCollection
                    .Find(creaturesFilter)
                    .Limit(limit)
                    .ToListAsync();

                foreach (var creature in creatures)
                {
                    results.Add(new AutocompleteResult
                    {
                        Id = creature.Id,
                        Text = creature.Name,
                        Type = "creature",
                        ImageUrl = creature.ImageUrl
                    });
                }
            }
            catch { }

            // Get items
            try
            {
                var itemsCollection = _mongoDbService.GetItemsCollection();
                var itemsFilter = Builders<Item>.Filter.Text(q);
                var items = await itemsCollection
                    .Find(itemsFilter)
                    .Limit(limit)
                    .ToListAsync();

                foreach (var item in items)
                {
                    results.Add(new AutocompleteResult
                    {
                        Id = item.Id,
                        Text = item.Name,
                        Type = "item",
                        ImageUrl = item.ImageUrl
                    });
                }
            }
            catch { }

            // Get NPCs
            try
            {
                var npcsCollection = _mongoDbService.GetNpcsCollection();
                var npcsFilter = Builders<NPC>.Filter.Text(q);
                var npcs = await npcsCollection
                    .Find(npcsFilter)
                    .Limit(limit)
                    .ToListAsync();

                foreach (var npc in npcs)
                {
                    results.Add(new AutocompleteResult
                    {
                        Id = npc.Id,
                        Text = npc.Name,
                        Type = "npc",
                        ImageUrl = npc.ImageUrl
                    });
                }
            }
            catch { }

            // Sort by relevance (exact match first)
            var sortedResults = results
                .Where(r => r.Text?.Equals(q, StringComparison.OrdinalIgnoreCase) ?? false)
                .Concat(results.Where(r => r.Text?.StartsWith(q, StringComparison.OrdinalIgnoreCase) ?? false))
                .Concat(results)
                .Take(limit)
                .ToList();

            return Ok(new ApiResponse<List<AutocompleteResult>> { Data = sortedResults });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching autocomplete suggestions");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching suggestions"
            });
        }
    }
}

public class SearchResults
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public List<Creature> Creatures { get; set; } = new();
    public long CreaturesTotalCount { get; set; }
    public List<Item> Items { get; set; } = new();
    public long ItemsTotalCount { get; set; }
    public List<NPC> NPCs { get; set; } = new();
    public long NPCsTotalCount { get; set; }
    public List<Obstacle> Obstacles { get; set; } = new();
    public long ObstaclesTotalCount { get; set; }
}

public class AutocompleteResult
{
    public string? Id { get; set; }
    public string? Text { get; set; }
    public string? Type { get; set; } // creature, item, npc, obstacle
    public string? ImageUrl { get; set; }
}
