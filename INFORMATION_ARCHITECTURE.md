# Nightmares Wiki - Information Architecture Document

## Executive Summary

This document defines the comprehensive information architecture for the Nightmares Wiki overhaul. It provides the blueprint for organizing game content, defining data models, and structuring user interactions across the wiki platform. This document will guide design and development efforts to ensure consistency and completeness.

---

## 1. CONTENT INVENTORY & AUDIT

### Current State Analysis (from NightmaresWiki repository)

#### CREATURES
- **Current Count**: 4 documented creatures
  - Gaklorr (Boss)
  - Green Slime (Normal)
  - Red Mushroom (NPC/Obstacle - ambiguous)
  - Unspecified additional creatures referenced in bestiary

- **Planned Categories**:
  - Normal Enemies (regular spawn creatures)
  - Elite Enemies (rare, tougher variants)
  - Boss Creatures (unique, story-significant encounters)
  - Mini-Bosses (challenging but not full bosses)
  - Unique Creatures (special one-of-a-kind entities)

#### ITEMS
- **Current Count**: ~41 item pages across subcategories

- **Weapon Categories**:
  - Melee Weapons (swords, axes, hammers, maces, spears)
  - Ranged Weapons (bows, crossbows, thrown weapons)

- **Armor Categories**:
  - Head Armor (helmets, crowns, circlets)
  - Body Armor (chests, robes, plates)
  - Leg Armor (legs, leggings, pants)

- **Accessory Items**:
  - Body Equipment (accessories, rings, amulets - separate directory)

- **Consumable/Quest Items**:
  - Power-ups (potions, temporary buffs)
  - Valuables (coins, gems, quest items)
  - Tools (crafting items, utility items)
  - Other Items (miscellaneous)

#### NPCs
- **Current Count**: 1 documented NPC
  - Red Mushroom (listed as NPC)
  - Additional NPCs referenced but not yet documented

- **Expected Types**:
  - Merchants (buy/sell items)
  - Quest Givers (offer quests/objectives)
  - Story Characters (narrative significance)
  - Trainers/Specialists (provide skills/upgrades)

#### OBSTACLES/INTERACTABLES
- **Current Count**: 13 documented obstacles

- **Categories**:
  - Bush/Vegetation (bushes, trees)
  - Treasure Chests (loot containers)
  - Mining Veins (resource nodes)
  - Doors/Barriers (environmental obstacles)
  - Traps (hazardous elements)
  - Destructibles (breakable objects)

#### GUIDES & CONTENT
- **Guides**: Beginner guides, build guides, farming guides, etc.
- **Mechanics Explanations**: Game systems, combat, progression
- **Lore/Storyline**: World-building content
- **Community Content**: Fan content, strategies

### Inventory Targets
Based on analysis, the wiki should eventually contain:
- **Creatures**: 50-100+ creatures (variety of types and difficulties)
- **Items**: 200-300+ items (weapons, armor, accessories)
- **NPCs**: 20-40+ unique NPCs
- **Obstacles**: 20-30+ interactive elements
- **Guides**: 15-25+ comprehensive guides

---

## 2. ENTITY DATA MODELS

### CREATURE Entity Model

```
CREATURE
├── IDENTITY
│   ├── Name (string, unique)
│   ├── Type (enum: normal, elite, boss, miniboss, unique)
│   ├── Rarity (enum: common, uncommon, rare, legendary, unique)
│   ├── Level/Difficulty (integer 1-100)
│   └── Description (text, 200-500 words with lore)
│
├── VISUALS
│   ├── Portrait Image
│   ├── In-Game Sprite/Animation
│   ├── Size Classification (tiny, small, medium, large, huge)
│   └── Color/Appearance Tags
│
├── COMBAT STATS
│   ├── Health (HP)
│   ├── Damage (min-max range, damage type)
│   ├── Armor (defense value)
│   ├── Resistance (damage type: resist %)
│   ├── Speed/Mobility
│   ├── Aggro Range (detection distance)
│   └── Experience Reward
│
├── ABILITIES & BEHAVIOR
│   ├── Special Abilities (list with descriptions)
│   ├── Attack Patterns (melee, ranged, AoE, etc.)
│   ├── Immunities (status effects immune to)
│   ├── Weaknesses (damage types dealt bonus damage)
│   ├── Elemental Alignment (fire, ice, poison, etc.)
│   ├── Behavior Description (how it acts)
│   └── Difficulty Notes
│
├── LOOT TABLE
│   ├── Common Drops (item, quantity, rarity %)
│   ├── Uncommon Drops (item, quantity, rarity %)
│   ├── Rare Drops (item, quantity, rarity %)
│   ├── Guaranteed Drops (item, quantity)
│   └── Special/Unique Drops (boss items, crafting materials)
│
├── LOCATION & DISTRIBUTION
│   ├── Primary Spawn Locations (map zones)
│   ├── Secondary Locations (rare spawns)
│   ├── Spawn Conditions (time, weather, events)
│   ├── Spawn Rate (frequency)
│   ├── Group Behavior (solo, pack, swarm)
│   └── Elevation/Terrain Requirements
│
├── LORE & RELATIONSHIPS
│   ├── Faction (if applicable)
│   ├── Related Creatures (allies, enemies, variants)
│   ├── Quest Involvement (if any)
│   ├── Story Significance
│   ├── World Role/Purpose
│   └── Historical Context
│
└── TAXONOMY
    ├── Race/Species (Orc, Slime, Humanoid, Beast, etc.)
    ├── Subtype (e.g., Fire Orc, Poison Slime)
    ├── Bestiarity Category (what player category does killing count toward)
    └── Tags (keywords for searching/filtering)
```

**Current Implementation Examples**:
- Gaklorr: Boss | Type=Orc | Health=50 | Damage=5-10 | Loot: 1-3x Copper Coin, 1-2x Silver Coin
- Green Slime: Normal | Type=Slime | Health=5 | Damage=1 | Loot: Copper Coin, Silver Coin | Immunity: Poison

---

### ITEM Entity Model

