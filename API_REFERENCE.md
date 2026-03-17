# Phase 2 Backend API - Complete Endpoint Reference

## Base URL
```
http://localhost:5000/api
```

## Response Format
All responses follow this format:
```json
{
  "data": { /* actual data */ },
  "error": null
}
```

---

## 1. CREATURES ENDPOINTS

### List All Creatures (with pagination, filtering, sorting)
```
GET /api/creatures?page=1&pageSize=20&sort=name&sortOrder=asc&type=boss&location=forest&minHealth=10&maxHealth=100&search=slime
```
**Query Parameters:**
- `page` (int, default: 1) - Page number
- `pageSize` (int, default: 20, max: 100) - Items per page
- `sort` (string, default: "name") - Field to sort by
- `sortOrder` (string, default: "asc") - "asc" or "desc"
- `type` (string, optional) - Filter by type (normal, elite, boss, miniboss, unique)
- `location` (string, optional) - Filter by location
- `minHealth` (int, optional) - Minimum health value
- `maxHealth` (int, optional) - Maximum health value
- `search` (string, optional) - Full-text search

**Response:**
```json
{
  "data": {
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 45,
    "items": [
      {
        "id": "507f1f77bcf86cd799439011",
        "name": "Gaklorr",
        "type": "boss",
        "description": "A fearsome boss creature...",
        "imageUrl": "https://example.com/gaklorr.png",
        "health": 500,
        "attack": 120,
        "defense": 80,
        "experience": 5000,
        "location": "Dark Forest",
        "abilities": ["Fire Attack", "Stomp"],
        "weaknesses": ["Water", "Ice"],
        "drops": [
          {
            "itemName": "Gaklorr's Fang",
            "dropRate": 0.5,
            "quantity": 1
          }
        ],
        "createdAt": "2024-01-15T10:30:00Z",
        "updatedAt": "2024-01-15T10:30:00Z"
      }
    ]
  }
}
```

### Get Single Creature
```
GET /api/creatures/{id}
```
**Response:** Single creature object (same structure as above)

### Get Creatures by Location
```
GET /api/creatures/by-location/{location}
```
**Response:** Array of creature objects

### Get Creature's Loot Drops
```
GET /api/creatures/{id}/loot
```
**Response:**
```json
{
  "data": [
    {
      "itemName": "Dragon Scale",
      "dropRate": 0.75,
      "quantity": 2
    },
    {
      "itemName": "Gold Coin",
      "dropRate": 1.0,
      "quantity": 100
    }
  ]
}
```

### Create Creature
```
POST /api/creatures
Content-Type: application/json

{
  "name": "Fire Elemental",
  "type": "elite",
  "description": "An elemental creature made of pure fire",
  "imageUrl": "https://example.com/fire-elemental.png",
  "health": 150,
  "attack": 90,
  "defense": 40,
  "experience": 800,
  "location": "Volcano",
  "abilities": ["Fire Burst", "Ignite"],
  "weaknesses": ["Water"],
  "drops": [
    {
      "itemName": "Fire Core",
      "dropRate": 0.3,
      "quantity": 1
    }
  ]
}
```

### Update Creature
```
PUT /api/creatures/{id}
Content-Type: application/json

{
  "name": "Updated Name",
  "health": 200,
  "abilities": ["Updated Ability"]
}
```
(All fields optional - only provided fields are updated)

### Delete Creature
```
DELETE /api/creatures/{id}
```
**Response:** `{ "data": { "message": "Creature deleted successfully" } }`

---

## 2. ITEMS ENDPOINTS

### List All Items (with pagination, filtering, sorting)
```
GET /api/items?page=1&pageSize=20&sort=name&sortOrder=asc&type=weapon&category=swords&rarity=legendary&minDamage=50&maxDamage=150&search=flame
```
**Query Parameters:**
- `page` (int, default: 1)
- `pageSize` (int, default: 20, max: 100)
- `sort` (string, default: "name")
- `sortOrder` (string, default: "asc")
- `type` (string, optional) - weapon, armor, tool, consumable, quest, accessory
- `category` (string, optional) - helmets, chests, legs, swords, axes, etc.
- `rarity` (string, optional) - common, uncommon, rare, epic, legendary
- `minDamage` (int, optional)
- `maxDamage` (int, optional)
- `search` (string, optional)

