using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

/// <summary>
/// Enum for obstacle types.
/// </summary>
public enum ObstacleType
{
    Riddle,
    LockedChest,
    TrappedDoor,
    Puzzle
}

/// <summary>
/// Base class for challenge-specific data.
/// </summary>
[BsonKnownTypes(typeof(RiddleChallenge), typeof(ChestChallenge), typeof(TrapChallenge), typeof(PuzzleChallenge))]
public abstract class ChallengeData
{
    [BsonElement("type")]
    public abstract string ChallengeType { get; }
}

/// <summary>
/// Riddle challenge data.
/// </summary>
public class RiddleChallenge : ChallengeData
{
    public override string ChallengeType => "riddle";

    [BsonElement("question")]
    public string Question { get; set; } = string.Empty;

    [BsonElement("correctAnswer")]
    public string CorrectAnswer { get; set; } = string.Empty;

    [BsonElement("hints")]
    public List<string> Hints { get; set; } = new();
}

/// <summary>
/// Locked chest challenge data.
/// </summary>
public class ChestChallenge : ChallengeData
{
    public override string ChallengeType => "locked_chest";

    [BsonElement("lockDifficulty")]
    public int LockDifficulty { get; set; }

    [BsonElement("requiredKeyId")]
    public string RequiredKeyId { get; set; } = string.Empty;
}

/// <summary>
/// Trapped door challenge data.
/// </summary>
public class TrapChallenge : ChallengeData
{
    public override string ChallengeType => "trapped_door";

    [BsonElement("triggerType")]
    public string TriggerType { get; set; } = string.Empty;

    [BsonElement("detectionDifficulty")]
    public int DetectionDifficulty { get; set; }

    [BsonElement("disarmDifficulty")]
    public int DisarmDifficulty { get; set; }
}

/// <summary>
/// Puzzle challenge data.
/// </summary>
public class PuzzleChallenge : ChallengeData
{
    public override string ChallengeType => "puzzle";

    [BsonElement("steps")]
    public List<string> Steps { get; set; } = new();

    [BsonElement("requiredItems")]
    public List<string> RequiredItems { get; set; } = new();
}

/// <summary>
/// Reward structure for obstacle completion.
/// </summary>
public class ObstacleReward
{
    [BsonElement("items")]
    public List<RewardItem> Items { get; set; } = new();

    [BsonElement("experience")]
    public int Experience { get; set; }

    [BsonElement("gold")]
    public int Gold { get; set; }
}

/// <summary>
/// Individual item reward in an obstacle reward pool.
/// </summary>
public class RewardItem
{
    [BsonElement("itemId")]
    public string ItemId { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public int Quantity { get; set; } = 1;

    [BsonElement("dropChance")]
    public double DropChance { get; set; } = 1.0;
}

/// <summary>
/// Represents an interactive obstacle or challenge in the game world.
/// </summary>
[BsonIgnoreExtraElements]
public class Obstacle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("id")]
    public string ObstacleId { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("location")]
    public string Location { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = "riddle"; // riddle, locked_chest, trapped_door, puzzle

    [BsonElement("difficulty")]
    public int Difficulty { get; set; } = 1; // 1-10 scale

    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [BsonElement("challengeData")]
    public ChallengeData? ChallengeData { get; set; }

    [BsonElement("rewards")]
    public ObstacleReward Rewards { get; set; } = new();

    [BsonElement("successRateModifier")]
    public double SuccessRateModifier { get; set; } = 1.0;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Tracks a player's attempt on an obstacle.
/// </summary>
[BsonIgnoreExtraElements]
public class ObstacleAttempt
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("playerId")]
    public string PlayerId { get; set; } = string.Empty;

    [BsonElement("obstacleId")]
    public string ObstacleId { get; set; } = string.Empty;

    [BsonElement("success")]
    public bool Success { get; set; }

    [BsonElement("attemptNumber")]
    public int AttemptNumber { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [BsonElement("solutionProvided")]
    public string SolutionProvided { get; set; } = string.Empty;

    [BsonElement("damageReceived")]
    public int DamageReceived { get; set; } = 0;

    [BsonElement("rewardsClaimed")]
    public bool RewardsClaimed { get; set; }
}

/// <summary>
/// Result of an obstacle attempt.
/// </summary>
public class ObstacleResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ExperienceGained { get; set; }
    public int GoldGained { get; set; }
    public List<ItemReward> ItemsGained { get; set; } = new();
    public int DamageTaken { get; set; }
}

/// <summary>
/// Item reward result.
/// </summary>
public class ItemReward
{
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
