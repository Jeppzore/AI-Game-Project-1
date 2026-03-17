# Phase 2: Preliminary API Endpoint Planning
**Status**: Preliminary Planning (Awaiting data audit results)  
**Last Updated**: 2026-03-17

---

## Overview

This document defines all API endpoints needed for the complete Nightmares Wiki implementation. It builds on the existing Enemy/Enemies pattern and extends it to cover Creatures, Items, NPCs, and Obstacles with advanced filtering, search, and relationship queries.

## API Architecture Decisions

### Base URL
```
http://localhost:5000/api (development)
https://api.nightmareswiki.com/api (production)
```

### Response Format (Consistent with existing pattern)
```json
{
  "data": { /* actual data */ },
  "error": null
}
```

### HTTP Status Codes
- **200 OK** - Successful GET, PUT
- **201 Created** - Successful POST
- **204 No Content** - Successful DELETE
- **400 Bad Request** - Invalid parameters
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server error

### Pagination Strategy
- **Query Parameters**: `?page=1&pageSize=20`
- **Response**: Include `pageNumber`, `pageSize`, `totalCount`, `items` array
- **Default**: page=1, pageSize=20
- **Max**: pageSize=100

### Sorting
- **Query Parameters**: `?sort=name&sortOrder=asc|desc`
- **Default**: By relevance for search, by creation date for lists

### Filtering
- Entity-specific filters in query parameters
- Examples: `?type=boss&difficulty=hard&minHealth=100`

---

## Endpoint Groups

### 1. CREATURES ENDPOINTS

#### GET /creatures
List all creatures with optional filtering.

**Query Parameters:**
```
?page=1
&pageSize=20
&sort=name|health|attack|experience
&sortOrder=asc|desc
&type=normal|elite|boss
&difficulty=easy|medium|hard|extreme
&minHealth=100
&maxHealth=1000
&minAttack=10
&search=dragon
```

**Response:**
```json
{
  "data": {
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 150,
    "items": [
      {
        "id": "507f1f77bcf86cd799439011",
        "name": "Gaklorr",
        "type": "boss",
        "description": "The Ironfist Tyrant...",
        "health": 500,
        "attack": 80,
        "defense": 60,
        "experience": 5000,
        "imageUrl": "/images/creatures/gaklorr.gif",
        "drops": [
          {
            "itemId": "507f1f77bcf86cd799439012",
            "itemName": "Legendary Sword",
            "dropRate": 0.15,
            "quantity": 1,
            "rarity": "legendary"
          }
        ],
        "location": "Bloodcrag Highlands",
        "abilities": ["Smash", "Ground Slam"],
        "createdAt": "2026-03-15T10:00:00Z",
        "updatedAt": "2026-03-15T10:00:00Z"
      }
    ]
  },
  "error": null
}
```

**Status**: 200 OK

---

#### GET /creatures/{id}
Get a specific creature by ID.

**Parameters:**
- `id` (path, required): MongoDB ObjectId

**Response:**
```json
{
  "data": {
    "id": "507f1f77bcf86cd799439011",
    "name": "Gaklorr",
    "type": "boss",
    "description": "...",
    "health": 500,
    "attack": 80,
    "defense": 60,
    "experience": 5000,
    "imageUrl": "/images/creatures/gaklorr.gif",
    "drops": [...],
    "location": "Bloodcrag Highlands",
    "abilities": ["Smash", "Ground Slam"],
    "weaknesses": ["Water", "Cold"],
    "relatedCreatures": [
      {
        "id": "507f1f77bcf86cd799439013",
        "name": "Gaklorr's Minion",
        "type": "elite"
      }
    ]
  },
  "error": null
}
```

**Status**: 200 OK or 404 Not Found

---

#### POST /creatures
Create a new creature.

**Request Body:**
```json
{
  "name": "New Boss",
  "type": "boss",
  "description": "A powerful creature...",
  "health": 500,
  "attack": 80,
  "defense": 60,
  "experience": 5000,
  "imageUrl": "/images/creatures/newboss.gif",
  "location": "Dark Forest",
  "abilities": ["Attack 1", "Attack 2"],
  "weaknesses": ["Fire"],
  "drops": [
    {
      "itemId": "507f1f77bcf86cd799439012",
      "itemName": "Magic Staff",
      "dropRate": 0.25,
      "quantity": 1,
      "rarity": "rare"
    }
  ]
}
```

**Response**: 201 Created + created creature data

---

#### PUT /creatures/{id}
Update a creature.

**Parameters:**
- `id` (path, required): MongoDB ObjectId

**Request Body**: All fields optional (patch-like behavior)
```json
{
  "name": "Updated Name",
  "health": 600,
  "drops": [...]
}
```

**Response**: 200 OK + updated creature data

---

#### DELETE /creatures/{id}
Delete a creature.

**Response**: 200 OK with message

---

### 2. ITEMS ENDPOINTS

#### GET /items
List all items with optional filtering.