```
ITEM
├── IDENTITY
│   ├── Name (string, unique)
│   ├── Item Type (enum: weapon, armor, accessory, consumable, material, quest)
│   ├── Subtype (weapon: melee/ranged; armor: head/body/legs; etc.)
│   ├── Rarity (enum: common, uncommon, rare, epic, legendary)
│   ├── Tier/Level (integer, indicates progression value)
│   └── Description (text, 100-300 words with flavor)
│
├── VISUALS
│   ├── Icon Image (small, for inventory)
│   ├── Equipped Image (how it looks on character)
│   ├── Detailed Model/Screenshot
│   ├── Color Scheme
│   └── Visual Effects (if applicable)
│
├── STATS & EFFECTS
│   ├── Primary Stat (damage, defense, etc.)
│   ├── Secondary Stats (crit chance, attack speed, etc.)
│   ├── Elemental Bonuses (fire +5, poison +3, etc.)
│   ├── Special Bonuses (movespeed +10%, experience +5%, etc.)
│   ├── Negative Effects (if cursed/balanced)
│   ├── Passive Abilities (always active bonuses)
│   └── Active Abilities (special attacks/effects)
│
├── REQUIREMENTS
│   ├── Level Requirement (minimum level to use)
│   ├── Class Requirement (if restricted)
│   ├── Stat Requirements (strength, dexterity, intelligence)
│   ├── Skill Requirements (proficiency levels needed)
│   └── Quest Requirements (must complete quest)
│
├── ACQUISITION
│   ├── Drop Sources (which creatures drop this item)
│   │   ├── Creature Name (rarity %)
│   │   └── Specific Conditions (only from boss, etc.)
│   ├── NPC Vendors (who sells it)
│   │   ├── NPC Name
│   │   ├── Price
│   │   └── Availability Conditions
│   ├── Quest Rewards (which quests reward it)
│   ├── Crafting (recipes that create it)
│   ├── Treasure Chests (which locations contain it)
│   └── Unique Acquisition (limited time, event, etc.)
│
├── VALUE & TRADE
│   ├── Sell Price (base vendor value)
│   ├── Market Value (player-to-player trading)
│   ├── Crafting Cost (materials + currency to make)
│   ├── Upgrade Cost (if upgradeable)
│   └── Salvage Value (if can be disassembled)
│
├── DURABILITY & MAINTENANCE
│   ├── Durability Value (if items degrade)
│   ├── Repair Cost
│   ├── Enchantment Level (if upgradeable)
│   └── Breakage Conditions
│
├── RELATIONSHIPS
│   ├── Set Items (if part of a set)
│   ├── Upgrade Path (what it upgrades to)
│   ├── Prerequisite Item (what you need before this)
│   ├── Related Items (similar or synergistic)
│   └── Quest Chain (if part of quest progression)
│
├── LORE & FLAVOR
│   ├── Item History
│   ├── Legendary Status
│   ├── Creator/Origin
│   ├── Cursed Status (if applicable)
│   └── Unique Story
│
└── TAGS & CATEGORIZATION
    ├── Search Tags (keywords)
    ├── Filters (for browsing UI)
    ├── Expansion/Update (when added)
    └── Availability (available, limited, seasonal)
```

**Current Implementation Examples**:
- Copper Coin: Type=Currency | Rarity=Common | Acquisition=All creatures drop | Value=trade currency
- Silver Coin: Type=Currency | Rarity=Uncommon | Acquisition=Higher-level creatures | Value=higher trade value

---

### NPC Entity Model

```
NPC
├── IDENTITY
│   ├── Name (string, unique)
│   ├── Title/Role (Merchant, Quest Giver, Blacksmith, etc.)
│   ├── Faction/Allegiance (if applicable)
│   ├── Personality Type (friendly, neutral, hostile, mysterious)
│   ├── Race/Species
│   └── Description (backstory, 200-400 words)
│
├── VISUALS
│   ├── Portrait Image
│   ├── In-Game Model/Sprite
│   ├── Appearance Description
│   └── Outfit/Equipment Details
│
├── LOCATION & INTERACTION
│   ├── Primary Location (map zone)
│   ├── Coordinates/Precise Location
│   ├── Available Hours (if time-dependent)
│   ├── Interaction Radius
│   └── Accessibility Notes
│
├── COMMERCE/TRADING
│   ├── Sells Items (list of items with prices)
│   │   ├── Item Name
│   │   ├── Price (currency)
│   │   ├── Availability (in stock, restocks)
│   │   └── Quantity Limits
│   ├── Buys Items (what they purchase for)
│   │   ├── Item Category
│   │   ├── Price Paid (% of normal value)
│   │   └── Conditions
│   ├── Special Trades (item for item)
│   └── Trading Requirements
│
├── QUESTS & OBJECTIVES
│   ├── Quests Offered (list)
│   │   ├── Quest Name
│   │   ├── Level Requirement
│   │   ├── Reward Type
│   │   └── Objective Summary
│   ├── Quest Chain Position (if part of chain)
│   ├── Related NPCs (for chain quests)
│   └── Completion Requirements
│
├── DIALOGUE & INTERACTION
│   ├── Greeting Dialogue
│   ├── Quest Offer Dialogue
│   ├── Conversation Options (if branching)
│   ├── Special Dialogue Conditions
│   ├── Key Phrases/Topics
│   └── Lore Dialogue Snippets
│
├── RELATIONSHIP SYSTEM
│   ├── Reputation Affects (quest availability, prices, dialogue)
│   ├── Faction Standing (if part of faction quests)
│   ├── Special Requirements (friendship level, item ownership)
│   └── Betrayal/Hostile Conditions
│
├── SERVICES
│   ├── Skill Training (if trainer)
│   ├── Item Repair (blacksmith, etc.)
│   ├── Enchanting/Crafting (if applicable)
│   ├── Banking/Storage (if merchant hub)
│   ├── Fast Travel (if applicable)
│   └── Resurrection (if applicable)
│
├── LORE & WORLD ROLE
│   ├── Character Arc
│   ├── Relationship to Story
│   ├── Connected Locations
│   ├── Related Creatures
│   └── Unique Features
│
└── INTERACTION DATA
    ├── Schedule (daily routine, if any)
    ├── Quest Restrictions (mutually exclusive, etc.)
    ├── Enemy Conditions (becomes hostile if)
    └── Special Events (appearances in storyline)
```

**Current Implementation Example**:
- Red Mushroom: Type=NPC | Location=Unknown | Commerce=Unknown | Role=Unknown (needs expansion)

---

### OBSTACLE/INTERACTABLE Entity Model

