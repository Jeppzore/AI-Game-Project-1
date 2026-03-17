# Trading System MongoDB Schema

## Collections Overview

### NPCs Collection
Extends existing NPC model with trading capabilities.

```json
{
  "_id": ObjectId("..."),
  "id": "npc_001",
  "name": "Blacksmith Thrall",
  "role": "vendor",
  "location": {
    "area": "Crimson Keep",
    "coordinates": { "x": 100, "y": 200 },
    "description": "Main fortress blacksmith"
  },
  "shop_id": ObjectId("..."),
  "description": "An expert blacksmith",
  "trades": {
    "buys": [
      { "itemId": "item_001", "itemName": "Iron Ore", "price": 50 },
      { "itemId": "item_002", "itemName": "Steel Bar", "price": 100 }
    ],
    "sells": [
      { "itemId": "item_003", "itemName": "Iron Sword", "price": 200 },
      { "itemId": "item_004", "itemName": "Steel Plate Armor", "price": 500 }
    ]
  },
  "createdAt": ISODate("2024-01-01T00:00:00Z"),
  "updatedAt": ISODate("2024-01-01T00:00:00Z")
}
```

### ShopInventory Collection
Manages shop stock and pricing for each NPC vendor.

```json
{
  "_id": ObjectId("..."),
  "npc_id": ObjectId("..."),
  "npc_name": "Blacksmith Thrall",
  "items": [
    {
      "itemId": "item_003",
      "itemName": "Iron Sword",
      "price": 200,
      "stock_quantity": 5,
      "restock_rate": 1
    },
    {
      "itemId": "item_004",
      "itemName": "Steel Plate Armor",
      "price": 500,
      "stock_quantity": 2,
      "restock_rate": 0.5
    }
  ],
  "is_player_shop": false,
  "last_restocked": ISODate("2024-01-15T12:00:00Z"),
  "createdAt": ISODate("2024-01-01T00:00:00Z"),
  "updatedAt": ISODate("2024-01-15T12:00:00Z")
}
```

### PlayerInventory Collection
Tracks player items and gold.

```json
{
  "_id": ObjectId("..."),
  "player_id": "player_001",
  "items": [
    {
      "itemId": "item_005",
      "itemName": "Health Potion",
      "quantity": 10
    },
    {
      "itemId": "item_001",
      "itemName": "Iron Ore",
      "quantity": 25
    }
  ],
  "gold": 1500,
  "level": 1,
  "experience": 0,
  "last_updated": ISODate("2024-01-15T14:30:00Z"),
  "createdAt": ISODate("2024-01-01T00:00:00Z")
}
```

### Transactions Collection
Audit log for all trades.

```json
{
  "_id": ObjectId("..."),
  "buyer_id": "player_001",
  "seller_id": ObjectId("npc_001_object_id"),
  "seller_name": "Blacksmith Thrall",
  "item_id": "item_003",
  "item_name": "Iron Sword",
  "quantity": 1,
  "unit_price": 200,
  "total_price": 200,
  "transaction_type": "purchase",
  "timestamp": ISODate("2024-01-15T14:30:00Z")
}
```

## Example Data

### 3 NPC Vendors with Shops

**NPC 1: Blacksmith Thrall**
- Role: vendor (weapons/armor)
- Location: Crimson Keep
- Buys: Iron Ore (50g), Steel Bar (100g)
- Sells: Iron Sword (200g), Steel Plate Armor (500g), Wooden Shield (150g)

**NPC 2: Potion Merchant**
- Role: vendor (potions/consumables)
- Location: Shadow Market
- Buys: Herbs (25g), Monster Parts (75g)
- Sells: Health Potion (100g), Mana Potion (150g), Antidote (200g)

**NPC 3: Treasure Appraiser**
- Role: vendor (specialty items)
- Location: Grand Library
- Buys: Rare Gems (500g), Ancient Artifacts (1000g)
- Sells: Enchanted Ring (750g), Philosopher's Stone (2000g)

## Indexes

```
npcs: { name: 1, role: 1, location.area: 1 }
shop_inventory: { npc_id: 1, items.itemId: 1 }
player_inventory: { player_id: 1 }
transactions: { buyer_id: 1, timestamp: -1 }
```
