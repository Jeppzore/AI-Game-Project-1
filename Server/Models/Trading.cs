using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

/// <summary>
/// Represents a shop inventory for NPC vendors or players.
/// </summary>
[BsonIgnoreExtraElements]
public class ShopInventory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("npc_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? NpcId { get; set; }

    [BsonElement("npc_name")]
    public string NpcName { get; set; } = string.Empty;

    [BsonElement("items")]
    public List<ShopItem> Items { get; set; } = new();

    [BsonElement("is_player_shop")]
    public bool IsPlayerShop { get; set; }

    [BsonElement("last_restocked")]
    public DateTime LastRestocked { get; set; } = DateTime.UtcNow;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents an item in a shop's inventory.
/// </summary>
public class ShopItem
{
    [BsonElement("itemId")]
    public string ItemId { get; set; } = string.Empty;

    [BsonElement("itemName")]
    public string ItemName { get; set; } = string.Empty;

    [BsonElement("price")]
    public int Price { get; set; }

    [BsonElement("stock_quantity")]
    public int StockQuantity { get; set; }

    [BsonElement("restock_rate")]
    public double RestockRate { get; set; } = 1.0;
}

/// <summary>
/// Represents a player's inventory containing items and gold.
/// </summary>
[BsonIgnoreExtraElements]
public class PlayerInventory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("player_id")]
    public string PlayerId { get; set; } = string.Empty;

    [BsonElement("items")]
    public List<InventoryItem> Items { get; set; } = new();

    [BsonElement("gold")]
    public int Gold { get; set; }

    [BsonElement("level")]
    public int Level { get; set; } = 1;

    [BsonElement("experience")]
    public int Experience { get; set; }

    [BsonElement("last_updated")]
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents an item in a player's inventory.
/// </summary>
public class InventoryItem
{
    [BsonElement("itemId")]
    public string ItemId { get; set; } = string.Empty;

    [BsonElement("itemName")]
    public string ItemName { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public int Quantity { get; set; }
}

/// <summary>
/// Represents a transaction record for audit trail.
/// </summary>
[BsonIgnoreExtraElements]
public class Transaction
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("buyer_id")]
    public string BuyerId { get; set; } = string.Empty;

    [BsonElement("seller_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? SellerId { get; set; }

    [BsonElement("seller_name")]
    public string SellerName { get; set; } = string.Empty;

    [BsonElement("item_id")]
    public string ItemId { get; set; } = string.Empty;

    [BsonElement("item_name")]
    public string ItemName { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public int Quantity { get; set; }

    [BsonElement("unit_price")]
    public int UnitPrice { get; set; }

    [BsonElement("total_price")]
    public int TotalPrice { get; set; }

    [BsonElement("transaction_type")]
    public string TransactionType { get; set; } = string.Empty; // "purchase" or "sale"

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents a trade offer between buyer and seller.
/// </summary>
public class TradeOffer
{
    [BsonElement("buyer_id")]
    public string BuyerId { get; set; } = string.Empty;

    [BsonElement("seller_id")]
    public string SellerId { get; set; } = string.Empty;

    [BsonElement("item_id")]
    public string ItemId { get; set; } = string.Empty;

    [BsonElement("item_name")]
    public string ItemName { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public int Quantity { get; set; }

    [BsonElement("price")]
    public int Price { get; set; }

    [BsonElement("total_cost")]
    public int TotalCost { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