**Response:**
```json
{
  "data": {
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 150,
    "items": [
      {
        "id": "507f1f77bcf86cd799439012",
        "name": "Flaming Sword",
        "type": "weapon",
        "category": "swords",
        "rarity": "epic",
        "description": "A legendary sword wreathed in eternal flames",
        "imageUrl": "https://example.com/flaming-sword.png",
        "stats": {
          "damage": 120,
          "defense": 0,
          "speed": 5,
          "specialAttributes": ["Fire Damage +50", "Burn Effect 20%"]
        },
        "requirements": {
          "minLevel": 50,
          "minStr": 75,
          "minDex": null
        },
        "acquisition": {
          "methods": ["drop", "quest"],
          "sources": ["Dragon Boss", "Fire Temple Quest"]
        },
        "usedBy": ["Gaklorr", "Fire Knight"],
        "createdAt": "2024-01-15T10:30:00Z",
        "updatedAt": "2024-01-15T10:30:00Z"
      }
    ]
  }
}
```

### Get Single Item
```
GET /api/items/{id}
```

### Get Items by Type
```
GET /api/items/by-type/{type}
```
**Response:** Array of item objects

### Get Creatures That Drop Item
```
GET /api/items/{id}/dropped-by
```
**Response:** Array of creature objects that drop this item

### Get Creatures That Use Item
```
GET /api/items/{id}/used-by
```
**Response:** Array of creatures that have this item in UsedBy list

### Create Item
```
POST /api/items
Content-Type: application/json

{
  "name": "Iron Helmet",
  "type": "armor",
  "category": "helmets",
  "rarity": "common",
  "description": "A basic iron helmet for protection",
  "imageUrl": "https://example.com/iron-helmet.png",
  "stats": {
    "damage": null,
    "defense": 30,
    "speed": -2,
    "specialAttributes": []
  },
  "requirements": {
    "minLevel": 5,
    "minStr": 10,
    "minDex": null
  },
  "acquisition": {
    "methods": ["purchase", "drop"],
    "sources": ["Blacksmith", "Goblins"]
  },
  "usedBy": []
}
```

### Update Item
```
PUT /api/items/{id}
```

### Delete Item
```
DELETE /api/items/{id}
```

---

## 3. NPCs ENDPOINTS

### List All NPCs (with pagination, filtering)
```
GET /api/npcs?page=1&pageSize=20&sort=name&sortOrder=asc&role=merchant&location=village&search=smith
```
**Query Parameters:**
- `page`, `pageSize`, `sort`, `sortOrder`
- `role` (string, optional) - merchant, quest-giver, trainer, vendor, story, guide
- `location` (string, optional) - Filter by location area
- `search` (string, optional)

**Response:**
```json
{
  "data": {
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 25,
    "items": [
      {
        "id": "507f1f77bcf86cd799439013",
        "name": "Old Blacksmith",
        "role": "merchant",
        "description": "A seasoned blacksmith who buys and sells weapons and armor",
        "imageUrl": "https://example.com/blacksmith.png",
        "location": {
          "area": "Village Square",
          "coordinates": {
            "x": 150,
            "y": 200
          },
          "description": "Near the town entrance"
        },
        "trades": {
          "buys": [
            {
              "itemId": "507f1f77bcf86cd799439012",
              "itemName": "Iron Ore",
              "price": 50
            }
          ],
          "sells": [
            {
              "itemId": "507f1f77bcf86cd799439014",
              "itemName": "Iron Sword",
              "price": 200
            }
          ]
        },
        "quests": [
          {
            "questId": "q1",
            "questName": "Bring Iron Ore"
          }
        ],
        "createdAt": "2024-01-15T10:30:00Z",
        "updatedAt": "2024-01-15T10:30:00Z"
      }
    ]
  }
}
```

### Get Single NPC
```
GET /api/npcs/{id}
```

### Get NPC's Trade Lists
```
GET /api/npcs/{id}/trades
```
**Response:**
```json
{
  "data": {
    "buys": [ /* array of TradeItem */ ],
    "sells": [ /* array of TradeItem */ ]
  }
}
```

