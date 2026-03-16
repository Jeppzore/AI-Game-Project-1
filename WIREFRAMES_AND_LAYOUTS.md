# Nightmares Wiki - Wireframes & Layout Specifications

## 1. Navigation Architecture

### Information Hierarchy
```
Home (Landing Page)
├── Enemies
│   ├── Enemy List (Grid/Table)
│   ├── Enemy Detail Page
│   └── Enemy Comparison View
├── Items (Future)
│   ├── Item List
│   └── Item Detail Page
├── NPCs (Future)
│   ├── NPC List
│   └── NPC Detail Page
├── Search (Global)
└── About
```

---

## 2. Page Layouts

### 2.1 Master Layout (All Pages)

```
┌─────────────────────────────────────────────────────────┐
│                      NAVBAR                             │
│  [LOGO] Enemies | Items | NPCs | About   [Search]      │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  [SIDEBAR - Optional]        [MAIN CONTENT AREA]       │
│  - Filters                   - Page Header             │
│  - Categories                - Content Grid/List       │
│  - Tags                       - Pagination             │
│                                                         │
├─────────────────────────────────────────────────────────┤
│                      FOOTER                             │
│  © 2026 Nightmares Wiki | About | Contact | Privacy    │
└─────────────────────────────────────────────────────────┘
```

### 2.2 Navbar Wireframe

```
┌─────────────────────────────────────────────────────────┐
│ [Pixel] NIGHTMARES WIKI    Enemies | Items | NPCs |...  │
│                                           [🔍 Search]   │
└─────────────────────────────────────────────────────────┘

Height: 80px
Border-bottom: 4px neon purple
Sticky: Yes
Shadow: 0 4px 12px rgba(0, 0, 0, 0.6)

Logo Style:
- Font: Press Start 2P
- Size: 20px
- Color: Acid Green (#39ff14)
- Glow effect on hover

Nav Links:
- Font: Inter, 16px, Bold
- Color: Off-white (default), Neon Purple (active)
- Underline animation on active/hover
- Spacing: 24px between items
```

### Desktop Layout
```
[Logo]  [Enemies] [Items] [NPCs] [About]                [Search]
```

### Mobile Layout (< 768px)
```
[☰]  [Logo]                                      [🔍]
- Dropdown menu for nav items
- Hamburger menu in top-left
- Search as popup/modal
```

---

### 2.3 Homepage/Landing Page

```
┌─────────────────────────────────────────────────────────┐
│                     PAGE HEADER                         │
│                  NIGHTMARES WIKI                        │
│          Explore creatures from the Nightmares game     │
│                                                         │
│              [Browse Enemies] [Recent Enemies]          │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  Featured Enemies                                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │  Dragon      │  │  Goblin      │  │  Wraith      │  │
│  │  Level 42    │  │  Level 5     │  │  Level 28    │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
│                                                         │
│  Recent Updates                                         │
│  ├─ Added: Basilisk (2 days ago)                       │
│  ├─ Updated: Orc Stats (5 days ago)                    │
│  └─ Added: Sprite Data (1 week ago)                    │
│                                                         │
├─────────────────────────────────────────────────────────┤
│                      FOOTER                             │
└─────────────────────────────────────────────────────────┘

Key Sections:
1. Hero Section (400px height)
   - Background gradient or pattern
   - Large title with subtitle
   - CTA buttons

2. Featured Content (3-column grid)
   - Enemy cards with images
   - Quick stats display

3. Recent Activity
   - Timeline or list
   - Latest additions/updates
```

---

### 2.4 Enemy List Page

