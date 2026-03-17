# Nightmares Wiki - Detail Page Wireframes

## WIREFRAME 1: CREATURE DETAIL PAGE (Boss/Enemy)

### Desktop View (1200px+)

```
┌──────────────────────────────────────────────────────────────────────────────┐
│                              HEADER / NAV BAR                                │
├──────────┬──────────────────────────────────────┬──────────────────────────┤
│          │ Home > Creatures > Bosses > Demon   │                          │
│  SIDEBAR │                                     │   TABLE OF               │
│          │                                     │   CONTENTS               │
│  🏠 Home │ ╔════════════════════════════════╗ │  ═════════════════       │
│          │ ║   [Creature Image: 280px]      ║ │  📑 Overview             │
│  👹      │ ║                                ║ │  📊 Statistics            │
│  Creatures
│          │ ║        Demon Avatar            ║ │  🎯 Behavior             │
│  ├─ Bosses
│          │ ║                                ║ │  💰 Loot Table           │
│  ├─ Mobs │ ╚════════════════════════════════╝ │  🗺️  Habitat              │
│  ├─ Summons
│          │                                     │  🖼️  Gallery              │
│  📦      │ ### Demon                          │  🔗 Related               │
│  Items   │                                     │  ═════════════════       │
│          │ **Type:** Boss                      │                          │
│  👤      │ **Difficulty:** ★★★★★              │                          │
│  NPCs    │ **Location:** Castle Depths, Level 12│                        │
│          │ **Summon Cost:** 300 Mana           │                          │
│  🗺️       │                                     │                          │
│  World   │ ---                                │                          │
│          │                                     │                          │
│  🔍      │ ## Overview                        │                          │
│  Search  │                                     │                          │
│          │ The Demon is one of the most feared │                          │
│          │ creatures in the Nightmares realm.  │                          │
│          │ Known for its devastating dark magic│                          │
│          │ and melee attacks, this boss-level  │                          │
│          │ enemy should only be faced by high- │                          │
│          │ level adventurers with proper      │                          │
│          │ preparation and group support.     │                          │
│          │                                     │                          │
│          │ ## Statistics                      │                          │
│          │                                     │                          │
│          │ ┌──────────────────┬─────────────┐ │                          │
│          │ │ Hit Points       │ 220         │ │                          │
│          │ ├──────────────────┼─────────────┤ │                          │
│          │ │ Experience       │ 440         │ │                          │
│          │ ├──────────────────┼─────────────┤ │                          │
│          │ │ Armor Rating     │ 13          │ │                          │
│          │ ├──────────────────┼─────────────┤ │                          │
│          │ │ Melee Damage     │ 100-210     │ │                          │
│          │ ├──────────────────┼─────────────┤ │                          │
│          │ │ Ranged Damage    │ 80-140      │ │                          │
│          │ ├──────────────────┼─────────────┤ │                          │
│          │ │ Magic Damage     │ 60-120      │ │                          │
│          │ ├──────────────────┼─────────────┤ │                          │
│          │ │ Attack Speed     │ 1.0x        │ │                          │
│          │ ├──────────────────┼─────────────┤ │                          │
│          │ │ Defense Speed    │ 1.2x        │ │                          │
│          │ ├──────────────────┼─────────────┤ │                          │
│          │ │ Movement Speed   │ 150         │ │                          │
│          │ ├──────────────────┼─────────────┤ │                          │
│          │ │ Magic Resist     │ 40%         │ │                          │
│          │ └──────────────────┴─────────────┘ │                          │
│          │                                     │                          │
│          │ ## Abilities & Behavior             │                          │
│          │                                     │                          │
│          │ [Tab: Combat] [Tab: Movement]      │                          │
│          │ ────────────────────────────────────│                          │
│          │                                     │                          │
│          │ **Dark Bolt** (Ranged)              │                          │
│          │ Primary ranged attack. Deals 80-140 │                          │
│          │ damage. Bounces off walls.          │                          │
│          │                                     │                          │
│          │ **Melee Strike** (Melee)            │                          │
│          │ Close-range slash. Deals 100-210    │                          │
│          │ damage. Can hit multiple foes.      │                          │
│          │                                     │                          │
│          │ **Curse Aura** (Special)            │                          │
│          │ Applies curse debuff to nearby      │                          │
│          │ enemies. Reduces damage output by  │                          │
│          │ 30% for 10 seconds.                │                          │
│          │                                     │                          │
│          │ ## Loot Table                       │                          │
│          │                                     │                          │
│          │ ┌─────────────────────┬──────┬──┐  │                          │
│          │ │ Item                │ Rarity│%│  │                          │
│          │ ├─────────────────────┼──────┼──┤  │                          │
│          │ │ Gold Coin (50-200)  │ Common│70│  │                          │
│          │ ├─────────────────────┼──────┼──┤  │                          │
│          │ │ Demonic Essence     │ Rare │10│  │                          │
│          │ ├─────────────────────┼──────┼──┤  │                          │
│          │ │ Demon Fang          │ Rare │5│   │                          │
│          │ ├─────────────────────┼──────┼──┤  │                          │
│          │ │ Dark Staff          │ Epic │3│   │                          │
│          │ └─────────────────────┴──────┴──┘  │                          │
│          │                                     │                          │
│          │ ## Habitat & Locations              │                          │
│          │                                     │                          │
│          │ **Spawn Location:** Castle Depths   │                          │
│          │ **Level Requirement:** 30+          │                          │
│          │ **Group Size Recommendation:** 3-5 │                          │
│          │ **Respawn Time:** 30 minutes       │                          │
│          │                                     │                          │
│          │ Demons spawn in dark chambers      │                          │
│          │ throughout the castle depths.      │                          │
│          │                                     │                          │
│          │ ## Related Creatures                │                          │
│          │                                     │                          │
│          │ ╔════════════╗ ╔════════════╗     │                          │
│          │ ║   Shadow   ║ ║ Lesser     ║     │                          │
│          │ ║   Beast    ║ ║ Demon      ║     │                          │
│          │ ║ [Image]    ║ ║ [Image]    ║     │                          │
│          │ ║ Boss       ║ ║ Mini-Boss  ║     │                          │
│          │ ║ Related    ║ ║ Related    ║     │                          │
│          │ ╚════════════╝ ╚════════════╝     │                          │
│          │                                     │                          │
│          │ ## Gallery                          │                          │
│          │                                     │                          │
│          │ [Img1] [Img2] [Img3] [Img4]        │                          │
│          │ [Img5] [Img6] [Img7] [Img8]        │                          │
│          │                                     │                          │
├──────────┴──────────────────────────────────────┴──────────────────────────┤
│                                                                            │
│  Categories: Undead | Demons | Bosses | Castle Creatures                  │
│  Last Updated: 2024-01-15 | Edit History                                  │
│                                                                            │
│                                FOOTER                                     │
└──────────────────────────────────────────────────────────────────────────┘
```

