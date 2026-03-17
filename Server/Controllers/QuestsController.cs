using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// Manages all quest-related API endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class QuestsController : ControllerBase
{
    private readonly IQuestService _questService;
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<QuestsController> _logger;
    private const string HARDCODED_PLAYER_ID = "player_1"; // TODO: Replace with auth system
    private const int HARDCODED_PLAYER_LEVEL = 20; // TODO: Replace with actual player data

    public QuestsController(IQuestService questService, IMongoDbService mongoDbService, ILogger<QuestsController> logger)
    {
        _questService = questService;
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    /// <summary>
    /// Get all available quests for the player's level.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<QuestDetailsDto>>>> GetAvailableQuests()
    {
        try
        {
            var quests = await _questService.GetAvailableQuestsAsync(HARDCODED_PLAYER_LEVEL);
            
            var questDetails = new List<QuestDetailsDto>();
            foreach (var quest in quests)
            {
                var details = await _questService.GetQuestDetailsAsync(quest.Id);
                if (details != null)
                    questDetails.Add(details);
            }

            return Ok(new ApiResponse<List<QuestDetailsDto>>
            {
                Success = true,
                Data = questDetails,
                Message = $"Retrieved {questDetails.Count} available quests"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available quests");
            return StatusCode(500, new ApiResponse<List<QuestDetailsDto>>
            {
                Success = false,
                Message = "Error retrieving quests"
            });
        }
    }

    /// <summary>
    /// Get details for a specific quest.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<QuestDetailsDto>>> GetQuestById(string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var questId))
            {
                return BadRequest(new ApiResponse<QuestDetailsDto>
                {
                    Success = false,
                    Message = "Invalid quest ID format"
                });
            }

            var questDetails = await _questService.GetQuestDetailsAsync(questId);
            if (questDetails == null)
            {
                return NotFound(new ApiResponse<QuestDetailsDto>
                {
                    Success = false,
                    Message = "Quest not found"
                });
            }

            return Ok(new ApiResponse<QuestDetailsDto>
            {
                Success = true,
                Data = questDetails
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quest details");
            return StatusCode(500, new ApiResponse<QuestDetailsDto>
            {
                Success = false,
                Message = "Error retrieving quest"
            });
        }
    }

    /// <summary>
    /// Accept a quest for the player.
    /// </summary>
    [HttpPost("{id}/accept")]
    public async Task<ActionResult<ApiResponse<QuestProgressDto>>> AcceptQuest(string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var questId))
            {
                return BadRequest(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Invalid quest ID format"
                });
            }

            // Validate player can accept this quest
            var canAccess = await _questService.ValidateQuestAccessAsync(HARDCODED_PLAYER_ID, questId);
            if (!canAccess)
            {
                return BadRequest(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Quest cannot be accepted. Check level requirements and prerequisites."
                });
            }

            var questProgress = await _questService.AcceptQuestAsync(HARDCODED_PLAYER_ID, questId);
            if (questProgress == null)
            {
                return NotFound(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Quest not found"
                });
            }

            var progressDto = MapToQuestProgressDto(questProgress);
            return Created($"/api/quests/{id}/progress", new ApiResponse<QuestProgressDto>
            {
                Success = true,
                Data = progressDto,
                Message = "Quest accepted successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error accepting quest");
            return StatusCode(500, new ApiResponse<QuestProgressDto>
            {
                Success = false,
                Message = "Error accepting quest"
            });
        }
    }

    /// <summary>
    /// Update progress on a quest objective.
    /// </summary>
    [HttpPost("{questId}/progress")]
    public async Task<ActionResult<ApiResponse<QuestProgressDto>>> UpdateQuestProgress(
        string questId,
        [FromBody] UpdateQuestProgressRequest request)
    {
        try
        {
            if (!ObjectId.TryParse(questId, out var questObjectId))
            {
                return BadRequest(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Invalid quest ID format"
                });
            }

            if (request.ProgressAmount <= 0)
            {
                return BadRequest(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Progress amount must be greater than 0"
                });
            }

            await _questService.UpdateObjectiveProgressAsync(
                HARDCODED_PLAYER_ID,
                questObjectId,
                request.ObjectiveId,
                request.ProgressAmount);

            var progressCollection = _mongoDbService.GetQuestProgressCollection();
            var questProgress = await progressCollection
                .Find(qp => qp.PlayerId == HARDCODED_PLAYER_ID && qp.QuestId == questObjectId)
                .FirstOrDefaultAsync();

            if (questProgress == null)
            {
                return NotFound(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Quest progress not found"
                });
            }

            var progressDto = MapToQuestProgressDto(questProgress);
            return Ok(new ApiResponse<QuestProgressDto>
            {
                Success = true,
                Data = progressDto,
                Message = "Quest progress updated"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating quest progress");
            return StatusCode(500, new ApiResponse<QuestProgressDto>
            {
                Success = false,
                Message = "Error updating quest progress"
            });
        }
    }

    /// <summary>
    /// Complete a quest if all objectives are finished.
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<ActionResult<ApiResponse<QuestProgressDto>>> CompleteQuest(string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var questId))
            {
                return BadRequest(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Invalid quest ID format"
                });
            }

            var success = await _questService.CompleteQuestAsync(HARDCODED_PLAYER_ID, questId);
            if (!success)
            {
                return BadRequest(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Cannot complete quest. Not all objectives are finished."
                });
            }

            var progressCollection = _mongoDbService.GetQuestProgressCollection();
            var questProgress = await progressCollection
                .Find(qp => qp.PlayerId == HARDCODED_PLAYER_ID && qp.QuestId == questId)
                .FirstOrDefaultAsync();

            var progressDto = MapToQuestProgressDto(questProgress);
            return Ok(new ApiResponse<QuestProgressDto>
            {
                Success = true,
                Data = progressDto,
                Message = "Quest completed and rewards dispensed!"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing quest");
            return StatusCode(500, new ApiResponse<QuestProgressDto>
            {
                Success = false,
                Message = "Error completing quest"
            });
        }
    }

    /// <summary>
    /// Get player's quest log (all active and completed quests).
    /// </summary>
    [HttpGet("player/{playerId}/log")]
    public async Task<ActionResult<ApiResponse<List<QuestProgressDto>>>> GetPlayerQuestLog(string playerId)
    {
        try
        {
            // For now, ignore playerId parameter and use hardcoded player
            var questProgress = await _questService.GetPlayerQuestProgressAsync(HARDCODED_PLAYER_ID);
            
            var progressDtos = questProgress.Select(MapToQuestProgressDto).ToList();

            return Ok(new ApiResponse<List<QuestProgressDto>>
            {
                Success = true,
                Data = progressDtos,
                Message = $"Retrieved {progressDtos.Count} quests"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving player quest log");
            return StatusCode(500, new ApiResponse<List<QuestProgressDto>>
            {
                Success = false,
                Message = "Error retrieving quest log"
            });
        }
    }

    /// <summary>
    /// Get quest progress for a specific quest.
    /// </summary>
    [HttpGet("{questId}/progress")]
    public async Task<ActionResult<ApiResponse<QuestProgressDto>>> GetQuestProgress(string questId)
    {
        try
        {
            if (!ObjectId.TryParse(questId, out var questObjectId))
            {
                return BadRequest(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Invalid quest ID format"
                });
            }

            var progressCollection = _mongoDbService.GetQuestProgressCollection();
            var questProgress = await progressCollection
                .Find(qp => qp.PlayerId == HARDCODED_PLAYER_ID && qp.QuestId == questObjectId)
                .FirstOrDefaultAsync();

            if (questProgress == null)
            {
                return NotFound(new ApiResponse<QuestProgressDto>
                {
                    Success = false,
                    Message = "Quest progress not found"
                });
            }

            var progressDto = MapToQuestProgressDto(questProgress);
            return Ok(new ApiResponse<QuestProgressDto>
            {
                Success = true,
                Data = progressDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quest progress");
            return StatusCode(500, new ApiResponse<QuestProgressDto>
            {
                Success = false,
                Message = "Error retrieving quest progress"
            });
        }
    }

    private QuestProgressDto MapToQuestProgressDto(QuestProgress questProgress)
    {
        return new QuestProgressDto
        {
            Id = questProgress.Id,
            QuestName = questProgress.QuestName,
            Status = questProgress.Status,
            ObjectivesProgress = questProgress.ObjectivesProgress.Select(op => new ObjectiveProgressDto
            {
                ObjectiveType = op.ObjectiveType,
                Completed = op.Completed,
                Progress = op.Progress,
                Target = op.Target
            }).ToList(),
            StartedAt = questProgress.StartedAt,
            CompletedAt = questProgress.CompletedAt
        };
    }
}