```
┌─────────────────────────────────────────────────────────┐
│                    PAGE HEADER                          │
│                   ENEMIES                               │
│         Discover all enemies in the Nightmares world    │
├─────────────────────────────────────────────────────────┤
│                                                         │
│ [SIDEBAR]              [MAIN CONTENT]                  │
│ ┌──────────────────┐  ┌───────────────────────────────┐│
│ │ FILTERS          │  │ [Search] [View: Grid|List]    ││
│ │ ┌──────────────┐ │  │                               ││
│ │ │ Level Range  │ │  │ ┌─────┐ ┌─────┐ ┌─────┐     ││
│ │ │ □ 1-10       │ │  │ │Card │ │Card │ │Card │     ││
│ │ │ □ 11-20      │ │  │ │  1  │ │  2  │ │  3  │     ││
│ │ │ □ 21-40      │ │  │ └─────┘ └─────┘ └─────┘     ││
│ │ │ □ 40+        │ │  │                               ││
│ │ └──────────────┘ │  │ ┌─────┐ ┌─────┐ ┌─────┐     ││
│ │ ┌──────────────┐ │  │ │Card │ │Card │ │Card │     ││
│ │ │ Type         │ │  │ │  4  │ │  5  │ │  6  │     ││
│ │ │ □ Beast      │ │  │ └─────┘ └─────┘ └─────┘     ││
│ │ │ □ Undead     │ │  │                               ││
│ │ │ □ Humanoid   │ │  │ [← Prev] Page 1 of 12 [Next →] ││
│ │ │ □ Elemental  │ │  │                               ││
│ │ └──────────────┘ │  └───────────────────────────────┘│
│ │                  │                                    │
│ │ [Clear Filters]  │                                    │
│ └──────────────────┘                                    │
│                                                         │
├─────────────────────────────────────────────────────────┤
│                      FOOTER                             │
└─────────────────────────────────────────────────────────┘

Components:
1. Sidebar (250px, responsive collapse)
   - Filter groups (Level, Type, Rarity, etc.)
   - Clear filters button

2. Main Content Area
   - Search bar at top
   - View toggle (Grid/List/Table)
   - Grid of 3-4 cards per row (responsive)
   - Pagination controls

3. Enemy Card (280px width)
   - Image area
   - Name & level
   - Quick stats
   - "View Details" button
```

---

### 2.5 Enemy Detail Page

```
┌─────────────────────────────────────────────────────────┐
│                    PAGE HEADER                          │
│                                                         │
│          < Enemies  /  DRAGON  /  Level 42              │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  [IMAGE SECTION]          [BASIC INFO]                 │
│  ┌──────────────────┐     ┌──────────────┐             │
│  │                  │     │ Name: Dragon │             │
│  │  Enemy Image     │     │ Level: 42    │             │
│  │  (400x400px)     │     │ Type: Beast  │             │
│  │                  │     │ Rarity: Rare │             │
│  └──────────────────┘     └──────────────┘             │
│                                                         │
│  [STATS SECTION]                                       │
│  ┌─────────────────────────────────────────────────┐  │
│  │ STATS                                          │  │
│  │ ┌──────────┬──────────┬──────────┬──────────┐  │  │
│  │ │ Health   │ Attack   │ Defense  │ Speed    │  │  │
│  │ │ 250      │ 85       │ 45       │ 30       │  │  │
│  │ │ ████████ │ ██████   │ ████     │ ███      │  │  │
│  │ └──────────┴──────────┴──────────┴──────────┘  │  │
│  │ Experience: 2500 XP                            │  │
│  │ Gold: 500-1000                                 │  │
│  └─────────────────────────────────────────────────┘  │
│                                                         │
│  [LOOT TABLE]                                          │
│  ┌─────────────────────────────────────────────────┐  │
│  │ DROP TABLE                                      │  │
│  │ Item              | Rarity  | Rate  | Quantity │  │
│  │ Dragon Scale      | Rare    | 50%   | 1-3      │  │
│  │ Armor Fragment    | Common  | 80%   | 1-2      │  │
│  │ Dragon Tooth      | Epic    | 15%   | 1        │  │
│  │ Golden Coin       | Common  | 100%  | 50-200   │  │
│  └─────────────────────────────────────────────────┘  │
│                                                         │
│  [ABILITIES]                                           │
│  Fireball (AOE damage)                                 │
│  Dragon Roar (Stun effect)                             │
│  Flight (Combat advantage)                             │
│                                                         │
│  [DESCRIPTION]                                         │
│  The mighty dragon is one of the most dangerous        │
│  creatures in the Nightmares world. Known for its      │
│  devastating fire attacks and high armor...            │
│                                                         │
│  [RELATED]                                             │
│  Similar Enemies: [Card] [Card] [Card]                 │
│                                                         │
├─────────────────────────────────────────────────────────┤
│ [Edit] [Share] [Report Issue]                          │
├─────────────────────────────────────────────────────────┤
│                      FOOTER                             │
└─────────────────────────────────────────────────────────┘

Layout Sections:
1. Breadcrumb Navigation
   - "< Enemies / Dragon / Level 42"

2. Hero Section
   - Large enemy image (400x400px or responsive)
   - Basic info card (name, level, type, rarity)

3. Stats Section
   - Grid of 4 stat cards
   - Health, Attack, Defense, Speed bars
   - XP and gold rewards

4. Loot Table
   - Sortable/filterable table
   - Item name, rarity badge, drop rate %, quantity range

5. Abilities
   - Bullet list or cards
   - Description for each ability

6. Description
   - Full-width text area
   - Formatted with proper spacing

7. Related Content
   - Similar enemies carousel/grid
   - Bottom suggestions

8. Action Buttons
   - Edit (admin only)
   - Share (social)
   - Report issue
```

