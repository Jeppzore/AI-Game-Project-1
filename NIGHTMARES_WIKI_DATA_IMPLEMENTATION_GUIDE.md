# Nightmares Wiki Data Audit - Implementation Guide

## Overview

This package contains a complete data audit and extraction from the Nightmares Wiki HTML repository. All game data has been cataloged, structured, and prepared for MongoDB integration into the AI-Game-Project-1 application.

## Deliverables

### 1. **NIGHTMARES_WIKI_AUDIT_REPORT.md**
The comprehensive audit report containing:
- Directory structure analysis
- Complete creature inventory (3 creatures with full stats)
- Complete item inventory (32 items by category)
- NPC analysis (1 NPC with gaps identified)
- Asset inventory (79 images, 2 MP3 files)
- Data quality assessment
- MongoDB schema design with detailed examples
- C# model class definitions
- Data migration recommendations
- API endpoint suggestions
- Action items and next steps

**Use this for:** Understanding the complete data landscape, making architectural decisions, planning development.

### 2. **NIGHTMARES_WIKI_STRUCTURED_DATA.json**
MongoDB-ready JSON export containing:
- All 3 creatures with full combat stats and lore
- All 32 items organized by category
- 1 NPC with noted gaps
- Asset inventory and categorization
- Data quality summary

**Use this for:**
- Importing data directly into MongoDB
- Seeding development/test databases
- Understanding data format and structure
- Reference during API development

### 3. **Server/Models/GameModels.cs**
Complete C# model class scaffolding including:
- `Creature` model with nested `CombatStats`, `LootDrop`, etc.
- `Item` model with nested `ItemStats`, `ItemSource`, `CraftingInfo`, etc.
- `NPC` model with nested `NPCProfile`, `Trade`, `DialogueEntry`, etc.
- `Obstacle` model for future implementation
- `Asset` model for tracking media
- All supporting classes with proper MongoDB attributes

**Use this for:**
- Immediate integration into your .NET backend
- Database context mapping
- API controller creation
- Type-safe data access

## Quick Start

### Step 1: Review the Audit Report
```bash
# Open and review the complete audit
Start-Process "NIGHTMARES_WIKI_AUDIT_REPORT.md"
```

### Step 2: Examine Your Existing Data Structure
```bash
# The source wiki is still available at:
C:\Repos\NightmaresWiki\
```

### Step 3: Set Up MongoDB Collections
```javascript
// Use MongoDB Compass or mongosh to create collections:

use("nightmares-wiki");

// Create creatures collection
db.createCollection("creatures", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["id", "name", "combat"],
      properties: {
        id: { bsonType: "string" },
        name: { bsonType: "string" },
        combat: { bsonType: "object" }
      }
    }
  }
});

// Create indexes
db.creatures.createIndex({ "id": 1 }, { unique: true });
db.creatures.createIndex({ "type": 1 });
db.creatures.createIndex({ "name": "text" });

// Similar for items, npcs, obstacles, assets
```

### Step 4: Import Data into MongoDB
```bash
# Using mongoimport utility
mongoimport --db nightmares-wiki --collection creatures --file creatures_data.json
mongoimport --db nightmares-wiki --collection items --file items_data.json
mongoimport --db nightmares-wiki --collection npcs --file npcs_data.json
```

Or manually import using MongoDB Compass's Import feature.

### Step 5: Add Models to Your Project
```bash
# The GameModels.cs file is ready to use in your .NET project
# Copy it to: Server/Models/GameModels.cs
# No modifications needed - all MongoDB attributes are configured
```

