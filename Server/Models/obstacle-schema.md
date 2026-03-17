# Obstacle System - MongoDB Schema Design

## Overview
The Obstacle System provides interactive challenges in the Nightmares game world, including riddles, locked containers, traps, and puzzles. Players earn rewards for successfully overcoming obstacles.

## Collections

### obstacles
Represents interactive obstacles and challenges in the game world.

```json
{
  "_id": ObjectId,
  "id": "string (unique identifier)",
  "name": "string",
  "description": "string",
  "location": "string",
  "type": "string (enum: riddle, locked_chest, trapped_door, puzzle)",
  "difficulty": "number (1-10)",
  "challengeData": {
    "type": "string",
    "question": "string (riddles only)",
    "correctAnswer": "string (riddles only)",
    "hints": ["string"] (riddles only),
    "lockDifficulty": "number (chests only, 1-10)",
    "requiredKeyId": "string (chests only)",
    "triggerType": "string (traps only, e.g., 'pressure_plate', 'tripwire')",
    "detectionDifficulty": "number (traps only, 1-10)",
    "disarmDifficulty": "number (traps only, 1-10)",
    "steps": ["string"] (puzzles only),
    "requiredItems": ["string"] (puzzles only)
  },
  "rewards": {
    "items": [
      {
        "itemId": "string",
        "quantity": "number",
        "dropChance": "number (0-1)"
      }
    ],
    "experience": "number",
    "gold": "number"
  },
  "successRateModifier": "number (0-1, difficulty modifier)",
  "createdAt": "Date",
  "updatedAt": "Date"
}
```

### obstacleAttempts
Tracks all player attempts on obstacles for progression and analytics.

```json
{
  "_id": ObjectId,
  "id": "string (unique identifier)",
  "playerId": "string",
  "obstacleId": "string (reference to obstacles collection)",
  "success": "boolean",
  "attemptNumber": "number",
  "timestamp": "Date",
  "solutionProvided": "string (answer/solution text)",
  "damageReceived": "number (on failure for traps/combat)",
  "rewardsClaimed": "boolean"
}
```

## Sample Obstacles

### 1. Sphinx Riddle (Easy)
```json
{
  "id": "sphinx-riddle-1",
  "name": "The Sphinx's Riddle",
  "description": "An ancient sphinx blocks the path. Answer her riddle to pass.",
  "location": "Forest Path - Sphinx Clearing",
  "type": "riddle",
  "difficulty": 3,
  "challengeData": {
    "type": "riddle",
    "question": "What has a mouth but cannot speak, a head but cannot weep, a bed but cannot sleep?",
    "correctAnswer": "river",
    "hints": [
      "Think of something natural...",
      "It flows and moves...",
      "Often found in valleys..."
    ]
  },
  "rewards": {
    "items": [
      { "itemId": "wisdom_scroll", "quantity": 1, "dropChance": 1.0 }
    ],
    "experience": 100,
    "gold": 50
  },
  "successRateModifier": 0.8
}
```

### 2. Ancient Chest (Medium)
```json
{
  "id": "ancient-chest-1",
  "name": "Locked Ancient Chest",
  "description": "A heavy wooden chest adorned with mysterious symbols. It appears to be locked.",
  "location": "Dungeon - Treasure Chamber",
  "type": "locked_chest",
  "difficulty": 5,
  "challengeData": {
    "type": "locked_chest",
    "lockDifficulty": 6,
    "requiredKeyId": "bronze_key"
  },
  "rewards": {
    "items": [
      { "itemId": "gold_coins", "quantity": 200, "dropChance": 1.0 },
      { "itemId": "amulet_of_protection", "quantity": 1, "dropChance": 0.5 },
      { "itemId": "health_potion", "quantity": 3, "dropChance": 1.0 }
    ],
    "experience": 250,
    "gold": 100
  },
  "successRateModifier": 0.75
}
```

### 3. Trapped Door (Medium-Hard)
```json
{
  "id": "trapped-door-1",
  "name": "Trapped Dungeon Door",
  "description": "A stone door with suspicious scratches around the lock mechanism.",
  "location": "Dungeon - Corridor",
  "type": "trapped_door",
  "difficulty": 6,
  "challengeData": {
    "type": "trapped_door",
    "triggerType": "pressure_plate",
    "detectionDifficulty": 7,
    "disarmDifficulty": 8
  },
  "rewards": {
    "items": [],
    "experience": 150,
    "gold": 75
  },
  "successRateModifier": 0.7
}
```