```
OBSTACLE
├── IDENTITY
│   ├── Name (string, unique)
│   ├── Type (enum: container, node, barrier, trap, destructible, lever)
│   ├── Subtype (chest, ore vein, bush, door, pressure plate, etc.)
│   ├── Description (text, 100-200 words)
│   └── Difficulty/Tier
│
├── VISUALS
│   ├── Sprite/Model Image
│   ├── State Variations (closed/open, intact/broken)
│   ├── Environmental Context
│   └── Size/Scale
│
├── INTERACTION MECHANICS
│   ├── Interaction Type (loot, harvest, break, unlock, activate)
│   ├── Requirements to Interact
│   │   ├── Level Requirement
│   │   ├── Item Requirements (key, tool, etc.)
│   │   ├── Skill Requirements (lockpicking, mining, etc.)
│   │   └── Condition Requirements (time of day, weather)
│   ├── Interaction Time (instant, 1 second, 5 seconds, etc.)
│   ├── Reusable (one-time only or respawnable)
│   └── Respawn Time (if applicable)
│
├── LOOT/REWARDS
│   ├── Loot Table (containers)
│   │   ├── Item Name
│   │   ├── Quantity Range
│   │   └── Drop Rate %
│   ├── Harvest Yield (nodes, vegetation)
│   │   ├── Resource Type
│   │   ├── Quantity Range
│   │   └── Quality Variation
│   ├── Special Rewards (quest items, unique drops)
│   └── Experience/Currency Reward
│
├── LOCATION & DISTRIBUTION
│   ├── Primary Spawn Location
│   ├── Secondary Locations
│   ├── Map Zone(s)
│   ├── Density (how many in area)
│   ├── Spawn Conditions
│   └── Respawn Rate
│
├── EFFECTS & HAZARDS
│   ├── Damage on Interaction (traps)
│   ├── Status Effects (poison, freeze, etc.)
│   ├── Environmental Effects (wind, sound)
│   ├── Chain Reactions (triggers other obstacles)
│   └── Disarm/Avoid Conditions
│
├── LORE & PURPOSE
│   ├── Story Role
│   ├── Environmental Context
│   ├── Crafting Significance
│   └── World Building
│
└── GAMEPLAY TAGS
    ├── Resource Type (ore, herb, wood, etc.)
    ├── Difficulty Tags
    └── Search Keywords
```

**Current Implementation Examples**:
- Bushes: Type=vegetation | Harvest=Interaction | Rewards=Unknown
- Treasure Chests: Type=container | Loot=Unknown | Accessibility=Unknown
- Mining Veins: Type=node | Harvest=Mining skill | Resource=Various ores

---

## 3. INFORMATION HIERARCHY & NAVIGATION

### Sitemap Structure

```
Nightmares Wiki
│
├── Home Page
│   ├── Featured Content
│   ├── Latest Updates
│   ├── Quick Links
│   ├── Search
│   └── Statistics/Progress
│
├── CREATURES
│   ├── All Creatures (list/grid view with filtering)
│   │   ├── Filter by Type (Normal, Elite, Boss, etc.)
│   │   ├── Filter by Difficulty/Level
│   │   ├── Filter by Location
│   │   ├── Filter by Race (Orc, Slime, Humanoid, etc.)
│   │   └── Sort Options (alphabetical, difficulty, location)
│   │
│   ├── Creature Categories
│   │   ├── Normal Enemies (browsable list)
│   │   ├── Elite Enemies (browsable list)
│   │   ├── Bosses (browsable list with boss tier)
│   │   ├── Mini-Bosses (browsable list)
│   │   └── Unique Creatures (browsable list)
│   │
│   ├── By Location (zone-based browsing)
│   │   ├── Zone 1 (creatures in this zone)
│   │   ├── Zone 2
│   │   └── ...
│   │
│   ├── By Race/Species
│   │   ├── Orcs
│   │   ├── Slimes
│   │   ├── Humanoids
│   │   └── Beasts
│   │
│   ├── By Difficulty Tier
│   │   ├── Level 1-10
│   │   ├── Level 11-20
│   │   └── ...
│   │
│   ├── Bestiary Progress (taxonomy guide)
│   │   └── Track what's been documented
│   │
│   └── Individual Creature Pages
│       └── [Creature Name] (detailed stats, loot, location, lore)
│
├── ITEMS
│   ├── All Items (master list with filtering)
│   │   ├── Filter by Type (Weapon, Armor, Accessory, etc.)
│   │   ├── Filter by Rarity
│   │   ├── Filter by Acquisition (Drops, Vendor, Quest, Craft)
│   │   ├── Filter by Requirements (Level, Class, etc.)
│   │   └── Sort Options (alphabetical, tier, rarity)
│   │
│   ├── Weapons
│   │   ├── All Weapons
│   │   ├── Melee Weapons
│   │   │   ├── Swords
│   │   │   ├── Axes
│   │   │   ├── Hammers
│   │   │   └── Spears
│   │   ├── Ranged Weapons
│   │   │   ├── Bows
│   │   │   ├── Crossbows
│   │   │   └── Thrown Weapons
│   │   └── [Individual Weapon Pages]
│   │
│   ├── Armor
│   │   ├── All Armor
│   │   ├── Head Armor (Helmets)
│   │   ├── Body Armor (Chests)
│   │   ├── Leg Armor (Legs)
│   │   ├── Full Sets (armor combinations)
│   │   └── [Individual Armor Pages]
│   │
│   ├── Accessories
│   │   ├── Rings
│   │   ├── Amulets
│   │   ├── Cloaks
│   │   └── [Individual Accessory Pages]
│   │
│   ├── Consumables
│   │   ├── Potions (by effect)
│   │   ├── Food/Drink
│   │   ├── Buffs/Scrolls
│   │   └── [Individual Pages]
│   │
│   ├── Materials/Crafting
│   │   ├── Ores
│   │   ├── Herbs
│   │   ├── Wood/Fibers
│   │   ├── Special Materials
│   │   └── [Individual Pages]
│   │
│   ├── Quest Items (special category)
│   │
│   ├── Currency
│   │   ├── Coins (copper, silver, gold, etc.)
│   │   └── Special Currency
│   │
│   ├── By Acquisition Method
│   │   ├── Creature Drops
│   │   ├── NPC Vendors
│   │   ├── Quest Rewards
│   │   ├── Crafting Recipes
│   │   └── Special Locations
│   │
│   ├── Equipment Tier Guide
│   │   └── How items progress
│   │
│   └── [Individual Item Pages]
│       └── [Item Name] (stats, acquisition, requirements, locations)
│
├── NPCs
│   ├── All NPCs (browsable list)
│   │   ├── Filter by Role (Merchant, Quest Giver, etc.)
│   │   ├── Filter by Location
│   │   ├── Filter by Faction
│   │   └── Sort Options
│   │
│   ├── By Role
│   │   ├── Merchants
│   │   ├── Quest Givers
│   │   ├── Trainers
│   │   ├── Story NPCs
│   │   └── Service Providers
│   │
│   ├── By Location
│   │   ├── Town 1 (NPCs here)
│   │   ├── Town 2
│   │   └── ...
│   │
│   ├── By Faction (if applicable)
│   │   ├── Faction 1
│   │   └── ...
│   │
│   ├── Quest Hub
│   │   ├── Available Quests
│   │   ├── Quest Chains
│   │   └── Completed Quests Tracker
│   │
│   └── [Individual NPC Pages]
│       └── [NPC Name] (role, location, trades, quests, dialogue)
│
├── OBSTACLES & INTERACTABLES
│   ├── All Obstacles (browsable list)
│   │   ├── Filter by Type
│   │   ├── Filter by Location
│   │   ├── Filter by Interaction Type
│   │   └── Sort Options
│   │
│   ├── By Type
│   │   ├── Containers (chests, barrels)
│   │   ├── Nodes (ore veins, herb patches)
│   │   ├── Vegetation (bushes, trees)
│   │   ├── Barriers (doors, walls)
│   │   ├── Traps (hazards)
│   │   └── Destructibles (breakables)
│   │
│   ├── By Location
│   │   ├── Zone 1 (obstacles here)
│   │   └── ...
│   │
│   ├── Resource Gathering Guide
│   │   ├── Best Farming Locations
│   │   ├── Resource Distribution
│   │   └── Respawn Rates
│   │
│   └── [Individual Obstacle Pages]
│       └── [Obstacle Name] (interaction, rewards, location)
│
├── GUIDES & RESOURCES
│   ├── Getting Started
│   │   ├── New Player Guide
│   │   ├── Basic Controls
│   │   ├── Game Mechanics 101
│   │   └── First Steps
│   │
│   ├── Gameplay Guides
│   │   ├── Combat Guide
│   │   ├── Farming/Grinding Guide
│   │   ├── Leveling Guide
│   │   ├── Crafting Guide
│   │   ├── Trading Guide
│   │   └── Questing Guide
│   │
│   ├── Build Guides
│   │   ├── Warrior Builds
│   │   ├── Rogue Builds
│   │   ├── Mage Builds
│   │   └── Hybrid Builds
│   │
│   ├── Location Guides
│   │   ├── Zone 1 (tips, strategy)
│   │   ├── Zone 2
│   │   └── Boss Fight Strategies
│   │
│   ├── Lore & World
│   │   ├── World History
│   │   ├── Factions Explained
│   │   ├── Character Profiles
│   │   └── Timeline
│   │
│   ├── FAQ
│   │   └── Common Questions
│   │
│   └── Community Content
│       ├── User Guides
│       ├── Strategies
│       └── Tips & Tricks
│
├── TOOLS & UTILITIES
│   ├── Search (global search with filters)
│   ├── Advanced Search
│   ├── Item Finder
│   │   └── "Where to get [Item]?"
│   ├── Creature Drops Checker
│   │   └── "What does [Creature] drop?"
│   ├── Quest Tracker
│   ├── Build Planner
│   ├── Wiki Progress Tracker
│   └── API/Data Export
│
├── About & Community
│   ├── About This Wiki
│   ├── Contributors
│   ├── Changelog/Updates
│   ├── Contact/Feedback
│   └── Community Guidelines
│
└── Account/Administration (if applicable)
    ├── Login
    ├── User Profile
    ├── Edit History
    ├── Content Management
    └── Admin Panel
```