### Responsive Behavior - Tablet (992px)

```
┌────────────────────────────────────────────────────┐
│              HEADER / NAV BAR                      │
├────────────┬─────────────────────────────────────┤
│   SIDEBAR  │ Home > Creatures > Bosses > Demon   │
│  ≡ (Menu)  │                                     │
│            │ ╔═══════════════════════════════╗  │
│            │ ║ [Creature Image: 250px]       ║  │
│            │ ║ Demon                         ║  │
│            │ ║ Type: Boss | ★★★★★           ║  │
│            │ ║ Location: Castle Depths       ║  │
│            │ ╚═══════════════════════════════╝  │
│            │                                     │
│            │ ### Demon                          │
│            │                                     │
│            │ **Type:** Boss                      │
│            │ Detailed description...            │
│            │                                     │
│            │ ## Statistics                       │
│            │ [Table of stats - full width]       │
│            │                                     │
│            │ ## Abilities & Behavior             │
│            │ Content...                          │
│            │                                     │
│            │ ## Loot Table                       │
│            │ [Table]                             │
│            │                                     │
│            │ ## Related Creatures                │
│            │ [Card] [Card]  (2 columns)          │
│            │ [Card] [Card]                       │
│            │                                     │
│            │ ## Gallery                          │
│            │ [Img] [Img] [Img]  (3 columns)      │
│            │ [Img] [Img] [Img]                   │
│            │                                     │
├────────────┴─────────────────────────────────────┤
│             FOOTER                               │
└────────────────────────────────────────────────┘
```

### Responsive Behavior - Mobile (< 768px)

