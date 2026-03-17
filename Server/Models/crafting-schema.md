# Crafting System MongoDB Schema

## Collections

### CraftingRecipes Collection

**Collection Name:** `crafting_recipes`

**Purpose:** Stores all available crafting recipes in the game world

**Schema:**

```javascript
{
  _id: ObjectId,                              // MongoDB auto-generated ID
  name: String,                                // Recipe name (e.g., "Iron Sword")
  description: String,                         // Human-readable description
  requiredItems: [                            // Array of required ingredients
    {
      itemId: ObjectId,                       // Reference to Item in items collection
      quantity: Int32                          // Quantity required
    },
    ...
  ],
  outputItem: {
    itemId: ObjectId,                         // Reference to Item produced
    quantity: Int32                           // Quantity produced
  },
  levelRequirement: Int32,                    // Minimum player level to craft
  skillRequirement: String,                   // Crafting skill type (e.g., "Smithing", "Alchemy")
  skillLevel: Int32,                          // Minimum skill level needed
  cooldownSeconds: Int32,                     // Cooldown after crafting (0 for no cooldown)
  xpReward: Int32,                            // Experience points gained
  createdAt: Date,                            // When recipe was created
  updatedAt: Date                             // When recipe was last updated
}
```

**Indexes:**
- `name` (ascending) - For quick recipe lookups by name
- `skillRequirement` (ascending) - For filtering recipes by skill type
- `levelRequirement` (ascending) - For filtering recipes by level

---

## Sample Recipes

### Recipe 1: Iron Sword

```json
{
  "_id": ObjectId("507f1f77bcf86cd799439011"),
  "name": "Iron Sword",
  "description": "A basic sword forged from iron ore and ingots",
  "requiredItems": [
    {
      "itemId": ObjectId("507f1f77bcf86cd799439001"),
      "quantity": 3
    },
    {
      "itemId": ObjectId("507f1f77bcf86cd799439002"),
      "quantity": 1
    }
  ],
  "outputItem": {
    "itemId": ObjectId("507f1f77bcf86cd799439010"),
    "quantity": 1
  },
  "levelRequirement": 5,
  "skillRequirement": "Smithing",
  "skillLevel": 10,
  "cooldownSeconds": 0,
  "xpReward": 100,
  "createdAt": ISODate("2024-01-15T10:00:00Z"),
  "updatedAt": ISODate("2024-01-15T10:00:00Z")
}
```

### Recipe 2: Health Potion

```json
{
  "_id": ObjectId("507f1f77bcf86cd799439012"),
  "name": "Health Potion",
  "description": "A magical elixir that restores health",
  "requiredItems": [
    {
      "itemId": ObjectId("507f1f77bcf86cd799439003"),
      "quantity": 2
    },
    {
      "itemId": ObjectId("507f1f77bcf86cd799439004"),
      "quantity": 1
    },
    {
      "itemId": ObjectId("507f1f77bcf86cd799439005"),
      "quantity": 1
    }
  ],
  "outputItem": {
    "itemId": ObjectId("507f1f77bcf86cd799439020"),
    "quantity": 3
  },
  "levelRequirement": 1,
  "skillRequirement": "Alchemy",
  "skillLevel": 5,
  "cooldownSeconds": 0,
  "xpReward": 50,
  "createdAt": ISODate("2024-01-15T10:00:00Z"),
  "updatedAt": ISODate("2024-01-15T10:00:00Z")
}
```

### Recipe 3: Mana Potion

```json
{
  "_id": ObjectId("507f1f77bcf86cd799439013"),
  "name": "Mana Potion",
  "description": "Restores magical energy",
  "requiredItems": [
    {
      "itemId": ObjectId("507f1f77bcf86cd799439006"),
      "quantity": 3
    },
    {
      "itemId": ObjectId("507f1f77bcf86cd799439007"),
      "quantity": 1
    }
  ],
  "outputItem": {
    "itemId": ObjectId("507f1f77bcf86cd799439021"),
    "quantity": 2
  },
  "levelRequirement": 3,
  "skillRequirement": "Alchemy",
  "skillLevel": 8,
  "cooldownSeconds": 0,
  "xpReward": 75,
  "createdAt": ISODate("2024-01-15T10:00:00Z"),
  "updatedAt": ISODate("2024-01-15T10:00:00Z")
}
```

### Recipe 4: Leather Armor

```json
{
  "_id": ObjectId("507f1f77bcf86cd799439014"),
  "name": "Leather Armor",
  "description": "Light but protective armor made from leather",
  "requiredItems": [
    {
      "itemId": ObjectId("507f1f77bcf86cd799439008"),
      "quantity": 5
    },
    {
      "itemId": ObjectId("507f1f77bcf86cd799439009"),
      "quantity": 2
    }
  ],
  "outputItem": {
    "itemId": ObjectId("507f1f77bcf86cd799439030"),
    "quantity": 1
  },
  "levelRequirement": 8,
  "skillRequirement": "Tailoring",
  "skillLevel": 12,
  "cooldownSeconds": 0,
  "xpReward": 150,
  "createdAt": ISODate("2024-01-15T10:00:00Z"),
  "updatedAt": ISODate("2024-01-15T10:00:00Z")
}
```

### Recipe 5: Enchanted Ring

```json
{
  "_id": ObjectId("507f1f77bcf86cd799439015"),
  "name": "Enchanted Ring",
  "description": "A magical ring imbued with power",
  "requiredItems": [
    {
      "itemId": ObjectId("507f1f77bcf86cd799439011"),
      "quantity": 1
    },
    {
      "itemId": ObjectId("507f1f77bcf86cd799439012"),
      "quantity": 3
    },
    {
      "itemId": ObjectId("507f1f77bcf86cd799439013"),
      "quantity": 1
    }
  ],
  "outputItem": {
    "itemId": ObjectId("507f1f77bcf86cd799439040"),
    "quantity": 1
  },
  "levelRequirement": 15,
  "skillRequirement": "Enchanting",
  "skillLevel": 20,
  "cooldownSeconds": 30,
  "xpReward": 250,
  "createdAt": ISODate("2024-01-15T10:00:00Z"),
  "updatedAt": ISODate("2024-01-15T10:00:00Z")
}
```

---

## Collection Access Pattern

**Get all recipes:**
```javascript
db.crafting_recipes.find({})
```

**Get recipes by skill:**
```javascript
db.crafting_recipes.find({ skillRequirement: "Smithing" })
```

**Get recipes by level requirement:**
```javascript
db.crafting_recipes.find({ levelRequirement: { $lte: 10 } })
```

**Get single recipe:**
```javascript
db.crafting_recipes.findOne({ _id: ObjectId("507f1f77bcf86cd799439011") })
```