### Navigation Patterns & Cross-Linking Strategy

**Cross-Linking Rules**:
1. **Creatures ↔ Items**: Each creature page links to its drop table items; each item links back to which creatures drop it
2. **Items ↔ NPCs**: Each NPC page lists what they sell; items list which NPCs sell them
3. **Creatures ↔ Locations**: Creature pages show spawn locations; location pages list creatures found there
4. **Quests ↔ NPCs & Items**: Quest pages reference the NPC who gives it and item rewards
5. **Guides → Entity Pages**: Guides link to specific creatures, items, NPCs for detailed info

**Primary Browsing Paths**:
- **Path 1: Creature Hunting** → Find Creature → View Stats/Loot → Find Related Items → Discover Drop Rates
- **Path 2: Item Acquisition** → Find Item → View Acquisition Methods → Learn Creatures/NPCs → Plan Route
- **Path 3: Quest Completion** → Find Quest → Learn Objectives → Find Related Creatures/Items → Check NPC
- **Path 4: Location Exploration** → View Zone → See Creatures → See Obstacles → See NPCs → See Items
- **Path 5: Farming/Grinding** → Goal (item/levels) → Find Creatures → Check Drops → Optimize Route

**Search & Discovery Facets**:
- Type (What kind of thing is this?)
- Rarity (How rare is it?)
- Level/Difficulty (How hard is it?)
- Location (Where do I find it?)
- Acquisition (How do I get it?)
- Purpose (What is it for?)
- Tags (Keywords for discovery)

### URL Structure

```
/creatures
  /creatures/all                      All creatures list
  /creatures/normal                   Normal enemies
  /creatures/elite                    Elite enemies
  /creatures/bosses                   Boss creatures
  /creatures/[creature-name]          Individual creature detail

/items
  /items/all                          All items list
  /items/weapons                      All weapons
  /items/weapons/melee                Melee weapons
  /items/weapons/ranged               Ranged weapons
  /items/armor                        All armor
  /items/armor/helmets                Head armor
  /items/armor/chests                 Body armor
  /items/armor/legs                   Leg armor
  /items/accessories                  All accessories
  /items/consumables                  Consumable items
  /items/materials                    Crafting materials
  /items/currency                     Currency items
  /items/[item-name]                  Individual item detail

/npcs
  /npcs/all                           All NPCs
  /npcs/merchants                     Merchant NPCs
  /npcs/quest-givers                  Quest giver NPCs
  /npcs/[npc-name]                    Individual NPC detail

/obstacles
  /obstacles/all                      All obstacles
  /obstacles/containers               Container obstacles
  /obstacles/nodes                    Resource nodes
  /obstacles/[obstacle-name]          Individual obstacle detail

/guides
  /guides/all                         All guides
  /guides/getting-started             Beginner guides
  /guides/combat                      Combat guides
  /guides/farming                     Farming guides
  /guides/[guide-name]                Individual guide

/search
  /search?q=[query]                   Global search
  /search/advanced                    Advanced search

/locations
  /locations/[zone-name]              Zone detail and entities
```

---

## 4. FEATURE REQUIREMENTS & USER STORIES

### Core Features

#### Feature: Browse Creatures by Type/Difficulty
**User Story**: "As a player, I want to browse creatures filtered by type (normal, elite, boss) and difficulty level so I can find appropriate enemies to fight."

