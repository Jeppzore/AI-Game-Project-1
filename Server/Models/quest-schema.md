# Quest System MongoDB Schema

## Collections Overview

This document defines the MongoDB schema for the Quest System in the Nightmares Wiki.

---

## 1. Quests Collection

**Collection Name:** `quests`

Represents available quests in the game world.

```json
{
  "_id": ObjectId,
  "questId": "string (unique identifier)",
  "title": "string",
  "description": "string",
  "giver_npc_id": ObjectId,
  "giver_npc_name": "string",
  "level_requirement": 1,
  "objectives": [ObjectId],
  "rewards": [ObjectId],
  "prerequisites": [ObjectId],
  "is_repeatable": boolean,
  "created_at": ISODate,
  "updated_at": ISODate
}
```

**Fields:**
- `_id`: MongoDB ObjectId
- `questId`: Unique string identifier for the quest
- `title`: Quest name (e.g., "The Lost Artifact")
- `description`: Long-form quest description
- `giver_npc_id`: Reference to NPC who gives the quest
- `giver_npc_name`: Name of quest giver (denormalized)
- `level_requirement`: Minimum player level required
- `objectives`: Array of Objective ObjectIds
- `rewards`: Array of Reward ObjectIds
- `prerequisites`: Array of prerequisite Quest ObjectIds (for quest chains)
- `is_repeatable`: Whether player can repeat this quest
- `created_at`: When quest was created
- `updated_at`: Last update timestamp

---

## 2. Objectives Collection

**Collection Name:** `objectives`

Individual quest objectives/goals.

```json
{
  "_id": ObjectId,
  "objectiveId": "string",
  "type": "kill_enemy|collect_item|reach_location|talk_to_npc|loot_item",
  "description": "string",
  "target_id": ObjectId,
  "target_name": "string",
  "target_quantity": 1,
  "optional": false,
  "created_at": ISODate
}
```

**Fields:**
- `_id`: MongoDB ObjectId
- `objectiveId`: Unique string identifier
- `type`: Objective type enum:
  - `kill_enemy`: Kill X enemies
  - `collect_item`: Collect X items
  - `reach_location`: Reach a location
  - `talk_to_npc`: Talk to NPC
  - `loot_item`: Loot item from specific source
- `description`: Human-readable objective text
- `target_id`: ObjectId reference (enemy, item, location, or NPC)
- `target_name`: Name of target (denormalized)
- `target_quantity`: How many times to complete (e.g., 5 kills)
- `optional`: Whether this objective is optional
- `created_at`: Creation timestamp

---

## 3. QuestProgress Collection

**Collection Name:** `quest_progress`

Tracks individual player progress on quests.

```json
{
  "_id": ObjectId,
  "player_id": "string",
  "quest_id": ObjectId,
  "quest_name": "string",
  "status": "not_started|accepted|in_progress|completed|abandoned",
  "objectives_progress": [
    {
      "objective_id": ObjectId,
      "objective_type": "kill_enemy|collect_item|...",
      "completed": false,
      "progress": 0,
      "target": 5
    }
  ],
  "started_at": ISODate,
  "completed_at": ISODate,
  "abandoned_at": ISODate,
  "created_at": ISODate,
  "updated_at": ISODate
}
```

**Fields:**
- `_id`: MongoDB ObjectId
- `player_id`: Player identifier (currently hardcoded)
- `quest_id`: Reference to Quest
- `quest_name`: Quest title (denormalized)
- `status`: Progress status:
  - `not_started`: Quest accepted but not started
  - `accepted`: Player has accepted the quest
  - `in_progress`: Player is actively working on objectives
  - `completed`: All objectives done, quest finished
  - `abandoned`: Player abandoned the quest
- `objectives_progress`: Array of objective progress objects:
  - `objective_id`: Reference to Objective
  - `objective_type`: Type of objective
  - `completed`: Whether this objective is complete
  - `progress`: Current progress (0-target_quantity)
  - `target`: Target quantity
- `started_at`: When player started the quest
- `completed_at`: When all objectives were completed
- `abandoned_at`: When player abandoned (if applicable)
- `created_at`: Record creation time
- `updated_at`: Last modification time

---

## 4. Rewards Collection

**Collection Name:** `rewards`

Quest reward definitions.

```json
{
  "_id": ObjectId,
  "reward_id": "string",
  "quest_id": ObjectId,
  "reward_type": "item|xp|gold",
  "reward_value": 500,
  "reward_item_id": ObjectId,
  "reward_item_name": "string",
  "quantity": 1,
  "created_at": ISODate
}
```

**Fields:**
- `_id`: MongoDB ObjectId
- `reward_id`: Unique string identifier
- `quest_id`: Reference to Quest
- `reward_type`: Type of reward:
  - `item`: Give player an item
  - `xp`: Give experience points
  - `gold`: Give currency
- `reward_value`: Numeric value (XP amount or gold amount)
- `reward_item_id`: ObjectId if type is "item"
- `reward_item_name`: Item name (denormalized)
- `quantity`: How many of the item (if item reward)
- `created_at`: Creation timestamp

---

## Sample Quests

### Quest 1: The Lost Artifact