### 4. Ancient Cipher Puzzle (Hard)
```json
{
  "id": "cipher-puzzle-1",
  "name": "The Lost Archive Cipher",
  "description": "Ancient texts scattered around a pedestal with magical runes.",
  "location": "Wizard's Tower - Library",
  "type": "puzzle",
  "difficulty": 8,
  "challengeData": {
    "type": "puzzle",
    "steps": [
      "Arrange the runestones in order of power",
      "Speak the incantation from the tome",
      "Place the seal on the pedestal"
    ],
    "requiredItems": ["crystal_runestone", "ancient_tome", "mage_seal"]
  },
  "rewards": {
    "items": [
      { "itemId": "spellbook_arcane", "quantity": 1, "dropChance": 1.0 },
      { "itemId": "mana_crystal", "quantity": 5, "dropChance": 0.8 }
    ],
    "experience": 500,
    "gold": 200
  },
  "successRateModifier": 0.6
}
```

### 5. Riddle of the Guardian (Hard)
```json
{
  "id": "guardian-riddle-1",
  "name": "The Guardian's Challenge",
  "description": "A stone golem stands before the sacred chamber, demanding proof of wit.",
  "location": "Temple - Guardian Chamber",
  "type": "riddle",
  "difficulty": 7,
  "challengeData": {
    "type": "riddle",
    "question": "I am not alive, but I grow; I don't have lungs, but I need air; I don't have a mouth, but water kills me. What am I?",
    "correctAnswer": "fire",
    "hints": [
      "Consider the elements...",
      "Often dangerous in a forest...",
      "Needs fuel to continue..."
    ]
  },
  "rewards": {
    "items": [
      { "itemId": "guardian_token", "quantity": 1, "dropChance": 1.0 },
      { "itemId": "rare_gem", "quantity": 2, "dropChance": 0.7 }
    ],
    "experience": 300,
    "gold": 150
  },
  "successRateModifier": 0.65
}
```

### 6. Treasure Vault (Hard)
```json
{
  "id": "treasure-vault-1",
  "name": "Legendary Treasure Vault",
  "description": "A massive iron vault reinforced with multiple locking mechanisms.",
  "location": "Castle - Treasury",
  "type": "locked_chest",
  "difficulty": 9,
  "challengeData": {
    "type": "locked_chest",
    "lockDifficulty": 9,
    "requiredKeyId": "master_vault_key"
  },
  "rewards": {
    "items": [
      { "itemId": "legendary_sword", "quantity": 1, "dropChance": 1.0 },
      { "itemId": "gold_bars", "quantity": 10, "dropChance": 1.0 },
      { "itemId": "crown_jewel", "quantity": 1, "dropChance": 0.3 }
    ],
    "experience": 1000,
    "gold": 500
  },
  "successRateModifier": 0.5
}
```

### 7. Boss Trap Chamber (Extreme)
```json
{
  "id": "boss-trap-chamber-1",
  "name": "Boss Chamber Trap",
  "description": "The chamber of a defeated boss contains deadly magical traps.",
  "location": "Dragon's Lair - Treasure Room",
  "type": "trapped_door",
  "difficulty": 10,
  "challengeData": {
    "type": "trapped_door",
    "triggerType": "magical_ward",
    "detectionDifficulty": 9,
    "disarmDifficulty": 10
  },
  "rewards": {
    "items": [
      { "itemId": "dragon_scale_armor", "quantity": 1, "dropChance": 1.0 }
    ],
    "experience": 2000,
    "gold": 1000
  },
  "successRateModifier": 0.4
}
```

### 8. Cryptic Stone Puzzle (Very Hard)
```json
{
  "id": "stone-puzzle-1",
  "name": "The Stone Keeper's Test",
  "description": "Enchanted stones must be arranged in the correct pattern to unlock passage.",
  "location": "Mountain Pass - Stone Hall",
  "type": "puzzle",
  "difficulty": 9,
  "challengeData": {
    "type": "puzzle",
    "steps": [
      "Read the prophecy etched in stone",
      "Arrange the stones by constellation",
      "Activate the seal with starlight"
    ],
    "requiredItems": ["prophecy_stone", "constellation_stones", "starlight_prism"]
  },
  "rewards": {
    "items": [
      { "itemId": "cosmic_staff", "quantity": 1, "dropChance": 1.0 },
      { "itemId": "essence_of_stars", "quantity": 3, "dropChance": 1.0 }
    ],
    "experience": 800,
    "gold": 300
  },
  "successRateModifier": 0.55
}
```

## Indexes

The following indexes should be created for performance:
- `obstacles.name` (ascending)
- `obstacles.type` (ascending)
- `obstacles.difficulty` (ascending)
- `obstacles.location` (ascending)
- `obstacleAttempts.playerId` (ascending)
- `obstacleAttempts.obstacleId` (ascending)
- `obstacleAttempts.timestamp` (descending)

## Key Features

1. **Type-specific Challenge Data**: Each obstacle type has unique challenge parameters
2. **Difficulty Scaling**: 1-10 scale affects success rates and rewards
3. **Flexible Rewards**: Can grant items with varying drop chances, experience, and gold
4. **Attempt Tracking**: Records all attempts for progression and analytics
5. **Success Rate Modifier**: Difficulty acts as a modifier for success calculations