**Requirements**:
- [ ] Create creature listing page with filtering sidebar
- [ ] Implement type filter (normal, elite, boss, miniboss, unique)
- [ ] Implement difficulty range filter (level 1-10, 11-20, etc.)
- [ ] Implement location filter (show creatures by zone)
- [ ] Implement race/type filter (Orc, Slime, Humanoid, Beast)
- [ ] Support multiple active filters simultaneously
- [ ] Show creature count for each filter
- [ ] Display creatures in grid/list view toggle
- [ ] Show creature image, name, type, difficulty level in preview
- [ ] Sorting options (alphabetical, difficulty, location)
- [ ] "Clear filters" button to reset

**Acceptance Criteria**:
- [ ] All creatures are categorized by type
- [ ] Filters reduce result set accurately
- [ ] UI responds in <100ms to filter changes
- [ ] At least 50+ creatures documented

---

#### Feature: Global Search
**User Story**: "As a wiki user, I want to search for any creature, item, NPC, or guide by name/keyword so I can quickly find information."

**Requirements**:
- [ ] Implement global search across all content types
- [ ] Search box visible on every page (header)
- [ ] Real-time search suggestions as user types
- [ ] Search results grouped by content type (creatures, items, npcs, guides)
- [ ] Results show thumbnail, name, type, brief description
- [ ] Support partial name matching and typo tolerance
- [ ] Advanced search filters (type, rarity, location)
- [ ] Search history (last 5 searches)
- [ ] No results handling with suggestions

**Acceptance Criteria**:
- [ ] Can find any documented entity by name
- [ ] Search returns results in <200ms
- [ ] Suggestions appear while typing
- [ ] At least 100+ entities are searchable
- [ ] Support for special characters and quotes

---

#### Feature: Item Details & Acquisition Paths
**User Story**: "As a player, I want to see detailed information about items including where to get them so I can plan my character progression."

**Requirements**:
- [ ] Item detail pages show all stats/bonuses
- [ ] Display acquisition methods in priority order (drops, vendors, quests, crafting)
- [ ] For creature drops: show creature name, image, drop rate, rarity
- [ ] For vendor sales: show NPC name, price, availability
- [ ] For quest rewards: show quest name, level, objectives
- [ ] For crafting: show recipe, materials needed, crafting cost
- [ ] Highlight "easiest" way to obtain in prominent UI
- [ ] Show all creatures that drop this item
- [ ] Show all NPCs that sell this item
- [ ] Link to related/similar items
- [ ] Show requirements (level, class, stats) prominently

**Acceptance Criteria**:
- [ ] Every item has at least 1 acquisition method documented
- [ ] Drop rates are accurate (or marked "unknown")
- [ ] Item pages include usage examples/builds
- [ ] Can trace item → source → location

---

#### Feature: Filter Items by Type/Stats/Requirements
**User Story**: "As a player building a character, I want to filter items by type, stats, and requirements so I can find gear that fits my build."

**Requirements**:
- [ ] Implement multi-faceted filtering system
- [ ] Filter by item type (weapon, armor, consumable, etc.)
- [ ] Filter by subtype (melee weapon, helmet, potion, etc.)
- [ ] Filter by rarity (common, uncommon, rare, epic, legendary)
- [ ] Filter by stat thresholds (min damage > 10, armor > 5, etc.)
- [ ] Filter by level requirement (show items for level 1-20, etc.)
- [ ] Filter by element/attribute (fire items, poison items, etc.)
- [ ] Filter by set membership (show armor sets)
- [ ] Filter by acquisition method
- [ ] Filter by class/profession requirements
- [ ] Combine multiple filters for refined searches
- [ ] Save favorite filter combinations
- [ ] Show item count for each filter

**Acceptance Criteria**:
- [ ] All filterable attributes are documented for 90%+ items
- [ ] Filters combine without conflicts
- [ ] Filter changes instant (<100ms)
- [ ] Saved filters persist across sessions
- [ ] Advanced filter help available

---

#### Feature: Creature → Items Relationship Mapping
**User Story**: "As a player farming for specific items, I want to see which creatures drop which items so I can optimize my farming route."

**Requirements**:
- [ ] Create relationship database mapping creatures to item drops
- [ ] Show on creature page: all items they drop with rates
- [ ] Show on item page: all creatures that drop it with rates
- [ ] Create "where to find item" search tool
- [ ] Create "what does this creature drop" lookup
- [ ] Sort creatures by drop rate (highest first)
- [ ] Group drops by rarity
- [ ] Show guaranteed vs. random drops separately
- [ ] Calculate farming efficiency (best creature for item)
- [ ] Show alternative acquisition methods for comparison

**Acceptance Criteria**:
- [ ] Every creature drop is documented
- [ ] Drop rates are accurate or marked "estimate"
- [ ] Can find any item via creature drops
- [ ] Creature-item map is bidirectional (links work both ways)
- [ ] At least 100+ item-creature relationships documented

---

#### Feature: NPC Trade Options & Inventory
**User Story**: "As a player, I want to see what NPCs trade so I can buy items or complete quests."

**Requirements**:
- [ ] Create NPC detail pages with complete trade inventory
- [ ] List all items NPC buys with prices paid
- [ ] List all items NPC sells with prices
- [ ] Show availability/stock (in stock, out of stock, limited)
- [ ] Show any special trade requirements (friendship level, quest completion)
- [ ] Link each trade item to item detail page
- [ ] Show NPC location with map coordinates
- [ ] Show NPC quest offerings
- [ ] Show NPC dialogue/lore snippets
- [ ] Track NPC availability (time of day, seasonal)
- [ ] Show trading margin (profit if buying/selling)

**Acceptance Criteria**:
- [ ] Every NPC has trade inventory documented
- [ ] All prices are consistent and tracked
- [ ] NPC pages include dialogue snippets
- [ ] Can plan trading routes
- [ ] At least 20+ NPCs documented

---

#### Feature: Browse Guides & Related Content
**User Story**: "As a new player, I want to access guides and tips about gameplay so I can learn the game mechanics and strategies."

**Requirements**:
- [ ] Create guides section with categorized content
- [ ] Getting started/beginner guides
- [ ] Combat and strategy guides
- [ ] Farming and grinding guides
- [ ] Build/character guides
- [ ] Lore and world-building content
- [ ] Location guides with maps and tips
- [ ] FAQ section
- [ ] Link guides to relevant entities (creatures, items, npcs)
- [ ] Create visual guides (infographics, diagrams)
- [ ] Community contributions section
- [ ] Guide versioning/update tracking

**Acceptance Criteria**:
- [ ] At least 15+ guides created
- [ ] Guides link to related entities
- [ ] New player can complete tutorial guide
- [ ] Guides cover major gameplay topics
- [ ] Community can contribute guides

