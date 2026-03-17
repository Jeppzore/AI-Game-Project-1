using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

[BsonIgnoreExtraElements]
public class Creature
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty; // normal, elite, boss, miniboss, unique

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [BsonElement("health")]
    public int Health { get; set; }

    [BsonElement("attack")]
    public int Attack { get; set; }

    [BsonElement("defense")]
    public int Defense { get; set; }

    [BsonElement("experience")]
    public int Experience { get; set; }

    [BsonElement("location")]
    public string Location { get; set; } = string.Empty;

    [BsonElement("abilities")]
    public List<string> Abilities { get; set; } = new();

    [BsonElement("weaknesses")]
    public List<string> Weaknesses { get; set; } = new();

    [BsonElement("drops")]
    public List<LootDrop> Drops { get; set; } = new();

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