```
┌──────────────────────────────────┐
│    HEADER / NAV                  │
├──────────────────────────────────┤
│ ≡  Logo  🔍                       │
├──────────────────────────────────┤
│                                  │
│ Home > Creatures > Bosses > Demon│
│                                  │
│ ╔══════════════════════════════╗│
│ ║ [Creature Image: 100%]       ║│
│ ║                              ║│
│ ║       Demon Avatar           ║│
│ ║                              ║│
│ ╚══════════════════════════════╝│
│                                  │
│ Demon                            │
│ Boss • ★★★★★ • Castle Depths     │
│                                  │
│ [Difficulty: ████░░] Hard       │
│ [Recommended Level: 30+]        │
│                                  │
│ ### Overview                     │
│                                  │
│ The Demon is one of the most    │
│ feared creatures in the        │
│ Nightmares realm...             │
│                                  │
│ ## Statistics [Tap to Expand ▼]  │
│                                  │
│ 📊 Hit Points: 220              │
│ 📈 Experience: 440              │
│ 🛡️  Armor: 13                   │
│ ⚔️  Melee Damage: 100-210        │
│ ➡️  Ranged Damage: 80-140        │
│ ✨ Magic Damage: 60-120          │
│                                  │
│ [Show All Stats ▼]              │
│                                  │
│ ## Abilities                     │
│                                  │
│ **Dark Bolt**                    │
│ Primary ranged attack. Deals    │
│ 80-140 damage...                │
│                                  │
│ **Melee Strike**                 │
│ Close-range slash...             │
│                                  │
│ [+ 1 More Ability]              │
│                                  │
│ ## Loot Table [Tap to Expand ▼]  │
│                                  │
│ ## Habitat                       │
│                                  │
│ **Location:** Castle Depths      │
│ **Level:** 30+                   │
│ **Group Size:** 3-5              │
│                                  │
│ ## Related Creatures             │
│                                  │
│ [Card] [Card]  (2 columns)       │
│                                  │
│ ## Gallery [Tap to View ▼]       │
│                                  │
├──────────────────────────────────┤
│          FOOTER                  │
└──────────────────────────────────┘
```

---

## WIREFRAME 2: ITEM DETAIL PAGE

### Desktop View (1200px+)