---

### Secondary Features

#### Feature: Advanced Sorting & Comparisons
- Sort items by damage, armor, level requirement, rarity
- Compare multiple items side-by-side
- Build comparison tool (compare two character builds)
- Creature difficulty ranking

#### Feature: Location/Zone Maps
- Interactive maps showing creature spawns
- Obstacle/treasure locations on maps
- NPC locations with store indicators
- Difficulty heat maps (where hard enemies spawn)

#### Feature: Quest Chains & Progression Tracking
- Visual quest chain diagrams
- Quest progress tracker
- Recommended quest order
- Quest reward calculator

#### Feature: Build Planner & Character Creator
- Drag-and-drop build creator
- See stat totals with different item combinations
- Save/share builds
- Theory crafting tools

#### Feature: Lore & Story Index
- Timeline of world events
- Character relationship diagrams
- Faction reputation trackers
- Story progression guide

#### Feature: Crafting System Documentation
- Craft recipes (inputs → outputs)
- Crafting material sources
- Recipe prerequisites
- Craft difficulty ratings

#### Feature: Wiki Progress Dashboard
- Coverage statistics (how many entities documented)
- What still needs documentation
- Contributor stats
- Recent activity feed

---

## 5. CONTENT ORGANIZATION STRATEGY

### Creatures Organization

**Primary Organization by Type** (first-level categorization):
```
Creatures
├── Normal Enemies (common spawns, no special drops)
├── Elite Enemies (enhanced versions, rarer drops)
├── Bosses (unique, story-significant, special drops)
├── Mini-Bosses (challenging but not full boss tier)
└── Unique Creatures (one-of-a-kind, special properties)
```

**Secondary Organization Options** (for browsing):
1. **By Difficulty Level** (Level 1-10, 11-20, 21-30, etc.)
2. **By Location/Zone** (where they spawn)
3. **By Race/Species** (Orcs, Slimes, Humanoids, Beasts, Dragons, Undead, etc.)
4. **By Combat Role** (melee attackers, ranged, casters, healers, tanks)
5. **By Rarity** (common spawn, uncommon, rare, legendary)

**Recommended Default View**: 
- Default to Type view (Normal → Elite → Boss) with secondary filters for difficulty, location, race
- Allow users to switch primary organization via "View by" dropdown
- Remember user preference

**Data Quality Rules**:
- Every creature must have at minimum: Name, Type, Basic Stats, Loot Table, Location
- Creatures must be grouped into Bestiarity categories for progress tracking
- Boss creatures should have full lore/background
- Mark incomplete creature pages as "Stub" or "Needs Expansion"

---

### Items Organization

**Primary Organization by Type** (what the item IS):
```
Items
├── WEAPONS
│   ├── Melee Weapons
│   │   ├── Swords
│   │   ├── Axes
│   │   ├── Hammers
│   │   ├── Maces
│   │   └── Spears
│   └── Ranged Weapons
│       ├── Bows
│       ├── Crossbows
│       └── Thrown Weapons
├── ARMOR
│   ├── Head Armor (Helmets)
│   ├── Body Armor (Chests)
│   └── Leg Armor (Legs)
├── ACCESSORIES
│   ├── Rings
│   ├── Amulets
│   ├── Cloaks
│   └── Other
├── CONSUMABLES
│   ├── Potions (by effect)
│   ├── Food/Drink
│   └── Scrolls/Buff Items
├── MATERIALS/CRAFTING
│   ├── Ores
│   ├── Herbs
│   ├── Wood/Fibers
│   └── Special Materials
├── CURRENCY
│   ├── Coins (copper, silver, gold)
│   └── Special Currency
├── QUEST ITEMS
│   └── Story/Quest specific items
└── OTHER
    └── Unique/Miscellaneous items
```

**Secondary Organization Options** (filtering layer):
1. **By Rarity** (common, uncommon, rare, epic, legendary)
2. **By Tier/Level** (equipment progression: tier 1, 2, 3, etc.)
3. **By Acquisition Method** (drops, vendor, quest, craft)
4. **By Stats/Effect** (high damage, high defense, elemental bonus, speed boost)
5. **By Element** (fire, ice, poison, lightning, holy, shadow)

**Recommended Default View**:
- Default to Type organization for weapons/armor (main path)
- Allow quick switch to Rarity/Level for progression view
- Provide acquisition-method view for "how do I get X" queries

**Equipment Tier System**:
Define clear progression tiers so users understand item value:
- **Tier 0**: Starting/basic equipment (level 1-10)
- **Tier 1**: Early game (level 11-20)
- **Tier 2**: Mid game (level 21-40)
- **Tier 3**: Late game (level 41-60)
- **Tier 4**: End game (level 60+)
- **Unique/Legendary**: Outside normal progression

Each tier should have recommended weapon/armor combinations.

**Armor Set Organization**:
- Organize complete armor sets (Head + Body + Legs combos)
- Show armor set bonuses (bonus stats when wearing complete set)
- Create armor set progression paths (starting set → mid-game set → end-game set)

**Data Quality Rules**:
- Every item must have: Name, Type, Stats, At least 1 acquisition method
- Items should show usage (which creatures drop it, which NPCs sell it)
- Rarity must be consistently applied across tiers
- Similar items should link to each other

---

### NPCs Organization

**Primary Organization by Role** (what they DO):
```
NPCs
├── Merchants (buy/sell items)
│   ├── General Merchants
│   ├── Weapon Vendors
│   ├── Armor Vendors
│   └── Specialty Merchants
├── Quest Givers (offer quests)
│   ├── Main Quest NPCs
│   ├── Side Quest NPCs
│   └── Daily Quest NPCs
├── Trainers (provide skills/abilities)
│   ├── Combat Trainers
│   ├── Crafting Trainers
│   └── Specialty Trainers
├── Story Characters (narrative role)
│   ├── Companions
│   ├── Antagonists
│   └── Lore-Important NPCs
├── Service Providers (special services)
│   ├── Blacksmiths (repair/enhance)
│   ├── Healers/Priests
│   ├── Bankers (storage)
│   └── Other Services
└── Neutral/Miscellaneous
    └── Other NPCs
```

