using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace NightmaresWiki.Models
{
    /// <summary>
    /// Represents a creature/monster/boss in the Nightmares game world.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Creature
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("id")]
        public string CreatureId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("lore")]
        public string Lore { get; set; }

        [BsonElement("type")]
        public List<string> Types { get; set; } = new List<string>();

        [BsonElement("image")]
        public string ImageFileName { get; set; }

        [BsonElement("combat")]
        public CombatStats Combat { get; set; }

        [BsonElement("loot")]
        public List<LootDrop> LootDrops { get; set; } = new List<LootDrop>();

        [BsonElement("behavior")]
        public string Behavior { get; set; }

        [BsonElement("difficulty")]
        public string Difficulty { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Combat statistics for a creature.
    /// </summary>
    public class CombatStats
    {
        [BsonElement("health")]
        public int Health { get; set; }

        [BsonElement("experience")]
        public int Experience { get; set; }

        [BsonElement("armor")]
        public int Armor { get; set; }

        [BsonElement("speed")]
        public int Speed { get; set; }

        [BsonElement("damage")]
        public DamageRange Damage { get; set; }

        [BsonElement("aggroRange")]
        public int AggroRange { get; set; }

        [BsonElement("immunities")]
        public List<string> Immunities { get; set; } = new List<string>();

        [BsonElement("abilities")]
        public List<string> Abilities { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents a damage range (min-max).
    /// </summary>
    public class DamageRange
    {
        [BsonElement("min")]
        public int Min { get; set; }

        [BsonElement("max")]
        public int Max { get; set; }

        public DamageRange() { }

        public DamageRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

    /// <summary>
    /// Represents an item drop from a creature.
    /// </summary>
    public class LootDrop
    {
        [BsonElement("itemId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? ItemId { get; set; }

        [BsonElement("itemName")]
        public string ItemName { get; set; }

        [BsonElement("quantity")]
        public QuantityRange Quantity { get; set; }

        [BsonElement("dropChance")]
        public double DropChance { get; set; } = 1.0;
    }

    /// <summary>
    /// Represents a quantity range (min-max).
    /// </summary>
    public class QuantityRange
    {
        [BsonElement("min")]
        public int Min { get; set; }

        [BsonElement("max")]
        public int Max { get; set; }

        public QuantityRange() { }

        public QuantityRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

    /// <summary>
    /// Represents an item in the game world.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("id")]
        public string ItemId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("image")]
        public string ImageFileName { get; set; }

        [BsonElement("stats")]
        public ItemStats Stats { get; set; }

        [BsonElement("requirements")]
        public ItemRequirements Requirements { get; set; }

        [BsonElement("value")]
        public ItemValue Value { get; set; }

        [BsonElement("sources")]
        public List<ItemSource> Sources { get; set; } = new List<ItemSource>();

        [BsonElement("crafting")]
        public CraftingInfo Crafting { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Statistics for an item (damage, armor, speed, etc.).
    /// </summary>
    public class ItemStats
    {
        [BsonElement("damage")]
        public DamageRange Damage { get; set; }

        [BsonElement("armor")]
        public int? Armor { get; set; }

        [BsonElement("speed")]
        public int? Speed { get; set; }

        [BsonElement("hands")]
        public int? Hands { get; set; }
    }

    /// <summary>
    /// Requirements to use an item.
    /// </summary>
    public class ItemRequirements
    {
        [BsonElement("level")]
        public int? Level { get; set; }

        [BsonElement("strength")]
        public int? Strength { get; set; }

        [BsonElement("agility")]
        public int? Agility { get; set; }

        [BsonElement("intelligence")]
        public int? Intelligence { get; set; }
    }

    /// <summary>
    /// Value and rarity of an item.
    /// </summary>
    public class ItemValue
    {
        [BsonElement("gold")]
        public int Gold { get; set; }

        [BsonElement("rarity")]
        public string Rarity { get; set; } // "common", "uncommon", "rare", "epic", "legendary"
    }

    /// <summary>
    /// Source where an item can be obtained.
    /// </summary>
    public class ItemSource
    {
        [BsonElement("type")]
        public string Type { get; set; } // "creature_drop", "craft", "vendor", "quest"

        [BsonElement("creatureId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? CreatureId { get; set; }

        [BsonElement("creatureName")]
        public string CreatureName { get; set; }

        [BsonElement("dropChance")]
        public double DropChance { get; set; }
    }

    /// <summary>
    /// Crafting information for an item.
    /// </summary>
    public class CraftingInfo
    {
        [BsonElement("craftable")]
        public bool Craftable { get; set; }

        [BsonElement("recipes")]
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    }

    /// <summary>
    /// A crafting recipe.
    /// </summary>
    public class Recipe
    {
        [BsonElement("materials")]
        public List<RecipeMaterial> Materials { get; set; } = new List<RecipeMaterial>();

        [BsonElement("skill")]
        public string Skill { get; set; }

        [BsonElement("skillLevel")]
        public int SkillLevel { get; set; }
    }

    /// <summary>
    /// A material required for a recipe.
    /// </summary>
    public class RecipeMaterial
    {
        [BsonElement("itemId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ItemId { get; set; }

        [BsonElement("materialName")]
        public string MaterialName { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Represents a Non-Player Character in the game world.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class NPC
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("id")]
        public string NPCId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("image")]
        public string ImageFileName { get; set; }

        [BsonElement("profile")]
        public NPCProfile Profile { get; set; }

        [BsonElement("trades")]
        public List<Trade> Trades { get; set; } = new List<Trade>();

        [BsonElement("quests")]
        public List<NPCQuest> Quests { get; set; } = new List<NPCQuest>();

        [BsonElement("dialogue")]
        public List<DialogueEntry> Dialogue { get; set; } = new List<DialogueEntry>();

        [BsonElement("relationships")]
        public NPCRelationships Relationships { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Profile information for an NPC.
    /// </summary>
    public class NPCProfile
    {
        [BsonElement("role")]
        public string Role { get; set; } // "Blacksmith", "Merchant", "Quest Giver", etc.

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("coordinates")]
        public Coordinates Coordinates { get; set; }
    }

    /// <summary>
    /// Coordinates for positioning.
    /// </summary>
    public class Coordinates
    {
        [BsonElement("x")]
        public float X { get; set; }

        [BsonElement("y")]
        public float Y { get; set; }

        public Coordinates() { }

        public Coordinates(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// A trade between player and NPC.
    /// </summary>
    public class Trade
    {
        [BsonElement("buys")]
        public TradeItem Buys { get; set; }

        [BsonElement("sells")]
        public TradeItem Sells { get; set; }
    }

    /// <summary>
    /// An item in a trade transaction.
    /// </summary>
    public class TradeItem
    {
        [BsonElement("itemId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ItemId { get; set; }

        [BsonElement("itemName")]
        public string ItemName { get; set; }

        [BsonElement("basePrice")]
        public int BasePrice { get; set; }
    }

    /// <summary>
    /// A quest associated with an NPC.
    /// </summary>
    public class NPCQuest
    {
        [BsonElement("questId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId QuestId { get; set; }

        [BsonElement("questName")]
        public string QuestName { get; set; }
    }

    /// <summary>
    /// A dialogue entry for NPC conversations.
    /// </summary>
    public class DialogueEntry
    {
        [BsonElement("trigger")]
        public string Trigger { get; set; } // "greeting", "quest_start", "quest_complete", etc.

        [BsonElement("text")]
        public string Text { get; set; }
    }

    /// <summary>
    /// Relationship information for an NPC.
    /// </summary>
    public class NPCRelationships
    {
        [BsonElement("friends")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<ObjectId> Friends { get; set; } = new List<ObjectId>();

        [BsonElement("enemies")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<ObjectId> Enemies { get; set; } = new List<ObjectId>();

        [BsonElement("faction")]
        public string Faction { get; set; }
    }

    /// <summary>
    /// Represents an obstacle or interactive object in the game world.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Obstacle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("id")]
        public string ObstacleId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } // "door", "trap", "wall", "chest", etc.

        [BsonElement("image")]
        public string ImageFileName { get; set; }

        [BsonElement("properties")]
        public ObstacleProperties Properties { get; set; }

        [BsonElement("requirements")]
        public ObstacleRequirements Requirements { get; set; }

        [BsonElement("effects")]
        public ObstacleEffects Effects { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Properties of an obstacle.
    /// </summary>
    public class ObstacleProperties
    {
        [BsonElement("passable")]
        public bool Passable { get; set; }

        [BsonElement("destructible")]
        public bool Destructible { get; set; }

        [BsonElement("interactive")]
        public bool Interactive { get; set; }
    }

    /// <summary>
    /// Requirements to interact with an obstacle.
    /// </summary>
    public class ObstacleRequirements
    {
        [BsonElement("itemRequired")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? ItemRequired { get; set; }

        [BsonElement("skillRequired")]
        public string SkillRequired { get; set; }

        [BsonElement("levelRequired")]
        public int? LevelRequired { get; set; }
    }

    /// <summary>
    /// Effects of interacting with an obstacle.
    /// </summary>
    public class ObstacleEffects
    {
        [BsonElement("damageOnInteraction")]
        public int? DamageOnInteraction { get; set; }

        [BsonElement("triggersEvent")]
        public string TriggersEvent { get; set; }
    }

    /// <summary>
    /// Represents a game asset (image, audio, etc.).
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Asset
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("filename")]
        public string Filename { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } // "image", "audio", "video", etc.

        [BsonElement("format")]
        public string Format { get; set; } // "png", "gif", "mp3", etc.

        [BsonElement("path")]
        public string Path { get; set; }

        [BsonElement("usedBy")]
        public List<AssetUsage> UsedBy { get; set; } = new List<AssetUsage>();

        [BsonElement("fileSize")]
        public long? FileSize { get; set; }

        [BsonElement("uploadDate")]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        [BsonElement("lastModified")]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Tracks where an asset is used.
    /// </summary>
    public class AssetUsage
    {
        [BsonElement("entityType")]
        public string EntityType { get; set; } // "creature", "item", "npc", "ui", etc.

        [BsonElement("entityId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? EntityId { get; set; }

        [BsonElement("entityName")]
        public string EntityName { get; set; }
    }
}
