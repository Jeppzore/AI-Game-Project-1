using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

[BsonIgnoreExtraElements]
public class NPC
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("role")]
    public string Role { get; set; } = string.Empty; // merchant, quest-giver, trainer, vendor

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [BsonElement("location")]
    public NPCLocation Location { get; set; } = new();

    [BsonElement("trades")]
    public NPCTrades Trades { get; set; } = new();

    [BsonElement("quests")]
    public List<NPCQuest> Quests { get; set; } = new();

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class NPCLocation
{
    [BsonElement("area")]
    public string Area { get; set; } = string.Empty;

    [BsonElement("coordinates")]
    public NPCCoordinates Coordinates { get; set; } = new();

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;
}

public class NPCCoordinates
{
    [BsonElement("x")]
    public int X { get; set; }

    [BsonElement("y")]
    public int Y { get; set; }
}

public class NPCTrades
{
    [BsonElement("buys")]
    public List<TradeItem> Buys { get; set; } = new();

    [BsonElement("sells")]
    public List<TradeItem> Sells { get; set; } = new();
}

public class TradeItem
{
    [BsonElement("itemId")]
    public string ItemId { get; set; } = string.Empty;

    [BsonElement("itemName")]
    public string ItemName { get; set; } = string.Empty;

    [BsonElement("price")]
    public int Price { get; set; }
}

public class NPCQuest
{
    [BsonElement("questId")]
    public string QuestId { get; set; } = string.Empty;

    [BsonElement("questName")]
    public string QuestName { get; set; } = string.Empty;
}