**Secondary Organization Options**:
1. **By Location** (which town/zone they're in)
2. **By Faction** (if applicable, which faction they belong to)
3. **By Availability** (always available, time-dependent, event-dependent)

**Quest Organization Within NPCs**:
- Show available quests directly on NPC page
- Create quest chains showing prerequisites
- Track quest rewards (items, experience, currency)
- Link quests to required creatures/items

**Data Quality Rules**:
- Every NPC must have: Name, Role, Location, Description
- Merchants must list their full inventory with prices
- Quest givers must list all quests they offer
- Important NPCs should have dialogue/lore content
- NPC location must be precise (zone + coordinates)

---

### Obstacles/Interactables Organization

**Primary Organization by Type** (how you interact with it):
```
Obstacles
├── Containers (loot)
│   ├── Treasure Chests
│   ├── Barrels
│   ├── Urns
│   └── Other Containers
├── Nodes (harvest/gather)
│   ├── Ore Veins
│   ├── Herb Patches
│   ├── Wood Trees
│   └── Other Resource Nodes
├── Vegetation (free gathering)
│   ├── Bushes
│   ├── Flowers
│   └── Grass/Low Plants
├── Barriers (environmental)
│   ├── Doors
│   ├── Gates
│   ├── Walls
│   └── Impassable Terrain
├── Traps (hazards)
│   ├── Pressure Plates
│   ├── Spikes
│   ├── Environmental Hazards
│   └── Magical Traps
├── Destructibles (breakable)
│   ├── Barrels
│   ├── Crates
│   ├── Furniture
│   └── Other Destructibles
└── Interactables (special)
    ├── Levers/Switches
    ├── Altars
    ├── Books/Lore Objects
    └── Other Interact Objects
```

**Secondary Organization Options**:
1. **By Location** (which zone/area)
2. **By Rarity** (common, uncommon, rare)
3. **By Interaction Type** (loot, harvest, break, activate, unlock)
4. **By Reward Type** (currency, materials, items)

**Resource Gathering Guide**:
- Create dedicated guide showing best farming locations
- Map density of nodes (ore concentration)
- Show respawn times for nodes
- Recommend routes for efficient farming

**Data Quality Rules**:
- Every obstacle must have: Name, Type, Interaction method, Loot/Reward
- Loot tables should list all possible drops with rates
- Location must be precise (zone + optional coordinates)
- Respawn information should be documented
- Requirements (level, tools, skills) should be listed

---

### Guides Organization

**Primary Organization by Purpose**:
```
Guides
├── GETTING STARTED
│   ├── New Player Welcome
│   ├── Basic Controls & UI
│   ├── Game Mechanics 101
│   ├── First Steps (tutorial guide)
│   └── Leveling Tips (0-10)
├── GAMEPLAY GUIDES
│   ├── Combat & Mechanics
│   │   ├── Combat System Explained
│   │   ├── Ability System
│   │   ├── Status Effects
│   │   └── Combat Tips & Strategies
│   ├── Progression & Leveling
│   │   ├── Leveling Guide (1-20, 20-40, etc.)
│   │   ├── Experience Grinding Spots
│   │   └── Efficient Leveling Routes
│   ├── Item & Equipment Progression
│   │   ├── When to Upgrade Gear
│   │   ├── Equipment Tier Guide
│   │   └── Best Items for Your Level
│   ├── Farming & Grinding
│   │   ├── Resource Farming Locations
│   │   ├── Item Drop Farming
│   │   ├── Experience Grinding
│   │   └── Optimal Routes
│   └── Economy & Trading
│       ├── Price Guide
│       ├── Trading Tips
│       └── Money Making Guide
├── BUILD GUIDES
│   ├── Melee Warrior Builds
│   ├── Ranger/Archery Builds
│   ├── Caster/Mage Builds
│   ├── Hybrid Builds
│   ├── Solo vs Group Builds
│   └── PvP Builds (if applicable)
├── LOCATION GUIDES
│   ├── Zone 1: [Zone Name] (loot, creatures, tips)
│   ├── Zone 2: [Zone Name]
│   ├── Boss Fight Strategies
│   ├── Hidden Areas & Secrets
│   └── Fast Travel Routes
├── LORE & WORLD
│   ├── World History & Timeline
│   ├── Faction Guide & Reputation
│   ├── Character Profiles
│   ├── Lore Timeline (story progression)
│   └── Hidden Lore & Secrets
├── SPECIAL CONTENT
│   ├── Events & Limited Content
│   ├── Secret Boss Strategies
│   ├── Easter Eggs & Secrets
│   └── Challenge Runs
└── FAQ & COMMUNITY
    ├── Frequently Asked Questions
    ├── Troubleshooting
    ├── Community Tips & Tricks
    └── Speed Run Guides (if applicable)
```

**Data Quality Rules**:
- Guides should link to related wiki entities (creatures, items, NPCs)
- Include images, diagrams, or maps where helpful
- Guides should have clear progression (basics → advanced)
- Regular update guides when game changes
- Tag guides with difficulty (beginner, intermediate, expert)

---

## 6. ACCEPTANCE CRITERIA FOR COMPLETE WIKI COVERAGE

### Minimum Viable Product (MVP) Coverage

**Phase 1: Core Content** (launch-ready)
- [ ] At least 50 creatures documented
  - [ ] At least 5 bosses with full lore
  - [ ] Normal enemies (30+) with combat stats
  - [ ] Elite enemies (10+) with distinct loot
  - [ ] Clear type/difficulty categorization
- [ ] At least 100 items documented
  - [ ] All weapon types (melee & ranged)
  - [ ] All armor slots (head, body, legs)
  - [ ] Consumables & materials
  - [ ] Acquisition methods for all items
- [ ] At least 10 NPCs documented
  - [ ] All merchants and vendors
  - [ ] All quest givers
  - [ ] Trade inventory complete
- [ ] 20+ obstacles/interactables documented
- [ ] 10+ beginner guides created
- [ ] Global search functional
- [ ] All cross-links working (creature↔items, items↔NPCs)

**Phase 2: Expansion** (comprehensive coverage)
- [ ] 100+ creatures documented
- [ ] 200+ items documented
- [ ] 20+ NPCs documented
- [ ] 30+ obstacles documented
- [ ] 15+ guides (including advanced guides)
- [ ] Location maps with annotations
- [ ] Complete lore section
- [ ] Quest chain documentation

**Phase 3: Polish** (complete)
- [ ] 150+ creatures documented
- [ ] 300+ items documented
- [ ] 40+ NPCs documented
- [ ] 50+ obstacles documented
- [ ] 25+ guides
- [ ] Build planner tool
- [ ] Advanced search filters
- [ ] Complete lore index
- [ ] Crafting recipes documented
- [ ] All content cross-linked

### Data Completeness Standards

**For Each Creature**:
- [ ] Name and image
- [ ] Type (normal/elite/boss)
- [ ] Combat stats (HP, damage, armor, speed)
- [ ] At least 3 stats documented
- [ ] Loot table with drop rates
- [ ] Primary spawn location
- [ ] Description/flavor text (100+ words for bosses)
- [ ] Special abilities documented

**For Each Item**:
- [ ] Name and icon
- [ ] Type and rarity
- [ ] Primary stats (damage or defense)
- [ ] At least 1 acquisition method
- [ ] Requirements (if any)
- [ ] Description/flavor text

**For Each NPC**:
- [ ] Name and image
- [ ] Role/occupation
- [ ] Location
- [ ] Trade inventory (if merchant)
- [ ] Quests offered (if quest giver)
- [ ] Description/backstory

**For Each Obstacle**:
- [ ] Name and image
- [ ] Type and interaction method
- [ ] Loot/reward table
- [ ] Location
- [ ] Requirements (if any)

### Quality Standards

- [ ] All images optimized and consistent style
- [ ] All text professionally written (spell-checked, grammar-checked)
- [ ] No placeholder text or incomplete entries
- [ ] Consistent terminology across wiki (e.g., always "creature" or always "enemy")
- [ ] Consistent formatting for all pages (stats display, loot tables, etc.)
- [ ] All cross-links validated (no broken links)
- [ ] Search returns accurate results
- [ ] Mobile-responsive design
- [ ] Page load time < 2 seconds
- [ ] At least 90% of entities have images
- [ ] All stat numbers consistent with game
- [ ] Drop rates documented or marked "unknown"

### Navigation & Findability

- [ ] Every page has breadcrumb navigation
- [ ] Sidebar navigation present on all pages
- [ ] Search works from any page
- [ ] Entity pages have "Related Items" or "Related Creatures"
- [ ] Clear categorization/hierarchy
- [ ] Filters are intuitive and helpful
- [ ] At least 3 paths to find any entity
- [ ] Featured content updated monthly
- [ ] Site map available
- [ ] 404 pages redirect to similar content

### Accessibility & Usability

- [ ] WCAG 2.1 AA compliance
- [ ] Mobile-responsive design
- [ ] Dark mode support
- [ ] Font sizes readable (14px minimum body text)
- [ ] Color contrast sufficient (4.5:1 minimum)
- [ ] Keyboard navigation supported
- [ ] Screen reader compatible
- [ ] Alternative text for all images
- [ ] Proper heading hierarchy
- [ ] Forms clearly labeled

### Technical Standards

- [ ] Page load time < 2 seconds
- [ ] 99.9% uptime
- [ ] Database backups automated
- [ ] Version control (Git) for all content
- [ ] Content edit history tracked
- [ ] API available for external tools
- [ ] Data export format available
- [ ] Performance metrics monitored
- [ ] Security tested
- [ ] Browser compatibility (Chrome, Firefox, Safari, Edge)

### Community & Maintenance

- [ ] Contribution guidelines documented
- [ ] Edit history visible for all pages
- [ ] Contributor list maintained
- [ ] Update log/changelog current
- [ ] Community feedback mechanism in place
- [ ] Issue reporting system
- [ ] Content moderation policy
- [ ] Regular updates (at least monthly)
- [ ] Discord/community links
- [ ] Contact information available

---

## 7. IMPLEMENTATION ROADMAP

### Phase 1: Foundation (Weeks 1-2)
- [ ] Design database schema (entities, relationships)
- [ ] Set up wiki platform (CMS or static site generator)
- [ ] Create page templates (creature, item, NPC, obstacle, guide)
- [ ] Implement global search
- [ ] Set up navigation structure
- [ ] Create 50 creature entries (data entry)
- [ ] Create 100 item entries (data entry)
- [ ] Create 10 NPC entries
- [ ] Create 20 obstacle entries
- [ ] Create beginner guides (5+)

### Phase 2: Cross-Linking (Weeks 2-3)
- [ ] Link creatures to item drops
- [ ] Link items to creature drops and NPC vendors
- [ ] Link NPCs to their trade inventory
- [ ] Create "How to get X" lookup system
- [ ] Create creature → items → NPCs browsing paths
- [ ] Implement advanced filtering
- [ ] Add guides and strategies

### Phase 3: Polish & Optimization (Week 4)
- [ ] Add images to all entries
- [ ] Write full descriptions/lore
- [ ] Test all cross-links
- [ ] Optimize page load times
- [ ] Implement analytics
- [ ] Create site maps and feeds
- [ ] Set up contribution system
- [ ] Launch MVP

### Subsequent Phases: Expansion
- [ ] Increase creature count to 100+
- [ ] Add location maps
- [ ] Create lore index
- [ ] Add build planner tool
- [ ] Expand guide library
- [ ] Create community section

---

## 8. SUCCESS METRICS

### User Engagement
- [ ] Average session duration > 5 minutes
- [ ] Pages per session > 3
- [ ] Return visitor rate > 40%
- [ ] Search queries logged and analyzed
- [ ] 90%+ of user searches return relevant results

### Content Coverage
- [ ] 95%+ of creatures documented
- [ ] 95%+ of items documented
- [ ] 90%+ of items have acquisition paths
- [ ] 100% of creatures have loot tables
- [ ] All cross-links working

### Technical Performance
- [ ] Page load time < 2 seconds (p95)
- [ ] Search response < 200ms
- [ ] 99.9% uptime
- [ ] Mobile traffic increases 20% month-over-month
- [ ] Bounce rate < 30%

### Community Growth
- [ ] 1,000+ unique visitors per month
- [ ] 5+ community contributors
- [ ] 100+ social shares
- [ ] Featured in game community forums
- [ ] Positive community feedback

---

## 9. CONCLUSION

This Information Architecture document provides comprehensive guidance for designing and building the Nightmares Wiki. It defines:

1. **Content Inventory** - What exists and what needs to be documented
2. **Data Models** - Exactly what information each entity type should contain
3. **Navigation Structure** - How users will browse and discover content
4. **Features & User Stories** - What functionality users need
5. **Organization Strategy** - How to categorize and organize all content
6. **Acceptance Criteria** - What "complete" looks like
7. **Implementation Roadmap** - How to build it in phases

By following this IA, the design and development teams can ensure:
- Consistency across the entire wiki
- Complete coverage of game content
- Intuitive navigation and discovery
- Strong cross-linking and relationships
- Professional quality and user experience

The wiki should serve as the authoritative reference for all Nightmares game content, enabling players to find any information they need quickly and efficiently.

---

**Document Version**: 1.0  
**Last Updated**: 2024  
**Owner**: Product Owner (Nightmares Wiki Project)  
**Status**: Ready for Design & Development Handoff