### Create NPC
```
POST /api/npcs
```

### Update NPC
```
PUT /api/npcs/{id}
```

### Delete NPC
```
DELETE /api/npcs/{id}
```

---

## 4. OBSTACLES ENDPOINTS

### List All Obstacles
```
GET /api/obstacles?page=1&pageSize=20&type=trap&difficulty=hard&location=dungeon&search=spike
```
**Query Parameters:**
- `page`, `pageSize`, `sort`, `sortOrder`
- `type` (string, optional) - trap, wall, platform, puzzle, container, node, vegetation, barrier
- `difficulty` (string, optional) - easy, medium, hard
- `location` (string, optional)
- `search` (string, optional)

**Response:**
```json
{
  "data": {
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 13,
    "items": [
      {
        "id": "507f1f77bcf86cd799439015",
        "name": "Spike Pit",
        "type": "trap",
        "difficulty": "hard",
        "description": "A deadly pit filled with sharp spikes",
        "imageUrl": "https://example.com/spike-pit.png",
        "location": "Dark Dungeon",
        "solveMethod": "Jump over with high agility or use levitation spell",
        "createdAt": "2024-01-15T10:30:00Z",
        "updatedAt": "2024-01-15T10:30:00Z"
      }
    ]
  }
}
```

### Get Single Obstacle
```
GET /api/obstacles/{id}
```

### Create Obstacle
```
POST /api/obstacles
```

### Update Obstacle
```
PUT /api/obstacles/{id}
```

### Delete Obstacle
```
DELETE /api/obstacles/{id}
```

---

## 5. SEARCH ENDPOINTS

### Global Search
```
GET /api/search?q=fire&entityTypes=creatures,items&page=1&pageSize=20
```
**Query Parameters:**
- `q` (string, required) - Search term
- `entityTypes` (string, optional) - Comma-separated (creatures, items, npcs, obstacles). Default: all
- `page` (int, default: 1)
- `pageSize` (int, default: 20)

**Response:**
```json
{
  "data": {
    "pageNumber": 1,
    "pageSize": 20,
    "creatures": [ /* array of creatures */ ],
    "creaturesTotalCount": 5,
    "items": [ /* array of items */ ],
    "itemsTotalCount": 12,
    "npcs": [ /* array of NPCs */ ],
    "npcsTotalCount": 0,
    "obstacles": [ /* array of obstacles */ ],
    "obstaclesTotalCount": 3
  }
}
```

### Autocomplete Suggestions
```
GET /api/search/autocomplete?q=sla&limit=10
```
**Query Parameters:**
- `q` (string, required)
- `limit` (int, default: 10, max: 50)