### Step 6: Create DbContext
```csharp
// In your Data/WikiDbContext.cs
using MongoDB.Driver;
using NightmaresWiki.Models;

public class WikiDbContext
{
    private readonly IMongoDatabase _database;

    public WikiDbContext(IMongoClient mongoClient)
    {
        _database = mongoClient.GetDatabase("nightmares-wiki");
    }

    public IMongoCollection<Creature> Creatures => 
        _database.GetCollection<Creature>("creatures");

    public IMongoCollection<Item> Items => 
        _database.GetCollection<Item>("items");

    public IMongoCollection<NPC> NPCs => 
        _database.GetCollection<NPC>("npcs");

    public IMongoCollection<Obstacle> Obstacles => 
        _database.GetCollection<Obstacle>("obstacles");

    public IMongoCollection<Asset> Assets => 
        _database.GetCollection<Asset>("assets");
}
```

### Step 7: Create API Controllers
```csharp
// Example: CreaturesController.cs
[ApiController]
[Route("api/[controller]")]
public class CreaturesController : ControllerBase
{
    private readonly IMongoCollection<Creature> _creatures;

    public CreaturesController(WikiDbContext context)
    {
        _creatures = context.Creatures;
    }

    [HttpGet]
    public async Task<ActionResult<List<Creature>>> GetAll()
    {
        var creatures = await _creatures.Find(_ => true).ToListAsync();
        return Ok(creatures);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Creature>> GetById(string id)
    {
        var creature = await _creatures.Find(c => c.CreatureId == id).FirstOrDefaultAsync();
        if (creature == null) return NotFound();
        return Ok(creature);
    }

    // Similar implementations for other CRUD operations
}
```

## Data Schema Overview

### Collections Structure

**creatures**
```javascript
{
  _id: ObjectId,
  id: "gaklorr",
  name: "Gaklorr",
  type: ["Boss", "Orc"],
  combat: {
    health: 50,
    experience: 100,
    armor: 2,
    speed: 45,
    damage: { min: 5, max: 10 },
    aggroRange: 1000,
    immunities: [],
    abilities: ["Rallying Cry"]
  },
  loot: [
    { itemName: "Copper Coin", quantity: { min: 1, max: 3 }, dropChance: 1.0 }
  ],
  description: "...",
  lore: "...",
  image: "gaklorr.gif",
  behavior: "...",
  difficulty: "Boss",
  created_at: ISODate,
  updated_at: ISODate
}
```

**items**
```javascript
{
  _id: ObjectId,
  id: "iron-sword",
  name: "Iron Sword",
  category: "weapon_melee",
  type: "Melee Weapon",
  image: "ironSword.png",
  stats: {
    damage: { min: 2, max: 4 },
    speed: 90,
    hands: 1
  },
  value: { gold: 150, rarity: "uncommon" },
  sources: [
    { type: "creature_drop", creatureName: "Gaklorr", dropChance: 0.5 }
  ],
  crafting: {
    craftable: true,
    recipes: [...]
  },
  description: "...",
  created_at: ISODate,
  updated_at: ISODate
}
```

**npcs**
```javascript
{
  _id: ObjectId,
  id: "merchant_john",
  name: "John the Merchant",
  profile: {
    role: "Merchant",
    location: "Town Square",
    coordinates: { x: 100, y: 200 }
  },
  trades: [
    { buys: {...}, sells: {...} }
  ],
  dialogue: [
    { trigger: "greeting", text: "Welcome, traveler!" }
  ],
  relationships: {
    friends: [...],
    enemies: [...],
    faction: "Traders Guild"
  },
  created_at: ISODate,
  updated_at: ISODate
}
```

## Data Quality Notes

### Complete (Ready to Use)
- ✅ Creature data (95% complete) - All 3 creatures have full stats, lore, and behavior
- ✅ Item stats and drop sources - Weapons, armor, and basic items documented
- ✅ Image assets - 79 images cataloged and available
- ✅ Audio assets - 2 MP3 files available

### Partial (Needs Development)
- ⚠️ Item crafting info - Sparse or incomplete for most items
- ⚠️ NPC data (10% complete) - Only 1 NPC documented
- ⚠️ Key items - 3 key files exist but are empty

