using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnemiesController : ControllerBase
{
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<EnemiesController> _logger;

    public EnemiesController(IMongoDbService mongoDbService, ILogger<EnemiesController> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<Enemy>>>> GetAllEnemies()
    {
        try
        {
            var collection = _mongoDbService.GetEnemiesCollection();
            var enemies = await collection.Find(_ => true).ToListAsync();
            return Ok(new ApiResponse<List<Enemy>> { Data = enemies });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching enemies");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching enemies"
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Enemy>>> GetEnemyById(string id)
    {
        try
        {
            var collection = _mongoDbService.GetEnemiesCollection();
            var enemy = await collection.Find(e => e.Id == id).FirstOrDefaultAsync();

            if (enemy == null)
                return NotFound(new ApiResponse<object> { Error = "Enemy not found" });

            return Ok(new ApiResponse<Enemy> { Data = enemy });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching enemy with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while fetching the enemy"
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Enemy>>> CreateEnemy([FromBody] CreateEnemyRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    Error = "Invalid request data"
                });

            var enemy = new Enemy
            {
                Name = request.Name,
                Description = request.Description,
                Health = request.Health,
                Attack = request.Attack,
                Defense = request.Defense,
                Experience = request.Experience,
                Drops = request.Drops ?? new()
            };

            var collection = _mongoDbService.GetEnemiesCollection();
            await collection.InsertOneAsync(enemy);

            return CreatedAtAction(nameof(GetEnemyById), new { id = enemy.Id }, new ApiResponse<Enemy> { Data = enemy });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating enemy");
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while creating the enemy"
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Enemy>>> UpdateEnemy(string id, [FromBody] UpdateEnemyRequest request)
    {
        try
        {
            var collection = _mongoDbService.GetEnemiesCollection();
            var enemy = await collection.Find(e => e.Id == id).FirstOrDefaultAsync();

            if (enemy == null)
                return NotFound(new ApiResponse<object> { Error = "Enemy not found" });

            var updateDefinition = Builders<Enemy>.Update
                .Set(e => e.Name, request.Name ?? enemy.Name)
                .Set(e => e.Description, request.Description ?? enemy.Description)
                .Set(e => e.Health, request.Health ?? enemy.Health)
                .Set(e => e.Attack, request.Attack ?? enemy.Attack)
                .Set(e => e.Defense, request.Defense ?? enemy.Defense)
                .Set(e => e.Experience, request.Experience ?? enemy.Experience)
                .Set(e => e.UpdatedAt, DateTime.UtcNow);

            if (request.Drops != null)
                updateDefinition = updateDefinition.Set(e => e.Drops, request.Drops);

            var updatedEnemy = await collection.FindOneAndUpdateAsync(
                e => e.Id == id,
                updateDefinition,
                new FindOneAndUpdateOptions<Enemy> { ReturnDocument = ReturnDocument.After }
            );

            return Ok(new ApiResponse<Enemy> { Data = updatedEnemy });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating enemy with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while updating the enemy"
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteEnemy(string id)
    {
        try
        {
            var collection = _mongoDbService.GetEnemiesCollection();
            var result = await collection.DeleteOneAsync(e => e.Id == id);

            if (result.DeletedCount == 0)
                return NotFound(new ApiResponse<object> { Error = "Enemy not found" });

            return Ok(new ApiResponse<object> { Data = new { message = "Enemy deleted successfully" } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting enemy with id: {Id}", id);
            return StatusCode(500, new ApiResponse<object>
            {
                Error = "An error occurred while deleting the enemy"
            });
        }
    }
}

public class CreateEnemyRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Experience { get; set; }
    public List<LootDrop>? Drops { get; set; }
}

public class UpdateEnemyRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Health { get; set; }
    public int? Attack { get; set; }
    public int? Defense { get; set; }
    public int? Experience { get; set; }
    public List<LootDrop>? Drops { get; set; }
}