```
┌──────────────────────────────────────────────────────────────────────────────┐
│                              HEADER / NAV BAR                                │
├──────────┬──────────────────────────────────────┬──────────────────────────┤
│          │ Home > Items > Weapons > Dark Staff  │                          │
│  SIDEBAR │                                     │   TABLE OF               │
│          │                                     │   CONTENTS               │
│  🏠 Home │ ╔════════════════════════════════╗ │  ═════════════════       │
│          │ ║   [Item Image: 280px]          ║ │  📑 Overview             │
│  📦      │ ║                                ║ │  📊 Stats                │
│  Items   │ ║     Dark Staff Icon            ║ │  💰 Value & Acquisition  │
│  ├─ Weapons
│          │ ║                                ║ │  🎯 Abilities            │
│  ├─ Armor │ ╚════════════════════════════════╝ │  🔧 Crafting             │
│  ├─ Consumables                    │  👥 Used By              │
│  ├─ Crafting                       │  🔗 Related Items        │
│          │                                     │  ═════════════════       │
│  👤      │ ### Dark Staff                      │                          │
│  NPCs    │                                     │                          │
│          │ **Rarity:** Epic                    │                          │
│  🗺️       │ **Type:** Two-Handed Weapon        │                          │
│  World   │ **Category:** Staff/Magic Weapon    │                          │
│          │ **Item Level:** 45                  │                          │
│  🔍      │ **Value:** 450 Gold Coins           │                          │
│  Search  │                                     │                          │
│          │ ---                                │                          │
│          │                                     │                          │
│          │ ## Overview                        │                          │
│          │                                     │                          │
│          │ A sophisticated staff carved from   │                          │
│          │ dark obsidian and reinforced with   │                          │
│          │ demonic essence. This legendary    │                          │
│          │ weapon channels dark magic with    │                          │
│          │ incredible efficiency, making it   │                          │
│          │ the preferred choice of high-level  │                          │
│          │ sorcerers and mages throughout      │                          │
│          │ the Nightmares realm.              │                          │
│          │                                     │                          │
│          │ ## Item Stats & Properties          │                          │
│          │                                     │                          │
│          │ ┌──────────────────────┬──────────┐ │                          │
│          │ │ Attack Power         │ +35      │ │                          │
│          │ ├──────────────────────┼──────────┤ │                          │
│          │ │ Magic Power          │ +50      │ │                          │
│          │ ├──────────────────────┼──────────┤ │                          │
│          │ │ Intelligence Bonus   │ +15      │ │                          │
│          │ ├──────────────────────┼──────────┤ │                          │
│          │ │ Defense Penalty      │ -5       │ │                          │
│          │ ├──────────────────────┼──────────┤ │                          │
│          │ │ Weight               │ 2.5 lbs  │ │                          │
│          │ ├──────────────────────┼──────────┤ │                          │
│          │ │ Required Level       │ 25+      │ │                          │
│          │ ├──────────────────────┼──────────┤ │                          │
│          │ │ Required Skills      │ Magic 40 │ │                          │
│          │ ├──────────────────────┼──────────┤ │                          │
│          │ │ Durability           │ 100/100  │ │                          │
│          │ └──────────────────────┴──────────┘ │                          │
│          │                                     │                          │
│          │ ## Special Abilities                │                          │
│          │                                     │                          │
│          │ [Tab: Passive] [Tab: Active]        │                          │
│          │ ────────────────────────────────────│                          │
│          │                                     │                          │
│          │ **Dark Mastery (Passive)**          │                          │
│          │ Increases dark magic damage by 20%.│                          │
│          │                                     │                          │
│          │ **Obsidian Bolt (Active - 30s CD)** │                          │
│          │ Launches a bolt of dark energy,    │                          │
│          │ dealing 150-200 damage in a 3m     │                          │
│          │ radius. Requires 30 Mana.          │                          │
│          │                                     │                          │
│          │ ## Value & Acquisition              │                          │
│          │                                     │                          │
│          │ **Purchase Price:** 450 Gold Coins  │                          │
│          │ **Sell Value:** 225 Gold Coins      │                          │
│          │ **Drop Rate:** 0.5% (Demon Boss)   │                          │
│          │                                     │                          │
│          │ ### Where to Find                   │                          │
│          │                                     │                          │
│          │ **From Creatures:**                 │                          │
│          │ • Demon (Boss) - 0.5% drop rate    │                          │
│          │ • Shadow Beast (Boss) - 0.3%       │                          │
│          │                                     │                          │
│          │ **From NPCs:**                      │                          │
│          │ • Mage Tower Quartermaster - Buy    │                          │
│          │ • Wizard's Guild Merchant - Buy     │                          │
│          │                                     │                          │
│          │ **From Quests:**                    │                          │
│          │ • "Dark Secrets" - Reward           │                          │
│          │ • "Sorcerer's Trial" - Reward       │                          │
│          │                                     │                          │
│          │ **Crafting Recipe:**                │                          │
│          │ [Collapsible Section]               │                          │
│          │ (See Crafting section below)        │                          │
│          │                                     │                          │
│          │ ## Crafting & Enhancement           │                          │
│          │                                     │                          │
│          │ **Recipe:** 2x Obsidian Shard       │                          │
│          │            + 1x Demonic Essence    │                          │
│          │            + 1x Ancient Binding    │                          │
│          │                                     │                          │
│          │ **Crafting Skill Required:** Magic  │                          │
│          │                                     │                          │
│          │ **Enhancement Materials:**          │                          │
│          │ • Enhancement Stones (up to +5)     │                          │
│          │ • Demon's Essence (special affix)   │                          │
│          │                                     │                          │
│          │ ## Used By (Professions/Classes)    │                          │
│          │                                     │                          │
│          │ ╔════════════════╗ ╔════════════╗  │                          │
│          │ ║    Sorcerer    ║ ║  Mage      ║  │                          │
│          │ ║  [Icon] 95%    ║ ║ [Icon] 80% ║  │                          │
│          │ ║   Use Rate     ║ ║ Use Rate   ║  │                          │
│          │ ╚════════════════╝ ╚════════════╝  │                          │
│          │                                     │                          │
│          │ ╔════════════════╗                  │                          │
│          │ ║ Dark Knight    ║                  │                          │
│          │ ║  [Icon] 40%    ║                  │                          │
│          │ ║   Use Rate     ║                  │                          │
│          │ ╚════════════════╝                  │                          │
│          │                                     │                          │
│          │ ## Related Items                    │                          │
│          │                                     │                          │
│          │ ╔════════════╗ ╔════════════╗     │                          │
│          │ ║ Obsidian   ║ ║ Demon Fang ║     │                          │
│          │ ║ Shard      ║ ║ Dagger     ║     │                          │
│          │ ║ [Image]    ║ ║ [Image]    ║     │                          │
│          │ ║ Common     ║ ║ Rare       ║     │                          │
│          │ ║ Crafting   ║ ║ Weapon     ║     │                          │
│          │ ╚════════════╝ ╚════════════╝     │                          │
│          │                                     │                          │
│          │ ╔════════════╗ ╔════════════╗     │                          │
│          │ ║ Robe of    ║ ║ Ring of    ║     │                          │
│          │ ║ Dark Magic ║ ║ Power      ║     │                          │
│          │ ║ [Image]    ║ ║ [Image]    ║     │                          │
│          │ ║ Epic       ║ ║ Rare       ║     │                          │
│          │ ║ Armor      ║ ║ Accessory  ║     │                          │
│          │ ╚════════════╝ ╚════════════╝     │                          │
│          │                                     │                          │
├──────────┴──────────────────────────────────────┴──────────────────────────┤
│  Categories: Weapons | Staves | Rare Items | Crafting Ingredients         │
│  Last Updated: 2024-01-15 | Edit History                                  │
│                            FOOTER                                          │
└──────────────────────────────────────────────────────────────────────────┘
```

### Mobile View (< 768px) - Item Page