### Not Implemented
- ❌ Obstacle system
- ❌ Quest system
- ❌ Guide content
- ❌ Consumable items
- ❌ Power-up items

## Recommended Development Order

### Phase 1: Foundation (Week 1)
1. Import creature and item data
2. Create basic CRUD endpoints
3. Build wiki view pages
4. Set up search functionality

### Phase 2: Expansion (Week 2-3)
1. Populate missing item data (especially keys)
2. Develop NPC system with at least 5 NPCs
3. Implement item filtering by category
4. Add creature/item relationships display

### Phase 3: Systems (Week 4-6)
1. Implement crafting system
2. Create trading system
3. Add quest framework
4. Implement obstacles

### Phase 4: Features (Ongoing)
1. Leveling system
2. Skills system
3. Multiplayer functionality
4. Admin panel for wiki management

## Key Statistics

| Category | Count | Completeness | Status |
|----------|-------|--------------|--------|
| Creatures | 3 | 95% | Ready |
| Items | 32 | 70% | Partial |
| NPCs | 1 | 10% | Needs Work |
| Obstacles | 0 | 0% | Not Started |
| Images | 79 | 100% | Complete |
| Audio | 2 | 100% | Complete |

## File Organization

```
AI-Game-Project-1/
├── NIGHTMARES_WIKI_AUDIT_REPORT.md          # This guide
├── NIGHTMARES_WIKI_STRUCTURED_DATA.json     # Data for import
├── Server/
│   ├── Models/
│   │   └── GameModels.cs                    # MongoDB models (ready to use)
│   ├── Data/
│   │   └── WikiDbContext.cs                 # Database context (needs creation)
│   └── Controllers/
│       ├── CreaturesController.cs           # Endpoints for creatures
│       ├── ItemsController.cs               # Endpoints for items
│       └── NPCsController.cs                # Endpoints for NPCs
└── C:\Repos\NightmaresWiki\                 # Original source HTML files
```

## MongoDB Connection String

```
mongodb://localhost:27017/nightmares-wiki
```

For development with Docker:
```
mongodb://mongodb:27017/nightmares-wiki
```

## Troubleshooting

### Issue: Models missing MongoDB driver attributes
**Solution:** Install NuGet package: `MongoDB.Driver`

### Issue: Can't import JSON files to MongoDB
**Solution:** Ensure MongoDB server is running and accessible
```bash
# Check MongoDB status
mongod --version

# Start MongoDB if not running
mongod
```

### Issue: ObjectId conversion errors
**Solution:** Use `BsonRepresentation(BsonType.ObjectId)` attribute on ObjectId properties

### Issue: Creature/Item IDs not unique
**Solution:** Create unique index on the `id` field
```javascript
db.creatures.createIndex({ "id": 1 }, { unique: true })
```

## Additional Resources

- **MongoDB .NET Driver Documentation:** https://www.mongodb.com/docs/drivers/csharp/
- **Original Wiki Source:** C:\Repos\NightmaresWiki\
- **Extracted Data Files:** NIGHTMARES_WIKI_STRUCTURED_DATA.json
- **Models Reference:** Server/Models/GameModels.cs

## Next Steps

1. **Review** the audit report in detail
2. **Set up** MongoDB collections and indexes
3. **Import** the structured JSON data
4. **Integrate** GameModels.cs into your project
5. **Create** database context (WikiDbContext)
6. **Build** API controllers using the provided endpoints
7. **Develop** React components for wiki views
8. **Extend** with additional NPCs and items

## Contact & Questions

For questions about the data structure, schema design, or implementation approach, refer to:
- **NIGHTMARES_WIKI_AUDIT_REPORT.md** - Full technical documentation
- **NIGHTMARES_WIKI_STRUCTURED_DATA.json** - Actual data format examples
- **Server/Models/GameModels.cs** - Model class documentation

---

**Data Audit Completed:** March 17, 2026  
**Status:** Ready for Implementation ✓  
**Next Review:** After Phase 1 completion