---

### 2.6 Search Results Page

```
┌─────────────────────────────────────────────────────────┐
│                      NAVBAR                             │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  Search Results: "dragon"                              │
│  ┌──────────────────────────────────────────────────┐  │
│  │ [Search Input] [Filters] [Sort: Relevance ▼]    │  │
│  └──────────────────────────────────────────────────┘  │
│                                                         │
│  Results: 42 matches                                   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐  │
│  │ Enemies (32)                                     │  │
│  │ ┌──────────────────────────────────────────────┐ │  │
│  │ │ Dragon [Level 42]                            │ │  │
│  │ │ Found in: Stats, Abilities, Description      │ │  │
│  │ │ Health: 250 | Attack: 85 | Defense: 45       │ │  │
│  │ └──────────────────────────────────────────────┘ │  │
│  │                                                  │  │
│  │ ┌──────────────────────────────────────────────┐ │  │
│  │ │ Dragon Knight [Level 35]                     │ │  │
│  │ │ Found in: Name, Stats, Description           │ │  │
│  │ │ Health: 180 | Attack: 75 | Defense: 50       │ │  │
│  │ └──────────────────────────────────────────────┘ │  │
│  │                                                  │  │
│  │ [Show more results...]                          │  │
│  └──────────────────────────────────────────────────┘  │
│                                                         │
│  ┌──────────────────────────────────────────────────┐  │
│  │ Items (8)                                        │  │
│  │ - Dragon Scale                                   │  │
│  │ - Dragon Tooth                                   │  │
│  │ - Dragon Armor                                   │  │
│  │ [Show more items...]                             │  │
│  └──────────────────────────────────────────────────┘  │
│                                                         │
│  ┌──────────────────────────────────────────────────┐  │
│  │ NPCs (2)                                         │  │
│  │ - Dragon Slayer Kris                             │  │
│  │ - Dragon Keeper                                  │  │
│  └──────────────────────────────────────────────────┘  │
│                                                         │
├─────────────────────────────────────────────────────────┤
│                      FOOTER                             │
└─────────────────────────────────────────────────────────┘
```

---

## 3. Component Specifications

### 3.1 Enemy Card Component

**Default State (Grid View)**
```
┌────────────────────────────┐
│  Enemy Image (280x200px)   │
├────────────────────────────┤
│ Dragon                     │
│ Level 42 • Rare            │
│                            │
│ Health:  250 Health Bar    │
│ Attack:  85  Attack Bar    │
│ Defense: 45  Def Bar       │
├────────────────────────────┤
│     [View Details] →       │
└────────────────────────────┘

Dimensions: 280px width (responsive)
Border: 2px solid var(--color-border)
Hover: Lift 4px, glow effect
States: Default, Hover, Active, Focus
```

**List View**
```
┌────────────────────────────────────────────────┐
│ [Icon] Dragon         Level 42 • Rare          │
│        Health: 250 | Attack: 85 | Defense: 45 │
└────────────────────────────────────────────────┘

Full-width row layout
Height: 80px
Hover: Background highlight
```

---

### 3.2 Stat Bar Component

```
Attack: 85/100
┌──────────────────────────────┐
│████████████░░░░░░░░░░░░░░░░░│ 85%
└──────────────────────────────┘

Components:
- Label
- Value (numeric and percentage)
- Progress bar (filled/unfilled)
- Color coding (Green=Good, Yellow=Medium, Red=Low)
```

---

### 3.3 Rarity Badge Component

```
Rarity: ⭐ Rare

Badge Style:
- Background: Subtle color based on rarity
- Color: Distinct for each level
  - Common: Gray
  - Uncommon: Green
  - Rare: Blue
  - Epic: Purple
  - Legendary: Gold

Implementation:
<span class="badge badge-rare">Rare</span>
```

---

### 3.4 Loot Table Component

```
┌─────────────────────────────────────────────────────┐
│ Item Name      │ Rarity  │ Drop Rate │ Quantity    │
├─────────────────────────────────────────────────────┤
│ Dragon Scale   │ ⭐ Rare │ 50%       │ 1-3         │
│ Armor Fragment │ Common  │ 80%       │ 1-2         │
│ Dragon Tooth   │ Epic    │ 15%       │ 1           │
│ Gold Coin      │ Common  │ 100%      │ 50-200      │
└─────────────────────────────────────────────────────┘

Features:
- Sortable columns (click to sort)
- Rarity indicator (color + badge)
- Percentage visualization (optional bar chart)
- Responsive: Mobile stacks into cards
```