```
┌──────────────────────────────────────────┐
│         HEADER / NAV                     │
├──────────────────────────────────────────┤
│ ≡  Logo  🔍                               │
├──────────────────────────────────────────┤
│                                          │
│ Home > Items > Weapons > Dark Staff      │
│                                          │
│ ╔══════════════════════════════════════╗│
│ ║   [Item Image: 100% width]           ║│
│ ║                                      ║│
│ ║      Dark Staff Icon                 ║│
│ ║                                      ║│
│ ╚══════════════════════════════════════╝│
│                                          │
│ Dark Staff                               │
│ Epic • Staff • Level 45                  │
│                                          │
│ [⭐⭐⭐⭐⭐] (5/5 Rating)                  │
│ [445 Gold] [Drop: 0.5%]                 │
│                                          │
│ ### Overview                             │
│                                          │
│ A sophisticated staff carved from dark   │
│ obsidian and reinforced with demonic    │
│ essence...                               │
│                                          │
│ ## Item Stats [Tap to Expand ▼]         │
│                                          │
│ ⚔️  Attack Power: +35                    │
│ ✨ Magic Power: +50                     │
│ 🧠 Intelligence: +15                    │
│ 🛡️  Defense Penalty: -5                  │
│ ⚖️  Weight: 2.5 lbs                      │
│ 📍 Required Level: 25+                   │
│ 🎓 Crafting Skill: Magic 40              │
│                                          │
│ ## Special Abilities                     │
│                                          │
│ **Dark Mastery**                         │
│ Passive: +20% Dark Magic Damage          │
│                                          │
│ **Obsidian Bolt** (Active)               │
│ 30s cooldown. 150-200 damage.           │
│ [Learn More ▼]                          │
│                                          │
│ ## How to Get                            │
│                                          │
│ **Buy:**                                 │
│ 🛒 Mage Tower Quartermaster              │
│ 🛒 Wizard's Guild Merchant               │
│ [450 Gold Coins]                        │
│                                          │
│ **Drop From:**                           │
│ 👹 Demon (Boss) - 0.5%                   │
│ 👹 Shadow Beast - 0.3%                   │
│                                          │
│ **Craft It:**                            │
│ [View Recipe ▼]                         │
│                                          │
│ **Quest Reward:**                        │
│ ✦ Dark Secrets                           │
│ ✦ Sorcerer's Trial                       │
│                                          │
│ ## Who Uses This?                        │
│                                          │
│ [Sorcerer] 95% ⭐⭐⭐⭐⭐                   │
│ [Mage] 80% ⭐⭐⭐⭐                        │
│ [Dark Knight] 40% ⭐⭐                    │
│                                          │
│ ## Similar Items                         │
│                                          │
│ [Obsidian Shard] [Demon Fang Dagger]    │
│ [Robe of Dark Magic] [Ring of Power]    │
│                                          │
├──────────────────────────────────────────┤
│            FOOTER                        │
└──────────────────────────────────────────┘
```

---

## WIREFRAME 3: NPC DETAIL PAGE

### Desktop View (1200px+)

