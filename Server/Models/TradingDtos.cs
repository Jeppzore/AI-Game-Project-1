using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Models;

/// <summary>
/// Request to purchase an item from an NPC shop.
/// </summary>
public class BuyItemRequest
{
    public string NpcId { get; set; } = string.Empty;
    public string ItemId { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
}

/// <summary>
/// Request to sell an item to an NPC.
/// </summary>
public class SellItemRequest
{
    public string NpcId { get; set; } = string.Empty;
    public string ItemId { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
}

/// <summary>
/// Response containing transaction details.
/// </summary>
public class TransactionResponse
{
    public string? Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
    public int TotalPrice { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Response containing NPC with shop details.
/// </summary>
public class NpcShopResponse
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public NPCLocation? Location { get; set; }
    public ShopInventoryResponse? Shop { get; set; }
}

/// <summary>
/// Response containing shop inventory details.
/// </summary>
public class ShopInventoryResponse
{
    public List<ShopItemResponse> Items { get; set; } = new();
    public int TotalItems { get; set; }
}

/// <summary>
/// Response containing individual shop item details.
/// </summary>
public class ShopItemResponse
{
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int Price { get; set; }
    public int StockQuantity { get; set; }
}

/// <summary>
/// Response containing player inventory details.
/// </summary>
public class PlayerInventoryResponse
{
    public string PlayerId { get; set; } = string.Empty;
    public int Gold { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public List<InventoryItemResponse> Items { get; set; } = new();
}

/// <summary>
/// Response containing individual inventory item details.
/// </summary>
public class InventoryItemResponse
{
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
