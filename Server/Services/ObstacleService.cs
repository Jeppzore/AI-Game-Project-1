using MongoDB.Driver;
using Server.Models;

namespace Server.Services;

/// <summary>
/// Service for managing obstacle interactions and challenge logic.
/// </summary>
public interface IObstacleService
{
    /// <summary>
    /// Get obstacle details by ID.
    /// </summary>
    Task<Obstacle?> GetObstacleAsync(string obstacleId);

    /// <summary>
    /// Submit an attempt/solution for an obstacle.
    /// </summary>
    Task<ObstacleResult> AttemptObstacleAsync(string playerId, string obstacleId, string solution);

    /// <summary>
    /// Get player's attempt history for an obstacle.
    /// </summary>
    Task<List<ObstacleAttempt>> GetPlayerAttemptsAsync(string playerId, string obstacleId);

    /// <summary>
    /// Get all attempts by a player.
    /// </summary>
    Task<List<ObstacleAttempt>> GetPlayerAllAttemptsAsync(string playerId);

    /// <summary>
    /// Log an obstacle attempt to the database.
    /// </summary>
    Task LogAttemptAsync(ObstacleAttempt attempt);

    /// <summary>
    /// Validate a solution for a riddle obstacle.
    /// </summary>
    bool ValidateRiddleSolution(RiddleChallenge riddle, string solution);

    /// <summary>
    /// Check if chest can be opened (has key or sufficient skill).
    /// </summary>
    Task<bool> CanOpenChestAsync(string playerId, string requiredKeyId);

    /// <summary>
    /// Evaluate if trap was detected and/or disarmed successfully.
    /// </summary>
    (bool detected, bool disarmed) EvaluateTrapAttempt(TrapChallenge trap, int playerPerception, int playerLockpicking);

    /// <summary>
    /// Calculate success rate based on difficulty and player level.
    /// </summary>
    double CalculateSuccessRate(int difficulty, int playerLevel, double modifier);
}

public class ObstacleService : IObstacleService
{
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<ObstacleService> _logger;
    private const int MaxAttemptsPerObstacle = 10;