```
┌──────────────────────────────────────────────────────────────────────────────┐
│                              HEADER / NAV BAR                                │
├──────────┬──────────────────────────────────────┬──────────────────────────┤
│          │ Home > NPCs > Merchants > Eldric     │                          │
│  SIDEBAR │                                     │   TABLE OF               │
│          │                                     │   CONTENTS               │
│  🏠 Home │ ╔════════════════════════════════╗ │  ═════════════════       │
│          │ ║   [NPC Portrait: 280px]        ║ │  📑 Profile              │
│          │ ║                                ║ │  📍 Location             │
│  👤      │ ║     Eldric the Mage            ║ │  💬 Dialogue             │
│  NPCs    │ ║                                ║ │  🛍️  Trading              │
│  ├─ Merchants
│          │ ║                                ║ │  📜 Quests               │
│  ├─ Quest Givers
│  ├─ Combat  ╚════════════════════════════════╝ │  🔗 Related              │
│  ├─ Trainers                                  │  ═════════════════       │
│          │                                     │                          │
│  👹      │ ### Eldric the Mage                │                          │
│  Creatures│                                     │                          │
│          │ **Role:** Merchant / Quest Giver    │                          │
│  📦      │ **Location:** Mage Tower, 3rd Floor │                          │
│  Items   │ **Race:** Human                     │                          │
│          │ **Affiliation:** Mage's Guild       │                          │
│  🗺️       │ **Availability:** Always           │                          │
│  World   │                                     │                          │
│          │ ---                                │                          │
│  🔍      │                                     │                          │
│  Search  │ ## Profile & Background             │                          │
│          │                                     │                          │
│          │ Eldric is a renowned mage and      │                          │
│          │ master of the arcane arts. For     │                          │
│          │ decades, he has served as the      │                          │
│          │ primary merchant of magical items  │                          │
│          │ and reagents in the Mage's Tower.  │                          │
│          │ Despite his age, Eldric remains    │                          │
│          │ sharp and insightful, often        │                          │
│          │ offering cryptic advice to         │                          │
│          │ adventurers who seek his counsel.  │                          │
│          │                                     │                          │
│          │ Eldric has also been involved in   │                          │
│          │ several major story events, and    │                          │
│          │ many consider him a key figure in  │                          │
│          │ the fight against darkness.        │                          │
│          │                                     │                          │
│          │ ## Location & Access                │                          │
│          │                                     │                          │
│          │ ┌────────────────────────┐         │                          │
│          │ │ 🏢 Mage Tower          │         │                          │
│          │ │ 📍 3rd Floor           │         │                          │
│          │ │ 📌 Coordinates: X:120  │         │                          │
│          │ │              Y:85      │         │                          │
│          │ │ 🗺️  Map: [View Large ▼]│         │                          │
│          │ │ 🚪 Access: Public      │         │                          │
│          │ └────────────────────────┘         │                          │
│          │                                     │                          │
│          │ [Map Preview]                       │                          │
│          │ ┌──────────────────────────────┐   │                          │
│          │ │                              │   │                          │
│          │ │                              │   │                          │
│          │ │  [Mage Tower Layout]         │   │                          │
│          │ │  Eldric Location Marked: 📍  │   │                          │
│          │ │                              │   │                          │
│          │ └──────────────────────────────┘   │                          │
│          │                                     │                          │
│          │ ## Dialogue & Greetings             │                          │
│          │                                     │                          │
│          │ [Tab: Greeting] [Tab: Farewell]   │                          │
│          │ ─────────────────────────────────   │                          │
│          │                                     │                          │
│          │ **First Meeting:**                  │                          │
│          │ "Greetings, young traveler. I am   │                          │
│          │ Eldric, keeper of magical          │                          │
│          │ knowledge in this tower. How might │                          │
│          │ I assist you today?"                │                          │
│          │                                     │                          │
│          │ **Subsequent Visits:**              │                          │
│          │ "Ah, welcome back. Have you come   │                          │
│          │ to purchase supplies or do you     │                          │
│          │ have other business with me?"      │                          │
│          │                                     │                          │
│          │ **Farewell:**                       │                          │
│          │ "May your magical journey be       │                          │
│          │ fruitful. Return if you need my    │                          │
│          │ services."                          │                          │
│          │                                     │                          │
│          │ ## Trading & Commerce               │                          │
│          │                                     │                          │
│          │ **Buys:** Magical Reagents, Runestones
│          │ **Sells:** Staves, Robes, Scrolls, Spellbooks
│          │                                     │                          │
│          │ ### Items for Sale (20 Total)      │                          │
│          │                                     │                          │
│          │ ┌────────────────────┬───────┬──┐  │                          │
│          │ │ Item               │ Price │Qty│  │                          │
│          │ ├────────────────────┼───────┼──┤  │                          │
│          │ │ Mana Potion (Small)│ 50    │999│  │                          │
│          │ ├────────────────────┼───────┼──┤  │                          │
│          │ │ Mana Potion (Large)│ 150   │50 │  │                          │
│          │ ├────────────────────┼───────┼──┤  │                          │
│          │ │ Spellbook: Fireball│ 200   │5  │  │                          │
│          │ ├────────────────────┼───────┼──┤  │                          │
│          │ │ Staff of Wisdom    │ 850   │2  │  │                          │
│          │ ├────────────────────┼───────┼──┤  │                          │
│          │ │ Robe of Mages      │ 600   │3  │  │                          │
│          │ └────────────────────┴───────┴──┘  │                          │
│          │ [View Full Inventory ▼]            │                          │
│          │                                     │                          │
│          │ **Prices adjusted based on:**       │                          │
│          │ • Your Reputation (10% bonus)      │                          │
│          │ • Trading Skill Level               │                          │
│          │ • Current market fluctuation        │                          │
│          │                                     │                          │
│          │ ## Quests & Interactions            │                          │
│          │                                     │                          │
│          │ ### Available Quests (3 Total)      │                          │
│          │                                     │                          │
│          │ ☐ [Quest] "Rare Reagents"          │                          │
│          │   ⭐ Level 20 | Reward: 200 XP     │                          │
│          │   "Eldric needs rare spell         │                          │
│          │    components from the Willow      │                          │
│          │    Forest."                         │                          │
│          │                                     │                          │
│          │ ✓ [Quest] "Dark Secrets"           │                          │
│          │   ⭐ Level 30 | Status: Complete   │                          │
│          │   Reward: Dark Staff               │                          │
│          │   "Retrieve the forbidden tome..."  │                          │
│          │                                     │                          │
│          │ ○ [Quest] "Artifact Restoration"   │                          │
│          │   ⭐ Level 40 | Status: In Progress│                          │
│          │   "Bring the broken amulet to     │                          │
│          │    Eldric for restoration."         │                          │
│          │                                     │                          │
│          │ ### Previous Interactions           │                          │
│          │                                     │                          │
│          │ **Quest History:** 5 completed     │                          │
│          │ **Reputation:** Friendly (+50)      │                          │
│          │ **Last Interaction:** 2 hours ago  │                          │
│          │                                     │                          │
│          │ ## Related Entities                 │                          │
│          │                                     │                          │
│          │ **Associated NPCs:**                │                          │
│          │ ╔══════════════╗ ╔══════════════╗ │                          │
│          │ ║  Lyssa       ║ ║  Marcus      ║ │                          │
│          │ ║  Apprentice  ║ ║  Quartermaster
│          │ ║  [Portrait]  ║ ║  [Portrait]  ║ │                          │
│          │ ║  Works With  ║ ║  Works With  ║ │                          │
│          │ ║  Eldric      ║ ║  Eldric      ║ │                          │
│          │ ╚══════════════╝ ╚══════════════╝ │                          │
│          │                                     │                          │
│          │ **Related Quests:**                 │                          │
│          │ ╔══════════════╗ ╔══════════════╗ │                          │
│          │ ║  "Tower      ║ ║  "Mage's     ║ │                          │
│          │ ║  Secrets"    ║ ║  Conflict"   ║ │                          │
│          │ ║  Epic Quest  ║ ║  Story Arc   ║ │                          │
│          │ ║  Mentions:   ║ ║  Features:   ║ │                          │
│          │ ║  Eldric      ║ ║  Eldric      ║ │                          │
│          │ ╚══════════════╝ ╚══════════════╝ │                          │
│          │                                     │                          │
│          │ **Related Location:**               │                          │
│          │ ╔══════════════╗                    │                          │
│          │ ║  Mage Tower  ║                    │                          │
│          │ ║  [Image]     ║                    │                          │
│          │ ║  Headquarters║                    │                          │
│          │ ║  of Eldric   ║                    │                          │
│          │ ╚══════════════╝                    │                          │
│          │                                     │                          │
├──────────┴──────────────────────────────────────┴──────────────────────────┤
│  Categories: Merchants | NPCs | Mage's Guild | Mage Tower Residents        │
│  Last Updated: 2024-01-15 | Edit History                                  │
│                            FOOTER                                          │
└──────────────────────────────────────────────────────────────────────────┘
```