**Query Parameters:**
```
?page=1
&pageSize=20
&sort=name|type|rarity|damage|defense
&sortOrder=asc|desc
&type=weapon|armor|tool|consumable|quest
&category=helmets|chests|legs|boots|weapons|shields|tools|other
&rarity=common|uncommon|rare|epic|legendary
&minDamage=10
&maxDamage=100
&minDefense=5
&maxDefense=50
&search=sword
```

**Response:**
```json
{
  "data": {
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 500,
    "items": [
      {
        "id": "507f1f77bcf86cd799439012",
        "name": "Legendary Sword",
        "type": "weapon",
        "category": "weapons",
        "rarity": "legendary",
        "description": "A powerful sword...",
        "imageUrl": "/images/items/legendary-sword.png",
        "stats": {
          "damage": 85,
          "defense": 0,
          "speed": 1.0,
          "specialAttributes": ["Fire Damage +20", "Lifesteal +5%"]
        },
        "requirements": {
          "minLevel": 50,
          "minStr": 40,
          "minDex": 20
        },
        "acquisition": {
          "methods": ["boss_drop", "npc_trade", "quest_reward"],
          "sources": [
            {
              "sourceType": "creature",
              "sourceName": "Gaklorr",
              "sourceId": "507f1f77bcf86cd799439011",
              "dropRate": 0.15
            }
          ]
        },
        "usedBy": [
          {
            "creatureId": "507f1f77bcf86cd799439050",
            "creatureName": "Fire Knight"
          }
        ]
      }
    ]
  },
  "error": null
}
```

**Status**: 200 OK

---

#### GET /items/{id}
Get a specific item by ID.

**Response**: 200 OK + full item details including all stats, requirements, acquisition methods, and where it's used

---

#### GET /items/by-type/{type}
Get items by type (weapon, armor, tool, consumable).

**Query Parameters**: All standard filtering parameters plus:
```
?category=specific-category
&rarity=legendary
```

**Response**: Paginated list of items of that type

---

#### GET /items/search
Global item search with full-text capabilities.

**Query Parameters:**
```
?q=sword
&type=weapon
&rarity=rare
&minDamage=50
&page=1
```

**Response**: Search results with relevance ranking

---

#### POST /items
Create a new item.

#### PUT /items/{id}
Update an item.

#### DELETE /items/{id}
Delete an item.

---

### 3. NPCs ENDPOINTS

#### GET /npcs
List all NPCs with optional filtering.

**Query Parameters:**
```
?page=1
&pageSize=20
&sort=name|location
&sortOrder=asc|desc
&role=merchant|quest-giver|trainer|vendor
&location=city|dungeon|forest
&search=blacksmith
```

**Response:**
```json
{
  "data": {
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 80,
    "items": [
      {
        "id": "507f1f77bcf86cd799439031",
        "name": "Thorin Blacksmith",
        "role": "merchant",
        "description": "A master blacksmith...",
        "imageUrl": "/images/npcs/thorin.png",
        "location": {
          "area": "Iron City",
          "coordinates": {
            "x": 100,
            "y": 200
          },
          "description": "In the blacksmith forge"
        },
        "trades": {
          "buys": [
            {
              "itemId": "507f1f77bcf86cd799439032",
              "itemName": "Iron Ore",
              "price": 50
            }
          ],
          "sells": [
            {
              "itemId": "507f1f77bcf86cd799439012",
              "itemName": "Legendary Sword",
              "price": 5000
            }
          ]
        },
        "quests": [
          {
            "questId": "507f1f77bcf86cd799439040",
            "questName": "Find the Lost Ore"
          }
        ]
      }
    ]
  },
  "error": null
}
```

**Status**: 200 OK

---

#### GET /npcs/{id}
Get a specific NPC by ID.

**Response**: 200 OK + full NPC details including all trades and quests

---

#### POST /npcs
Create a new NPC.

#### PUT /npcs/{id}
Update an NPC.

#### DELETE /npcs/{id}
Delete an NPC.

---

### 4. OBSTACLES ENDPOINTS

#### GET /obstacles
List all obstacles with optional filtering.

**Query Parameters:**
```
?page=1
&pageSize=20
&type=trap|wall|platform|puzzle
&difficulty=easy|medium|hard
&location=forest|dungeon|ruins
```

**Response**: Paginated list of obstacles

---

#### GET /obstacles/{id}
Get a specific obstacle by ID.

---

#### POST /obstacles
Create a new obstacle.

#### PUT /obstacles/{id}
Update an obstacle.

#### DELETE /obstacles/{id}
Delete an obstacle.

---

## Advanced Query Endpoints

### 5. SEARCH ENDPOINTS

#### GET /search
Global full-text search across all entities.

**Query Parameters:**
```
?q=dragon
&entityTypes=creatures,items,npcs
&page=1
&pageSize=10
```