**Response:**
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439011",
      "text": "Green Slime",
      "type": "creature",
      "imageUrl": "https://example.com/green-slime.png"
    },
    {
      "id": "507f1f77bcf86cd799439016",
      "text": "Slime Helmet",
      "type": "item",
      "imageUrl": "https://example.com/slime-helmet.png"
    }
  ]
}
```

---

## 6. CATEGORIES ENDPOINTS

### Get Creature Types
```
GET /api/categories/creature-types
```
**Response:** `["normal", "elite", "boss", "miniboss", "unique"]`

### Get Creature Difficulties
```
GET /api/categories/creature-difficulties
```
**Response:** `["easy", "medium", "hard", "expert", "nightmare"]`

### Get Item Types
```
GET /api/categories/item-types
```
**Response:** `["weapon", "armor", "tool", "consumable", "quest", "accessory"]`

### Get Item Categories
```
GET /api/categories/item-categories
```
**Response:** `["helmets", "chests", "legs", "feet", "hands", "back", "swords", "axes", "hammers", "maces", "spears", "bows", "crossbows", "rings", "amulets", "potions", "gems", "valuables", "tools", "miscellaneous"]`

### Get Item Rarities
```
GET /api/categories/item-rarities
```
**Response:** `["common", "uncommon", "rare", "epic", "legendary"]`

### Get NPC Roles
```
GET /api/categories/npc-roles
```
**Response:** `["merchant", "quest-giver", "trainer", "vendor", "story", "guide"]`

### Get Obstacle Types
```
GET /api/categories/obstacle-types
```
**Response:** `["trap", "wall", "platform", "puzzle", "container", "node", "vegetation", "barrier"]`

### Get Obstacle Difficulties
```
GET /api/categories/obstacle-difficulties
```
**Response:** `["easy", "medium", "hard"]`

---

## HTTP Status Codes

| Code | Meaning | Scenario |
|------|---------|----------|
| 200 | OK | Successful GET, PUT, or generic response |
| 201 | Created | Successful POST with new resource |
| 400 | Bad Request | Invalid input or missing required fields |
| 404 | Not Found | Resource doesn't exist |
| 500 | Internal Server Error | Server exception |

---

## Error Response Format

```json
{
  "data": null,
  "error": "Creature not found"
}
```

---

## Data Types Reference

### Creature
```json
{
  "id": "ObjectId",
  "name": "string",
  "type": "string (enum)",
  "description": "string",
  "imageUrl": "string",
  "health": "int",
  "attack": "int",
  "defense": "int",
  "experience": "int",
  "location": "string",
  "abilities": "string[]",
  "weaknesses": "string[]",
  "drops": "LootDrop[]",
  "createdAt": "ISO 8601 datetime",
  "updatedAt": "ISO 8601 datetime"
}
```

### Item
```json
{
  "id": "ObjectId",
  "name": "string",
  "type": "string (enum)",
  "category": "string",
  "rarity": "string",
  "description": "string",
  "imageUrl": "string",
  "stats": {
    "damage": "int?",
    "defense": "int?",
    "speed": "int?",
    "specialAttributes": "string[]"
  },
  "requirements": {
    "minLevel": "int?",
    "minStr": "int?",
    "minDex": "int?"
  },
  "acquisition": {
    "methods": "string[]",
    "sources": "string[]"
  },
  "usedBy": "string[]",
  "createdAt": "ISO 8601 datetime",
  "updatedAt": "ISO 8601 datetime"
}
```

### NPC
```json
{
  "id": "ObjectId",
  "name": "string",
  "role": "string",
  "description": "string",
  "imageUrl": "string",
  "location": {
    "area": "string",
    "coordinates": {
      "x": "int",
      "y": "int"
    },
    "description": "string"
  },
  "trades": {
    "buys": "TradeItem[]",
    "sells": "TradeItem[]"
  },
  "quests": "NPCQuest[]",
  "createdAt": "ISO 8601 datetime",
  "updatedAt": "ISO 8601 datetime"
}
```

### Obstacle
```json
{
  "id": "ObjectId",
  "name": "string",
  "type": "string",
  "difficulty": "string",
  "description": "string",
  "imageUrl": "string",
  "location": "string",
  "solveMethod": "string",
  "createdAt": "ISO 8601 datetime",
  "updatedAt": "ISO 8601 datetime"
}
```

---

## Example Requests

### Get all legendary weapons
```
GET /api/items?type=weapon&rarity=legendary&pageSize=50
```

### Find all creatures in a location
```
GET /api/creatures/by-location/Dark Forest
```

### Get what drops from a creature
```
GET /api/creatures/507f1f77bcf86cd799439011/loot
```

### Get creatures that use an item
```
GET /api/items/507f1f77bcf86cd799439014/used-by
```

### Search for "slime" across all entities
```
GET /api/search?q=slime
```

### Create a new creature
```
POST /api/creatures
Content-Type: application/json

{
  "name": "Ice Golem",
  "type": "elite",
  "description": "A magical construct made of ice",
  "health": 250,
  "attack": 100,
  "defense": 120,
  "experience": 1500,
  "location": "Frozen Peak",
  "abilities": ["Ice Blast", "Freeze"],
  "weaknesses": ["Fire", "Lightning"],
  "drops": []
}
```

---

## Database Connection

**Default MongoDB Connection:**
- URL: `mongodb://localhost:27017`
- Database: `nightmares-wiki`
- Collections: `creatures`, `items`, `npcs`, `obstacles`, `enemies`

Configure in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  },
  "MongoDB": {
    "DatabaseName": "nightmares-wiki"
  }
}
```

---

**API Documentation Complete** - Ready for frontend integration!