    public ObstacleService(IMongoDbService mongoDbService, ILogger<ObstacleService> logger)
    {
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    /// <summary>
    /// Get obstacle details by ID.
    /// </summary>
    public async Task<Obstacle?> GetObstacleAsync(string obstacleId)
    {
        try
        {
            var collection = _mongoDbService.GetObstaclesCollection();
            return await collection.Find(o => o.Id == obstacleId || o.ObstacleId == obstacleId)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving obstacle {ObstacleId}", obstacleId);
            return null;
        }
    }

    /// <summary>
    /// Submit an attempt/solution for an obstacle.
    /// </summary>
    public async Task<ObstacleResult> AttemptObstacleAsync(string playerId, string obstacleId, string solution)
    {
        var obstacle = await GetObstacleAsync(obstacleId);
        if (obstacle == null)
        {
            return new ObstacleResult
            {
                Success = false,
                Message = "Obstacle not found"
            };
        }

        // Get attempt history
        var previousAttempts = await GetPlayerAttemptsAsync(playerId, obstacleId);
        if (previousAttempts.Count >= MaxAttemptsPerObstacle)
        {
            return new ObstacleResult
            {
                Success = false,
                Message = $"Maximum attempts ({MaxAttemptsPerObstacle}) exceeded for this obstacle"
            };
        }

        bool success = false;
        int attemptNumber = previousAttempts.Count + 1;

        // Validate solution based on obstacle type
        switch (obstacle.Type.ToLower())
        {
            case "riddle":
                if (obstacle.ChallengeData is RiddleChallenge riddle)
                {
                    success = ValidateRiddleSolution(riddle, solution);
                }
                break;

            case "locked_chest":
                // For chests, solution should contain key ID or lockpicking attempt
                success = await CanOpenChestAsync(playerId, solution);
                break;

            case "trapped_door":
                // For traps, solution indicates detection/disarm attempt
                success = await AttemptDisarmTrapAsync(playerId, obstacle, solution);
                break;

            case "puzzle":
                // For puzzles, solution should match required steps
                if (obstacle.ChallengeData is PuzzleChallenge puzzle)
                {
                    success = ValidatePuzzleSolution(puzzle, solution);
                }
                break;
        }

        // Log the attempt
        var attempt = new ObstacleAttempt
        {
            PlayerId = playerId,
            ObstacleId = obstacleId,
            Success = success,
            AttemptNumber = attemptNumber,
            Timestamp = DateTime.UtcNow,
            SolutionProvided = solution,
            RewardsClaimed = false
        };

        await LogAttemptAsync(attempt);

        // Generate result
        var result = new ObstacleResult
        {
            Success = success,
            Message = success
                ? $"Success! You have overcome the {obstacle.Name}."
                : $"Attempt {attemptNumber} failed. Try again (attempts remaining: {MaxAttemptsPerObstacle - attemptNumber})"
        };

        if (success)
        {
            // Award rewards
            result.ExperienceGained = obstacle.Rewards.Experience;
            result.GoldGained = obstacle.Rewards.Gold;

            // Calculate item drops based on drop chances
            foreach (var rewardItem in obstacle.Rewards.Items)
            {
                if (Random.Shared.NextDouble() <= rewardItem.DropChance)
                {
                    result.ItemsGained.Add(new ItemReward
                    {
                        ItemId = rewardItem.ItemId,
                        ItemName = rewardItem.ItemId, // In real impl, would look up item name
                        Quantity = rewardItem.Quantity
                    });
                }
            }

            attempt.RewardsClaimed = true;
            await UpdateAttemptAsync(attempt);
        }
        else
        {
            // Apply penalties on failure for traps
            if (obstacle.Type.ToLower() == "trapped_door" && obstacle.ChallengeData is TrapChallenge trap)
            {
                result.DamageTaken = 10 + (trap.DetectionDifficulty * 2);
                attempt.DamageReceived = result.DamageTaken;
                await UpdateAttemptAsync(attempt);
            }
        }

        return result;
    }

    /// <summary>
    /// Validate riddle solution with case-insensitive matching.
    /// </summary>
    public bool ValidateRiddleSolution(RiddleChallenge riddle, string solution)
    {
        if (string.IsNullOrWhiteSpace(riddle.CorrectAnswer))
            return false;

        var normalizedSolution = solution.Trim().ToLowerInvariant();
        var normalizedAnswer = riddle.CorrectAnswer.Trim().ToLowerInvariant();

        return normalizedSolution == normalizedAnswer;
    }

    /// <summary>
    /// Validate puzzle solution.
    /// </summary>
    private bool ValidatePuzzleSolution(PuzzleChallenge puzzle, string solution)
    {
        // In a real implementation, this would validate complex puzzle states
        // For now, check if solution contains key puzzle terms
        var solutionLower = solution.ToLowerInvariant();
        return puzzle.Steps.Count > 0 && !string.IsNullOrWhiteSpace(solution);
    }

    /// <summary>
    /// Attempt to disarm a trap.
    /// </summary>
    private async Task<bool> AttemptDisarmTrapAsync(string playerId, Obstacle obstacle, string solution)
    {
        if (!(obstacle.ChallengeData is TrapChallenge trap))
            return false;

        // For demo purposes, success chance based on difficulty
        // In real impl would check player skills (lockpicking, perception)
        int detectionSkill = 5; // Hardcoded for now
        int disarmSkill = 4;

        bool detected = detectionSkill >= trap.DetectionDifficulty;
        bool disarmed = detected && disarmSkill >= trap.DisarmDifficulty;

        return disarmed;
    }

    /// <summary>
    /// Check if chest can be opened.
    /// </summary>
    public async Task<bool> CanOpenChestAsync(string playerId, string requiredKeyId)
    {
        // In real implementation, would check player inventory
        // For demo, accept if solution matches required key ID
        return !string.IsNullOrWhiteSpace(requiredKeyId) && 
               requiredKeyId.Equals(requiredKeyId, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Evaluate trap attempt with perception and lockpicking skills.
    /// </summary>
    public (bool detected, bool disarmed) EvaluateTrapAttempt(TrapChallenge trap, int playerPerception, int playerLockpicking)
    {
        bool detected = playerPerception >= trap.DetectionDifficulty;
        bool disarmed = detected && playerLockpicking >= trap.DisarmDifficulty;
        return (detected, disarmed);
    }

    /// <summary>
    /// Calculate success rate based on difficulty and player level.
    /// </summary>
    public double CalculateSuccessRate(int difficulty, int playerLevel, double modifier)
    {
        // Base success rate: 90% at player level, decreases with difficulty
        double baseRate = 0.9;
        double difficultyPenalty = difficulty * 0.05; // 5% per difficulty level
        double levelBonus = (playerLevel - 1) * 0.02; // 2% per level above 1

        double successRate = baseRate - difficultyPenalty + levelBonus;
        successRate *= modifier;

        // Clamp between 5% and 95%
        return Math.Max(0.05, Math.Min(0.95, successRate));
    }

    /// <summary>
    /// Get player's attempt history for a specific obstacle.
    /// </summary>
    public async Task<List<ObstacleAttempt>> GetPlayerAttemptsAsync(string playerId, string obstacleId)
    {
        try
        {
            var collection = _mongoDbService.GetObstacleAttemptsCollection();
            return await collection
                .Find(a => a.PlayerId == playerId && (a.ObstacleId == obstacleId))
                .SortByDescending(a => a.Timestamp)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving attempts for player {PlayerId} on obstacle {ObstacleId}", playerId, obstacleId);
            return new List<ObstacleAttempt>();
        }
    }

    /// <summary>
    /// Get all attempts by a player.
    /// </summary>
    public async Task<List<ObstacleAttempt>> GetPlayerAllAttemptsAsync(string playerId)
    {
        try
        {
            var collection = _mongoDbService.GetObstacleAttemptsCollection();
            return await collection
                .Find(a => a.PlayerId == playerId)
                .SortByDescending(a => a.Timestamp)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all attempts for player {PlayerId}", playerId);
            return new List<ObstacleAttempt>();
        }
    }

    /// <summary>
    /// Log an obstacle attempt to the database.
    /// </summary>
    public async Task LogAttemptAsync(ObstacleAttempt attempt)
    {
        try
        {
            var collection = _mongoDbService.GetObstacleAttemptsCollection();
            await collection.InsertOneAsync(attempt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging attempt for player {PlayerId}", attempt.PlayerId);
        }
    }

    /// <summary>
    /// Update an obstacle attempt.
    /// </summary>
    private async Task UpdateAttemptAsync(ObstacleAttempt attempt)
    {
        try
        {
            var collection = _mongoDbService.GetObstacleAttemptsCollection();
            await collection.ReplaceOneAsync(a => a.Id == attempt.Id, attempt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating attempt {AttemptId}", attempt.Id);
        }
    }
}