**Response:**
```json
{
  "data": {
    "query": "dragon",
    "results": [
      {
        "entityType": "creature",
        "id": "507f1f77bcf86cd799439011",
        "name": "Black Dragon",
        "imageUrl": "...",
        "snippet": "A powerful dragon that..."
      },
      {
        "entityType": "item",
        "id": "507f1f77bcf86cd799439012",
        "name": "Dragon Slayer Sword",
        "imageUrl": "...",
        "snippet": "Used to slay dragons..."
      }
    ]
  },
  "error": null
}
```

**Status**: 200 OK

---

#### GET /search/autocomplete
Type-ahead search suggestions.

**Query Parameters:**
```
?q=dra
&limit=10
```

**Response:**
```json
{
  "data": [
    {
      "text": "Dragon",
      "type": "creature",
      "id": "507f1f77bcf86cd799439011"
    },
    {
      "text": "Dragon Slayer Sword",
      "type": "item",
      "id": "507f1f77bcf86cd799439012"
    }
  ],
  "error": null
}
```

---

### 6. RELATIONSHIP ENDPOINTS

#### GET /creatures/{id}/loot
Get all items dropped by a specific creature.

**Response**: List of items with drop rates

---

#### GET /items/{id}/dropped-by
Get all creatures that drop a specific item.

**Response**: List of creatures with drop rates

---

#### GET /items/{id}/used-by
Get all creatures that use/equip a specific item.

**Response**: List of creatures

---

#### GET /npcs/{id}/trades
Get detailed trade information for an NPC.

**Response**: Buy/sell lists with prices

---

#### GET /creatures/by-location/{location}
Get all creatures in a specific location.

**Response**: List of creatures in that location

---

### 7. CATEGORY/TYPE ENDPOINTS

#### GET /creatures/types
Get all creature types/classifications.

**Response:**
```json
{
  "data": ["normal", "elite", "boss", "mini-boss"],
  "error": null
}
```

---

#### GET /items/categories
Get all item categories.

**Response:**
```json
{
  "data": [
    { "id": "weapons", "name": "Weapons" },
    { "id": "armor", "name": "Armor" },
    { "id": "tools", "name": "Tools" }
  ],
  "error": null
}
```

---

#### GET /items/rarities
Get all rarity levels.

**Response:**
```json
{
  "data": ["common", "uncommon", "rare", "epic", "legendary"],
  "error": null
}
```

---

## Implementation Notes

### Data Model Requirements
Based on the endpoints above, the following models are required:

**Creature Model:**
- Basic: id, name, type, description, imageUrl
- Stats: health, attack, defense, experience
- Gameplay: location, abilities, weaknesses
- Loot: drops array with itemId, name, dropRate, rarity
- Timestamps: createdAt, updatedAt

**Item Model:**
- Basic: id, name, type, category, rarity, description, imageUrl
- Stats: damage, defense, speed, specialAttributes
- Requirements: minLevel, minStr, minDex
- Acquisition: methods array, sources array (creatures, NPCs, quests)
- Usage: usedBy array
- Timestamps: createdAt, updatedAt

**NPC Model:**
- Basic: id, name, role, description, imageUrl
- Location: area, coordinates, description
- Trades: buys array, sells array (with prices)
- Relationships: quests array
- Timestamps: createdAt, updatedAt

**Obstacle Model:**
- Basic: id, name, type, description, imageUrl
- Properties: difficulty, location, solveMethod
- Timestamps: createdAt, updatedAt

### Database Indexing
For optimal query performance, create indexes on:
- Creatures: name, type, difficulty, location
- Items: name, type, category, rarity
- NPCs: name, role, location
- All entities: For full-text search on name and description

### Pagination Best Practices
- Default page size: 20
- Maximum page size: 100
- Total count provided for UI pagination controls
- Consistent structure across all paginated endpoints

### Search Implementation
- Use MongoDB text search on name and description fields
- Support filters in parallel with search
- Implement autocomplete with prefix matching
- Cache frequently searched terms

---

## API Testing Checklist

- [ ] All CRUD operations (Create, Read, Update, Delete)
- [ ] Pagination with various page sizes
- [ ] Filtering by single and multiple criteria
- [ ] Sorting by different fields
- [ ] Search functionality with relevance ranking
- [ ] Autocomplete suggestions
- [ ] Relationship queries (loot drops, item usage, NPC trades)
- [ ] 404 handling for non-existent resources
- [ ] 400 handling for invalid parameters
- [ ] 500 error handling and logging
- [ ] Performance testing with large datasets
- [ ] Concurrent request handling

---

## Next Steps (After Data Audit)

1. **Developer Agent** provides data audit results
2. Refine data models based on actual data structure
3. Create MongoDB collection schemas
4. Implement all C# models and request/response DTOs
5. Implement all controllers following existing pattern
6. Create integration tests for all endpoints
7. Generate API documentation (Swagger/OpenAPI)
8. Deploy to test environment for frontend integration

---

**Status**: Ready for implementation phase  
**Dependencies**: Awaiting developer data audit results