### Mobile View (< 768px) - NPC Page

```
┌──────────────────────────────────────────┐
│         HEADER / NAV                     │
├──────────────────────────────────────────┤
│ ≡  Logo  🔍                               │
├──────────────────────────────────────────┤
│                                          │
│ Home > NPCs > Merchants > Eldric         │
│                                          │
│ ╔══════════════════════════════════════╗│
│ ║   [NPC Portrait: 100% width]         ║│
│ ║                                      ║│
│ ║      Eldric the Mage Portrait        ║│
│ ║                                      ║│
│ ╚══════════════════════════════════════╝│
│                                          │
│ Eldric the Mage                          │
│ Merchant • Quest Giver • Mage's Guild    │
│                                          │
│ 📍 Mage Tower, 3rd Floor                 │
│ 🕐 Always Available                      │
│ ⭐ Reputation: Friendly (+50)            │
│                                          │
│ ### About                                │
│                                          │
│ Eldric is a renowned mage and master of │
│ the arcane arts. For decades, he has    │
│ served as the primary merchant of       │
│ magical items...                        │
│                                          │
│ ## Location                              │
│                                          │
│ 🏢 Mage Tower                            │
│ 📍 3rd Floor (X: 120, Y: 85)             │
│ [View Map ▼]                            │
│                                          │
│ ## Dialogue                              │
│                                          │
│ **Greeting:**                            │
│ "Greetings, young traveler. I am        │
│ Eldric, keeper of magical knowledge...  │
│                                          │
│ [Show More ▼]                           │
│                                          │
│ ## Shop & Trading                        │
│                                          │
│ **Items for Sale:** 20                   │
│                                          │
│ 🛍️  Mana Potion (Small) - 50 Gold       │
│ 🛍️  Mana Potion (Large) - 150 Gold      │
│ 🛍️  Spellbook: Fireball - 200 Gold      │
│ 🛍️  Staff of Wisdom - 850 Gold          │
│ 🛍️  Robe of Mages - 600 Gold            │
│                                          │
│ [View Full Shop ▼]                      │
│                                          │
│ **Buys:** Magical Reagents, Runestones  │
│                                          │
│ ## Quests                                │
│                                          │
│ 📋 Available Quest                       │
│ ☐ Rare Reagents                         │
│    Level 20 • 200 XP Reward              │
│    [Learn More ▼]                       │
│                                          │
│ ✓ Completed Quest                       │
│ ✓ Dark Secrets                          │
│    Level 30 • Dark Staff Reward          │
│                                          │
│ 🔄 Active Quest                         │
│ ○ Artifact Restoration                  │
│    Level 40 • In Progress                │
│    [Continue ▼]                         │
│                                          │
│ **Reputation:** Friendly (+50)           │
│ **Quests Completed:** 5                  │
│                                          │
│ ## Related People & Places              │
│                                          │
│ **Works With:**                          │
│ [Lyssa - Apprentice] [Marcus - QM]      │
│                                          │
│ **Location:**                            │
│ [Mage Tower]                            │
│                                          │
│ **Featured In:**                         │
│ [Tower Secrets Quest] [Mage's Conflict] │
│                                          │
├──────────────────────────────────────────┤
│            FOOTER                        │
└──────────────────────────────────────────┘
```

