using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

[BsonIgnoreExtraElements]
public class Item
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty; // weapon, armor, tool, consumable, quest

    [BsonElement("category")]
    public string Category { get; set; } = string.Empty; // helmets, chests, weapons, etc.

    [BsonElement("rarity")]
    public string Rarity { get; set; } = string.Empty; // common, uncommon, rare, epic, legendary

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [BsonElement("stats")]
    public ItemStats Stats { get; set; } = new();

    [BsonElement("requirements")]
    public ItemRequirements Requirements { get; set; } = new();

    [BsonElement("acquisition")]
    public ItemAcquisition Acquisition { get; set; } = new();

    [BsonElement("usedBy")]
    public List<string> UsedBy { get; set; } = new(); // creature IDs or names

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class ItemStats
{
    [BsonElement("damage")]
    public int? Damage { get; set; }

    [BsonElement("defense")]
    public int? Defense { get; set; }

    [BsonElement("speed")]
    public int? Speed { get; set; }

    [BsonElement("specialAttributes")]
    public List<string> SpecialAttributes { get; set; } = new();
}

public class ItemRequirements
{
    [BsonElement("minLevel")]
    public int? MinLevel { get; set; }

    [BsonElement("minStr")]
    public int? MinStr { get; set; }

    [BsonElement("minDex")]
    public int? MinDex { get; set; }
}

public class ItemAcquisition
{
    [BsonElement("methods")]
    public List<string> Methods { get; set; } = new(); // drop, quest, purchase, craft, etc.

    [BsonElement("sources")]
    public List<string> Sources { get; set; } = new(); // creature/NPC names
}