---

### 3.5 Filter Sidebar Component

```
┌────────────────────────────────┐
│ FILTERS                        │
├────────────────────────────────┤
│ ☑ Level Range                  │
│  ○ 1-10         (8)            │
│  ○ 11-20        (15)           │
│  ○ 21-40        (22)           │
│  ○ 40+          (5)            │
├────────────────────────────────┤
│ ☑ Type                         │
│  ☐ Beast        (12)           │
│  ☐ Humanoid     (18)           │
│  ☐ Undead       (10)           │
│  ☐ Elemental    (8)            │
│  ☐ Plant        (2)            │
├────────────────────────────────┤
│ ☑ Rarity                       │
│  ☐ Common       (20)           │
│  ☐ Uncommon     (18)           │
│  ☐ Rare         (8)            │
│  ☐ Epic         (5)            │
│  ☐ Legendary    (2)            │
├────────────────────────────────┤
│        [Clear All Filters]     │
└────────────────────────────────┘

Features:
- Expandable/collapsible sections
- Count indicator for each option
- Clear filters button
- Sticky on desktop, collapsible on mobile
```

---

### 3.6 Pagination Component

```
← Previous  [ 1 ] [ 2 ] [ 3 ] ... [ 12 ]  Next →

Features:
- Previous/Next buttons
- Direct page number links
- Current page highlighted
- Ellipsis for large ranges
- Keyboard accessible (Arrow keys)
- Mobile: Only shows Previous/Next + current page
```

---

## 4. Responsive Breakpoints

### Desktop (1024px and up)
- Sidebar: 250px fixed
- Main content: Full width minus sidebar
- Grid: 3-4 columns
- Font sizes: Full design system sizes

### Tablet (768px - 1023px)
- Sidebar: Collapses or slides out on mobile
- Grid: 2-3 columns
- Font sizes: Slight reduction (90% of desktop)
- Spacing: Slightly reduced

### Mobile (480px - 767px)
- Single column layout
- Full-width content
- Stacked sidebar below content
- Larger touch targets (min 44px)
- Simplified navigation (hamburger menu)

### Small Mobile (< 480px)
- Single column
- Optimized font sizes
- Touch-friendly spacing
- Minimal navigation (icon only)

---

## 5. Mobile Navigation Pattern

### Hamburger Menu Implementation

```
Desktop (≥768px)
┌─────────────────────────────────────┐
│ [Logo] Enemies Items NPCs About [S] │
└─────────────────────────────────────┘

Tablet (480px-767px)
┌──────────────────────────────────┐
│ [☰] [Logo]       Search [?]      │
└──────────────────────────────────┘
  ├─ Enemies
  ├─ Items
  ├─ NPCs
  ├─ About
  └─ (Closes on selection)

Mobile (< 480px)
┌────────────────────────────┐
│ [☰] [Logo]         [🔍] [?] │
└────────────────────────────┘
  (Same dropdown menu)
```

---

## 6. Accessibility in Layouts

### Focus Management
- Visible focus indicators on all interactive elements
- Logical tab order following visual layout
- Skip-to-main-content link (visible on focus)

### Semantic Structure
```html
<header><!-- Navigation --></header>
<main id="main-content">
  <section><!-- Filters sidebar --></section>
  <section><!-- Main content --></section>
</main>
<footer><!-- Footer --></footer>
```

### Keyboard Shortcuts (Optional)
- `/` or `Ctrl+K`: Focus search
- `?`: Show help/shortcuts
- `Esc`: Close modals/dropdowns
- `Enter`: Activate buttons/links

---

## 7. Motion & Animation Timeline

### Page Load Sequence
```
Timeline (ms):
0-200ms:   Fade in navbar
200-400ms: Fade in page header
400-700ms: Slide up content grid (staggered)
700-1000ms: Fade in filters sidebar
```

### Interactive Animations
```
Button Hover: 150ms (lift + shadow)
Button Click: 100ms (press effect)
Modal Open: 300ms (fade + slide)
Card Hover: 200ms (lift + glow)
```

---

## 8. Dark Mode Behavior

The design uses dark mode as default. All wireframes assume dark backgrounds with light text.

### Color Mapping in Layouts
- Backgrounds: Dark purple/gray
- Text: Off-white/light gray
- Accents: Neon colors (purple, green, cyan, red)
- Cards: Slightly lighter than background
- Borders: Medium gray with glow on hover

---

## 9. Data-Driven Component Rendering