---

## KEY WIREFRAME ANNOTATIONS

### Information Architecture Principles

1. **Visual Hierarchy**
   - Hero image at top draws attention
   - Key metadata (type, location, stats) visible immediately
   - Secondary info in expandable sections
   - Related content at bottom encourages exploration

2. **Component Placement**
   - Infobox floats right on desktop, stacks on mobile
   - Content flows naturally from general to specific
   - Related entities in card grid format
   - Call-to-action buttons (Trading, Questing) prominent

3. **Responsive Behavior**
   - Desktop: 3-column with sticky sidebar
   - Tablet: 2-column, right sidebar collapses
   - Mobile: Single column, hero image full-width
   - Tab navigation becomes accordion on mobile

4. **Content Sections**
   - Overview: Story/lore (most engaging)
   - Stats/Details: Structured data tables
   - Acquisition/Usage: Practical information
   - Related: Encourages wiki navigation
   - Gallery: Visual media (images, screenshots)

5. **Mobile Optimizations**
   - Expandable sections reduce scrolling
   - Larger touch targets (44px minimum)
   - Full-width images for better visibility
   - Simplified table layouts
   - Accessible form inputs for any interactive elements

---

## RESPONSIVE BREAKPOINTS SUMMARY

| Breakpoint | Layout | Features |
|-----------|--------|----------|
| 1200px+   | 3-column | Full sidebars, sticky TOC, infobox floats |
| 992-1199px | 2-column | Right sidebar collapses into main column |
| 768-991px | 2-column | Hamburger menu, smaller fonts |
| <768px    | 1-column | Stack all content, full-width components |

---

## COMPONENT USAGE BY PAGE

### Creature Detail Page
- Infobox (creature stats)
- Stat tables (combat stats, abilities)
- Tabs (Combat/Behavior/Movement)
- Badges (Type, Difficulty, Status)
- Cards (Related creatures)
- Gallery (creature images)

### Item Detail Page
- Infobox (item preview)
- Stat tables (item properties)
- Tabs (Passive/Active abilities)
- Badges (Rarity, Type, Quality)
- Cards (Related items, NPCs who use it)
- List (Acquisition methods, crafting recipes)

### NPC Detail Page
- Portrait (NPC image)
- Info cards (location, affiliation)
- Dialog box (conversation text)
- Table (inventory/shop items)
- Quest list (available/completed)
- Cards (Related NPCs, locations, quests)

---

## NOTES FOR DEVELOPERS

1. **Infobox Component**
   - Should be absolutely positioned on desktop
   - Falls to top of content on tablet/mobile
   - Support image, title, key-value pairs, buttons

2. **Table of Contents**
   - Auto-generated from H2/H3 headings
   - Sticky on desktop (position: sticky)
   - Hides/collapses on mobile
   - Highlight current section on scroll

3. **Cards Component**
   - Flexible grid layout (3-4 columns desktop, 2 tablet, 1 mobile)
   - Image on top, content below
   - Equal height with bottom-aligned buttons
   - Hover effect (lift/shadow)

4. **Tabs Component**
   - SVG-based or CSS tabs
   - Active indicator (underline, not background)
   - Mobile: Consider converting to accordion
   - Accessible via keyboard navigation

5. **Gallery**
   - Lightbox for image expansion
   - Lazy loading for images
   - Thumbnails in grid
   - Caption support

6. **Search & Filter**
   - Real-time filtering
   - URL parameter updates
   - Clear filters button
   - Active filter badges

This wireframe set provides comprehensive layout guidance for the Nightmares Wiki's core entity detail pages.