```json
{
  "_id": ObjectId("quest_1"),
  "questId": "lost_artifact",
  "title": "The Lost Artifact",
  "description": "The ancient artifact was stolen by shadow creatures. Retrieve it from the Dark Cavern and return it to the Elder.",
  "giver_npc_id": ObjectId("npc_elder"),
  "giver_npc_name": "Elder Kael",
  "level_requirement": 10,
  "objectives": [
    ObjectId("obj_kill_shadows"),
    ObjectId("obj_loot_artifact")
  ],
  "rewards": [
    ObjectId("reward_xp_500"),
    ObjectId("reward_gold_1000"),
    ObjectId("reward_artifact_key")
  ],
  "prerequisites": [],
  "is_repeatable": false,
  "created_at": ISODate(),
  "updated_at": ISODate()
}
```

**Objectives:**
- Kill 5 Shadow Creatures
- Loot the Lost Artifact

**Rewards:**
- 500 XP
- 1000 Gold
- Artifact Key (item)

---

### Quest 2: Gathering Resources

```json
{
  "_id": ObjectId("quest_2"),
  "questId": "gathering_resources",
  "title": "Gathering Resources",
  "description": "The blacksmith needs materials to craft better equipment. Collect rare ore and herbs from the surrounding areas.",
  "giver_npc_id": ObjectId("npc_blacksmith"),
  "giver_npc_name": "Blacksmith Thorne",
  "level_requirement": 5,
  "objectives": [
    ObjectId("obj_collect_ore"),
    ObjectId("obj_collect_herbs")
  ],
  "rewards": [
    ObjectId("reward_xp_250"),
    ObjectId("reward_gold_500")
  ],
  "prerequisites": [],
  "is_repeatable": true,
  "created_at": ISODate(),
  "updated_at": ISODate()
}
```

**Objectives:**
- Collect 10 Rare Ore
- Collect 5 Moonflower Herbs

**Rewards:**
- 250 XP
- 500 Gold

---

### Quest 3: Rescue Mission

```json
{
  "_id": ObjectId("quest_3"),
  "questId": "rescue_mission",
  "title": "Rescue Mission",
  "description": "A villager was captured by forest bandits. Defeat the bandits, free the prisoner, and escort them back to safety.",
  "giver_npc_id": ObjectId("npc_captain"),
  "giver_npc_name": "Captain Drake",
  "level_requirement": 15,
  "objectives": [
    ObjectId("obj_defeat_bandits"),
    ObjectId("obj_free_prisoner"),
    ObjectId("obj_reach_village")
  ],
  "rewards": [
    ObjectId("reward_xp_750"),
    ObjectId("reward_gold_1500"),
    ObjectId("reward_badge")
  ],
  "prerequisites": [
    ObjectId("quest_1")
  ],
  "is_repeatable": false,
  "created_at": ISODate(),
  "updated_at": ISODate()
}
```

**Objectives:**
- Defeat 3 Bandits
- Free the Prisoner
- Reach the Village (location-based)

**Prerequisites:**
- Must complete "The Lost Artifact" first

**Rewards:**
- 750 XP
- 1500 Gold
- Hero's Badge (item)

---

### Quest 4: Dragon Slayer

```json
{
  "_id": ObjectId("quest_4"),
  "questId": "dragon_slayer",
  "title": "Dragon Slayer",
  "description": "The mighty dragon terrorizes the northern realm. Slay the beast and claim its hoard.",
  "giver_npc_id": ObjectId("npc_king"),
  "giver_npc_name": "King Aldric",
  "level_requirement": 25,
  "objectives": [
    ObjectId("obj_slay_dragon"),
    ObjectId("obj_loot_dragon_hoard")
  ],
  "rewards": [
    ObjectId("reward_xp_2000"),
    ObjectId("reward_gold_5000"),
    ObjectId("reward_dragon_stone"),
    ObjectId("reward_royal_armor")
  ],
  "prerequisites": [
    ObjectId("quest_3")
  ],
  "is_repeatable": false,
  "created_at": ISODate(),
  "updated_at": ISODate()
}
```

**Objectives:**
- Slay the Dragon
- Loot Dragon Hoard

**Prerequisites:**
- Must complete "Rescue Mission" first

**Rewards:**
- 2000 XP
- 5000 Gold
- Dragon Stone (crafting material)
- Royal Armor Set (item)

---

## Indexes

For optimal performance, create the following indexes:

```javascript
// Quests collection
db.quests.createIndex({ "questId": 1 }, { unique: true })
db.quests.createIndex({ "giver_npc_id": 1 })
db.quests.createIndex({ "level_requirement": 1 })

// Objectives collection
db.objectives.createIndex({ "objectiveId": 1 }, { unique: true })
db.objectives.createIndex({ "type": 1 })

// Quest Progress collection
db.quest_progress.createIndex({ "player_id": 1, "quest_id": 1 }, { unique: true })
db.quest_progress.createIndex({ "player_id": 1, "status": 1 })
db.quest_progress.createIndex({ "quest_id": 1 })

// Rewards collection
db.rewards.createIndex({ "quest_id": 1 })
db.rewards.createIndex({ "reward_type": 1 })
```

---

## Integration Points

### With Enemies System
- Objectives track kills of specific enemies
- When player kills enemy, progress is updated
- Kills are tracked in quest_progress

### With Items System
- Quest rewards dispense items via Trading system
- Collect item objectives reference items
- Loot objectives reference specific item drops

### With NPC System
- Quest givers are NPCs
- Talk objectives reference NPCs
- NPC.Quests array references available quests

### With Player System
- Quest progress tracked per player_id
- Rewards update player inventory
- Completion may affect player level/skills