### Enemy Card Data Structure
```typescript
interface Enemy {
  id: string;
  name: string;
  level: number;
  type: 'beast' | 'humanoid' | 'undead' | 'elemental' | 'plant';
  rarity: 'common' | 'uncommon' | 'rare' | 'epic' | 'legendary';
  stats: {
    health: number;
    attack: number;
    defense: number;
    speed: number;
  };
  experience: number;
  goldDrop: { min: number; max: number };
  drops: Loot[];
  image?: string;
  description?: string;
  abilities?: string[];
}

interface Loot {
  itemName: string;
  rarity: string;
  dropRate: number;      // 0.0 to 1.0
  quantity: { min: number; max: number };
}
```

### Filter State Management
```typescript
interface FilterState {
  levelRange: [min: number, max: number];
  types: string[];
  rarities: string[];
  searchQuery: string;
  sortBy: 'name' | 'level' | 'relevance';
  sortOrder: 'asc' | 'desc';
  page: number;
}
```

---

## 10. Performance Considerations

### Image Optimization
- Hero images: WebP with JPEG fallback
- Thumbnails: Lazy-loaded with blur-up effect
- Max image size: 400x400px for cards
- Use `srcset` for responsive images

### Layout Performance
- Use CSS Grid/Flexbox (no floats or absolute positioning)
- Lazy-load pagination content
- Virtual scrolling for very large lists
- Minimal reflows (batch DOM updates)

---

## 11. Error States & Empty States

### Empty State (No Enemies Found)
```
┌─────────────────────────────┐
│                             │
│         😶 No Results       │
│                             │
│   We couldn't find any      │
│   enemies matching your     │
│   search. Try adjusting     │
│   your filters or search    │
│   query.                    │
│                             │
│      [Clear Filters]        │
│      [Browse All Enemies]   │
│                             │
└─────────────────────────────┘
```

### Error State
```
┌─────────────────────────────┐
│ ⚠ Error Loading Enemies     │
│                             │
│ Failed to load data from    │
│ the server. Please try:     │
│ 1. Refresh the page         │
│ 2. Check your connection    │
│ 3. Try again later          │
│                             │
│      [Retry] [Go Home]      │
└─────────────────────────────┘
```

### Loading State
```
Animated skeleton loaders:
┌────────────────────┐
│ [Skeleton bars...] │
├────────────────────┤
│ [Loading...]       │
└────────────────────┘

Or animated spinner:
      ◢◣
    ◢◣   ◢◣
    ◢◣ ◢◣
      ◢◣
```

---

## 12. Print Stylesheet

Design consideration for printing enemy details (optional future feature):

```css
@media print {
  .navbar,
  .sidebar,
  .footer,
  .button,
  [aria-label="Close"] {
    display: none;
  }

  body {
    background: white;
    color: black;
  }

  .enemy-detail {
    max-width: 100%;
    margin: 0;
  }
}
```

---

## 13. Interactions & Micro-copy

### Button Micro-copy
| Action | Text | State |
|--------|------|-------|
| View enemy | "View Details" | Default |
| Delete enemy | "Delete" | Danger |
| Save changes | "Save Changes" | Primary |
| Cancel | "Cancel" | Secondary |
| Loading | "Loading..." | Disabled |
| Error | "Retry" | Alert |

### Empty States
| State | Message |
|-------|---------|
| No results | "We couldn't find any enemies matching your search." |
| Loading | "Loading enemies..." |
| Error | "Failed to load enemies. Please try again." |
| Network offline | "You appear to be offline. Some features may be limited." |

---

## 14. State Transition Diagram

### Enemy List Page States
```
                  ┌─────────────┐
                  │   INITIAL   │
                  └──────┬──────┘
                         │
         ┌───────────────┼───────────────┐
         ▼               ▼               ▼
    ┌────────┐    ┌──────────┐    ┌──────────┐
    │LOADING │    │ LOADED   │    │  ERROR   │
    └────────┘    └──────────┘    └──────────┘
         │               │               │
         │     ┌─────────┼─────────┐    │
         │     ▼         ▼         ▼    │
         │  Filter  Navigate   Reload   │
         │     │         │         │    │
         └──────────────────────────┘
```

---

## 15. Progressive Enhancement

### Core Experience (No JavaScript)
- Static HTML page with basic links
- Full navigation works
- Data displayed in semantic markup

### Enhanced Experience (JavaScript)
- Client-side filtering
- Smooth transitions
- AJAX loading
- Advanced sorting/search

### Future Enhancements
- Service workers for offline support
- WebGL visualizations for enemy stats
- Real-time multiplayer comparisons

---

**Document Version**: 1.0  
**Last Updated**: 2026-03-16  
**Status**: Ready for Implementation
