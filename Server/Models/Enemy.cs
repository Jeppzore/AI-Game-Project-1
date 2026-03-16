using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

[BsonIgnoreExtraElements]
public class Enemy
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("health")]
    public int Health { get; set; }

    [BsonElement("attack")]
    public int Attack { get; set; }

    [BsonElement("defense")]
    public int Defense { get; set; }

    [BsonElement("experience")]
    public int Experience { get; set; }

    [BsonElement("drops")]
    public List<LootDrop> Drops { get; set; } = new();

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class LootDrop
{
    [BsonElement("itemName")]
    public string ItemName { get; set; } = string.Empty;

    [BsonElement("dropRate")]
    public decimal DropRate { get; set; }

    [BsonElement("quantity")]
    public int Quantity { get; set; } = 1;
}
