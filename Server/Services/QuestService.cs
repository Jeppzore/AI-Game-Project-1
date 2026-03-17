using MongoDB.Bson;
using MongoDB.Driver;
using Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services;

/// <summary>
/// Service for managing quests, objectives, and player quest progress.
/// </summary>
public interface IQuestService
{
    Task<List<Quest>> GetAvailableQuestsAsync(int playerLevel);
    Task<QuestDetailsDto> GetQuestDetailsAsync(ObjectId questId);
    Task<QuestProgress> AcceptQuestAsync(string playerId, ObjectId questId);
    Task UpdateObjectiveProgressAsync(string playerId, ObjectId questId, ObjectId objectiveId, int progressAmount = 1);
    Task<bool> CompleteQuestAsync(string playerId, ObjectId questId);
    Task<List<QuestProgress>> GetPlayerQuestProgressAsync(string playerId);
    Task<bool> ValidateQuestAccessAsync(string playerId, ObjectId questId);
    Task<bool> ArePrerequisitesMetAsync(string playerId, ObjectId questId);
    Task<(bool success, string message)> DispenseRewardsAsync(string playerId, ObjectId questId);
}

public class QuestService : IQuestService
{
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<QuestService> _logger;

    public QuestService(IMongoDbService mongoDbService, ILogger<QuestService> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all quests available to a player based on their level.
    /// </summary>
    public async Task<List<Quest>> GetAvailableQuestsAsync(int playerLevel)
    {
        try
        {
            var questsCollection = _mongoDbService.GetQuestsCollection();
            var filter = Builders<Quest>.Filter.Lte(q => q.LevelRequirement, playerLevel);
            
            return await questsCollection
                .Find(filter)
                .SortByDescending(q => q.LevelRequirement)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available quests");
            return new List<Quest>();
        }
    }

    /// <summary>
    /// Gets detailed information about a specific quest.
    /// </summary>
    public async Task<QuestDetailsDto> GetQuestDetailsAsync(ObjectId questId)
    {
        try
        {
            var questsCollection = _mongoDbService.GetQuestsCollection();
            var quest = await questsCollection.Find(q => q.Id == questId).FirstOrDefaultAsync();

            if (quest == null)
                return null;

            var objectives = await GetObjectivesAsync(quest.ObjectiveIds);
            var rewards = await GetRewardsAsync(quest.RewardIds);

            return new QuestDetailsDto
            {
                Id = quest.Id,
                QuestId = quest.QuestId,
                Title = quest.Title,
                Description = quest.Description,
                GiverNpcName = quest.GiverNpcName,
                LevelRequirement = quest.LevelRequirement,
                Objectives = objectives,
                Rewards = rewards,
                IsRepeatable = quest.IsRepeatable,
                CreatedAt = quest.CreatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quest details");
            return null;
        }
    }

    /// <summary>
    /// Accepts a quest for a player, creating a QuestProgress entry.
    /// </summary>
    public async Task<QuestProgress> AcceptQuestAsync(string playerId, ObjectId questId)
    {
        try
        {
            // Validate quest exists
            var questsCollection = _mongoDbService.GetQuestsCollection();
            var quest = await questsCollection.Find(q => q.Id == questId).FirstOrDefaultAsync();
            
            if (quest == null)
                return null;

            // Check if already accepted/completed
            var progressCollection = _mongoDbService.GetQuestProgressCollection();
            var existingProgress = await progressCollection
                .Find(qp => qp.PlayerId == playerId && qp.QuestId == questId)
                .FirstOrDefaultAsync();

            if (existingProgress != null)
                return existingProgress;

            // Create objective progress entries
            var objectivesCollection = _mongoDbService.GetObjectivesCollection();
            var objectives = await objectivesCollection
                .Find(o => quest.ObjectiveIds.Contains(o.Id))
                .ToListAsync();

            var objectiveProgress = objectives.Select(obj => new ObjectiveProgress
            {
                ObjectiveId = obj.Id,
                ObjectiveType = obj.Type,
                Completed = false,
                Progress = 0,
                Target = obj.TargetQuantity
            }).ToList();

            var questProgress = new QuestProgress
            {
                PlayerId = playerId,
                QuestId = questId,
                QuestName = quest.Title,
                Status = "accepted",
                ObjectivesProgress = objectiveProgress,
                StartedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await progressCollection.InsertOneAsync(questProgress);
            _logger.LogInformation($"Player {playerId} accepted quest {quest.Title}");

            return questProgress;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error accepting quest");
            return null;
        }
    }

    /// <summary>
    /// Updates progress on a specific objective.
    /// </summary>
    public async Task UpdateObjectiveProgressAsync(string playerId, ObjectId questId, ObjectId objectiveId, int progressAmount = 1)
    {
        try
        {
            var progressCollection = _mongoDbService.GetQuestProgressCollection();
            var questProgress = await progressCollection
                .Find(qp => qp.PlayerId == playerId && qp.QuestId == questId)
                .FirstOrDefaultAsync();

            if (questProgress == null)
                return;

            var objective = questProgress.ObjectivesProgress
                .FirstOrDefault(op => op.ObjectiveId == objectiveId);

            if (objective == null)
                return;

            // Update progress
            objective.Progress += progressAmount;
            if (objective.Progress >= objective.Target)
            {
                objective.Completed = true;
                objective.Progress = objective.Target;
            }

            // Update status
            questProgress.Status = questProgress.ObjectivesProgress.All(op => op.Completed) 
                ? "completed" 
                : "in_progress";
            questProgress.UpdatedAt = DateTime.UtcNow;

            var filter = Builders<QuestProgress>.Filter.And(
                Builders<QuestProgress>.Filter.Eq(qp => qp.PlayerId, playerId),
                Builders<QuestProgress>.Filter.Eq(qp => qp.QuestId, questId)
            );

            await progressCollection.ReplaceOneAsync(filter, questProgress);
            _logger.LogInformation($"Updated objective progress for player {playerId} on quest {questId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating objective progress");
        }
    }

    /// <summary>
    /// Completes a quest if all objectives are finished.
    /// </summary>
    public async Task<bool> CompleteQuestAsync(string playerId, ObjectId questId)
    {
        try
        {
            var progressCollection = _mongoDbService.GetQuestProgressCollection();
            var questProgress = await progressCollection
                .Find(qp => qp.PlayerId == playerId && qp.QuestId == questId)
                .FirstOrDefaultAsync();

            if (questProgress == null)
                return false;

            // Check all objectives are complete
            if (!questProgress.ObjectivesProgress.All(op => op.Completed))
                return false;

            // Mark as completed
            questProgress.Status = "completed";
            questProgress.CompletedAt = DateTime.UtcNow;
            questProgress.UpdatedAt = DateTime.UtcNow;

            var filter = Builders<QuestProgress>.Filter.And(
                Builders<QuestProgress>.Filter.Eq(qp => qp.PlayerId, playerId),
                Builders<QuestProgress>.Filter.Eq(qp => qp.QuestId, questId)
            );

            await progressCollection.ReplaceOneAsync(filter, questProgress);

            // Dispense rewards
            var (success, message) = await DispenseRewardsAsync(playerId, questId);
            _logger.LogInformation($"Quest {questId} completed by player {playerId}. Rewards: {message}");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing quest");
            return false;
        }
    }

    /// <summary>
    /// Gets all quest progress records for a player.
    /// </summary>
    public async Task<List<QuestProgress>> GetPlayerQuestProgressAsync(string playerId)
    {
        try
        {
            var progressCollection = _mongoDbService.GetQuestProgressCollection();
            return await progressCollection
                .Find(qp => qp.PlayerId == playerId)
                .SortByDescending(qp => qp.UpdatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving player quest progress");
            return new List<QuestProgress>();
        }
    }

    /// <summary>
    /// Validates if a player can accept a quest.
    /// </summary>
    public async Task<bool> ValidateQuestAccessAsync(string playerId, ObjectId questId)
    {
        try
        {
            var questsCollection = _mongoDbService.GetQuestsCollection();
            var quest = await questsCollection.Find(q => q.Id == questId).FirstOrDefaultAsync();

            if (quest == null)
                return false;

            // Check prerequisites
            if (!await ArePrerequisitesMetAsync(playerId, questId))
                return false;

            // Check if already completed (non-repeatable)
            var progressCollection = _mongoDbService.GetQuestProgressCollection();
            var completed = await progressCollection
                .Find(qp => qp.PlayerId == playerId && qp.QuestId == questId && qp.Status == "completed")
                .FirstOrDefaultAsync();

            if (completed != null && !quest.IsRepeatable)
                return false;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating quest access");
            return false;
        }
    }

    /// <summary>
    /// Checks if all prerequisite quests are completed.
    /// </summary>
    public async Task<bool> ArePrerequisitesMetAsync(string playerId, ObjectId questId)
    {
        try
        {
            var questsCollection = _mongoDbService.GetQuestsCollection();
            var quest = await questsCollection.Find(q => q.Id == questId).FirstOrDefaultAsync();

            if (quest == null || quest.PrerequisiteQuestIds.Count == 0)
                return true;

            var progressCollection = _mongoDbService.GetQuestProgressCollection();

            foreach (var prereqId in quest.PrerequisiteQuestIds)
            {
                var completed = await progressCollection
                    .Find(qp => qp.PlayerId == playerId && qp.QuestId == prereqId && qp.Status == "completed")
                    .FirstOrDefaultAsync();

                if (completed == null)
                    return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking prerequisites");
            return false;
        }
    }

    /// <summary>
    /// Dispenses rewards to a player for completing a quest.
    /// </summary>
    public async Task<(bool success, string message)> DispenseRewardsAsync(string playerId, ObjectId questId)
    {
        try
        {
            var rewardsCollection = _mongoDbService.GetQuestRewardsCollection();
            var rewards = await rewardsCollection
                .Find(r => r.QuestId == questId)
                .ToListAsync();

            var rewardMessages = new List<string>();

            foreach (var reward in rewards)
            {
                switch (reward.RewardType.ToLower())
                {
                    case "xp":
                        rewardMessages.Add($"{reward.RewardValue} XP");
                        break;
                    case "gold":
                        rewardMessages.Add($"{reward.RewardValue} Gold");
                        break;
                    case "item":
                        rewardMessages.Add($"{reward.Quantity}x {reward.RewardItemName}");
                        break;
                }
            }

            var message = string.Join(", ", rewardMessages);
            return (true, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error dispensing rewards");
            return (false, "Error dispensing rewards");
        }
    }

    private async Task<List<ObjectiveDto>> GetObjectivesAsync(List<ObjectId> objectiveIds)
    {
        var objectivesCollection = _mongoDbService.GetObjectivesCollection();
        var objectives = await objectivesCollection
            .Find(o => objectiveIds.Contains(o.Id))
            .ToListAsync();

        return objectives.Select(obj => new ObjectiveDto
        {
            Id = obj.Id,
            Type = obj.Type,
            Description = obj.Description,
            TargetName = obj.TargetName,
            TargetQuantity = obj.TargetQuantity,
            Optional = obj.Optional
        }).ToList();
    }

    private async Task<List<QuestRewardDto>> GetRewardsAsync(List<ObjectId> rewardIds)
    {
        var rewardsCollection = _mongoDbService.GetQuestRewardsCollection();
        var rewards = await rewardsCollection
            .Find(r => rewardIds.Contains(r.Id))
            .ToListAsync();

        return rewards.Select(reward => new QuestRewardDto
        {
            RewardType = reward.RewardType,
            RewardValue = reward.RewardValue,
            RewardItemName = reward.RewardItemName,
            Quantity = reward.Quantity
        }).ToList();
    }
}
