using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Server.Models;

/// <summary>
/// Represents a quest in the game world.
/// </summary>
[BsonIgnoreExtraElements]
public class Quest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("questId")]
    public string QuestId { get; set; } = string.Empty;

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("giver_npc_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId GiverNpcId { get; set; }

    [BsonElement("giver_npc_name")]
    public string GiverNpcName { get; set; } = string.Empty;

    [BsonElement("level_requirement")]
    public int LevelRequirement { get; set; } = 1;

    [BsonElement("objectives")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<ObjectId> ObjectiveIds { get; set; } = new List<ObjectId>();

    [BsonElement("rewards")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<ObjectId> RewardIds { get; set; } = new List<ObjectId>();

    [BsonElement("prerequisites")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<ObjectId> PrerequisiteQuestIds { get; set; } = new List<ObjectId>();

    [BsonElement("is_repeatable")]
    public bool IsRepeatable { get; set; } = false;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents an individual objective within a quest.
/// </summary>
[BsonIgnoreExtraElements]
public class Objective
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("objectiveId")]
    public string ObjectiveId { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty; // kill_enemy, collect_item, reach_location, talk_to_npc, loot_item

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("target_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId TargetId { get; set; }

    [BsonElement("target_name")]
    public string TargetName { get; set; } = string.Empty;

    [BsonElement("target_quantity")]
    public int TargetQuantity { get; set; } = 1;

    [BsonElement("optional")]
    public bool Optional { get; set; } = false;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Enum for objective types.
/// </summary>
public enum ObjectiveType
{
    KillEnemy = 0,
    CollectItem = 1,
    ReachLocation = 2,
    TalkToNpc = 3,
    LootItem = 4
}

/// <summary>
/// Tracks player progress on a specific quest.
/// </summary>
[BsonIgnoreExtraElements]
public class QuestProgress
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("player_id")]
    public string PlayerId { get; set; } = string.Empty;

    [BsonElement("quest_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId QuestId { get; set; }

    [BsonElement("quest_name")]
    public string QuestName { get; set; } = string.Empty;

    [BsonElement("status")]
    public string Status { get; set; } = "not_started"; // not_started, accepted, in_progress, completed, abandoned

    [BsonElement("objectives_progress")]
    public List<ObjectiveProgress> ObjectivesProgress { get; set; } = new List<ObjectiveProgress>();

    [BsonElement("started_at")]
    public DateTime? StartedAt { get; set; }

    [BsonElement("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [BsonElement("abandoned_at")]
    public DateTime? AbandonedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Tracks progress on a specific objective.
/// </summary>
public class ObjectiveProgress
{
    [BsonElement("objective_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId ObjectiveId { get; set; }

    [BsonElement("objective_type")]
    public string ObjectiveType { get; set; } = string.Empty;

    [BsonElement("completed")]
    public bool Completed { get; set; } = false;

    [BsonElement("progress")]
    public int Progress { get; set; } = 0;

    [BsonElement("target")]
    public int Target { get; set; } = 1;
}

/// <summary>
/// Represents a reward for completing a quest.
/// </summary>
[BsonIgnoreExtraElements]
public class QuestReward
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("reward_id")]
    public string RewardId { get; set; } = string.Empty;

    [BsonElement("quest_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId QuestId { get; set; }

    [BsonElement("reward_type")]
    public string RewardType { get; set; } = string.Empty; // item, xp, gold

    [BsonElement("reward_value")]
    public int RewardValue { get; set; } = 0;

    [BsonElement("reward_item_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? RewardItemId { get; set; }

    [BsonElement("reward_item_name")]
    public string RewardItemName { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public int Quantity { get; set; } = 1;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Enum for reward types.
/// </summary>
public enum RewardType
{
    Item = 0,
    Experience = 1,
    Gold = 2
}

/// <summary>
/// Request DTO for accepting a quest.
/// </summary>
public class AcceptQuestRequest
{
    public string PlayerId { get; set; } = string.Empty;
}

/// <summary>
/// Request DTO for updating quest progress.
/// </summary>
public class UpdateQuestProgressRequest
{
    public string PlayerId { get; set; } = string.Empty;
    public ObjectId ObjectiveId { get; set; }
    public int ProgressAmount { get; set; } = 1;
}

/// <summary>
/// Request DTO for completing a quest.
/// </summary>
public class CompleteQuestRequest
{
    public string PlayerId { get; set; } = string.Empty;
}

/// <summary>
/// Response DTO for quest details.
/// </summary>
public class QuestDetailsDto
{
    public ObjectId Id { get; set; }
    public string QuestId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string GiverNpcName { get; set; } = string.Empty;
    public int LevelRequirement { get; set; }
    public List<ObjectiveDto> Objectives { get; set; } = new List<ObjectiveDto>();
    public List<QuestRewardDto> Rewards { get; set; } = new List<QuestRewardDto>();
    public bool IsRepeatable { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Response DTO for objectives.
/// </summary>
public class ObjectiveDto
{
    public ObjectId Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TargetName { get; set; } = string.Empty;
    public int TargetQuantity { get; set; }
    public bool Optional { get; set; }
}

/// <summary>
/// Response DTO for quest rewards.
/// </summary>
public class QuestRewardDto
{
    public string RewardType { get; set; } = string.Empty;
    public int RewardValue { get; set; }
    public string RewardItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

/// <summary>
/// Response DTO for player's quest progress.
/// </summary>
public class QuestProgressDto
{
    public ObjectId Id { get; set; }
    public string QuestName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<ObjectiveProgressDto> ObjectivesProgress { get; set; } = new List<ObjectiveProgressDto>();
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

/// <summary>
/// Response DTO for objective progress.
/// </summary>
public class ObjectiveProgressDto
{
    public string ObjectiveType { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public int Progress { get; set; }
    public int Target { get; set; }
}
