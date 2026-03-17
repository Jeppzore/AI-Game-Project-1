# Tibia.Fandom.com Design Pattern Analysis
## Nightmares Wiki Design Reference

---

## 1. OVERALL PAGE LAYOUT STRUCTURE

### Header/Navigation Area
```
┌─────────────────────────────────────────────────────────────┐
│  [Logo] Wiki Title | Search Box [🔍] | User Menu  [⚙️]      │
├─────────────────────────────────────────────────────────────┤
│  Main Navigation: Home | Browse | Community | More ▼        │
└─────────────────────────────────────────────────────────────┘
```

**Components:**
- **Logo + Brand**: Small icon/text in top-left (30-40px)
- **Search Bar**: Prominent, centered/right-aligned with autocomplete dropdown
- **User Menu**: Login/profile icon with dropdown
- **Global Navigation**: Horizontal menu bar with main sections

---

## 2. PAGE STRUCTURE (Desktop View)

### Standard Three-Column Layout
```
┌──────────────────────────────────────────────────────────────┐
│                        HEADER                                 │
├──────────────┬──────────────────────────┬────────────────────┤
│              │                          │                    │
│  SIDEBAR     │      MAIN CONTENT        │   RIGHT SIDEBAR    │
│  (250px)     │      (600-700px)         │   (250-300px)      │
│              │                          │                    │
│ • Navigation │                          │ • Table of Contents│
│ • Categories │ Article Title            │ • Infobox Preview  │
│ • Tools      │ Breadcrumbs              │ • Related Pages    │
│              │                          │ • Ads (optional)   │
│              │ [Main Content Area]      │                    │
│              │                          │                    │
│              │                          │                    │
├──────────────┴──────────────────────────┴────────────────────┤
│                        FOOTER                                 │
└──────────────────────────────────────────────────────────────┘
```

**Proportions (Desktop):**
- Sidebar: ~18% width
- Main Content: ~60-65% width
- Right Sidebar: ~17-22% width
- Gutter/spacing: ~16px-20px between columns

---

## 3. NAVIGATION PATTERNS

### A. Breadcrumb Navigation
```
Home > Creatures > Bosses > Demon > Description
```
- **Placement**: Top of main content, above article title
- **Style**: Small text (12-13px), gray color (#666-888)
- **Separators**: > or / symbols
- **Last item**: Non-clickable or darker color (current page)
- **Functionality**: Each segment is clickable except current page

### B. Left Sidebar Navigation
**Characteristics:**
- Sticky/follows scroll on desktop
- Hierarchical structure with expandable sections
- Current page highlighted
- Icons optional but helpful
- Categories and tags grouped

**Common Sections:**
```
Navigation
├── Home
├── Creatures
│   ├── Monsters
│   ├── Bosses
│   └── Summons
├── Items
├── NPCs
├── Quests
└── World
    ├── Locations
    ├── Maps
    └── Dungeons

Categories
├── [Category filters]

Tools
├── Special pages
├── Upload media
├── Community portal
```

### C. Table of Contents (Right Sidebar)
- **Auto-generated** from H2/H3 headings
- **Sticky** positioning as user scrolls
- **Highlight current section** as user scrolls
- **Jump links** to sections
- **Nested hierarchy** reflecting heading levels

---

## 4. INFORMATION HIERARCHY & VISUAL ORGANIZATION

### Typography
- **H1 (Article Title)**: 28-32px, bold (#000), single title
- **H2 (Section Headers)**: 20-24px, bold, with bottom border (1px solid #ccc)
- **H3 (Subsections)**: 16-18px, bold
- **Body Text**: 14-16px, line-height 1.6, color #333
- **Small Text/Metadata**: 12-13px, gray #666

### Color Scheme (Fandom Standard)
- **Primary Brand**: Blue (#003399 or site-specific)
- **Text**: Dark gray/black (#222-333)
- **Links**: Blue (#0645ad) with hover state (#c23)
- **Visited Links**: Purple (#663399)
- **Borders/Dividers**: Light gray (#ddd-eee)
- **Background**: White (#fff) with optional light gray sections (#f6f6f6)
- **Alerts/Warnings**: Orange (#ff6600), Red (#cc0000)
- **Success**: Green (#00aa00)

### Spacing
- **Section margin**: 24-32px top/bottom
- **Paragraph margin**: 12-16px bottom
- **Heading margin**: 16-20px top, 8-12px bottom
- **Content padding**: 16-20px inside containers
- **Column gutters**: 16-20px

---

## 5. COMPONENT PATTERNS

### A. Infobox (Right-Aligned Sidebar Box)
```
╔═══════════════════════════╗
║   [Creature Image]        ║  Width: 280-320px
╠═══════════════════════════╣
║ Demon                     ║  Title
╠═══════════════════════════╣
║ HP: 220                   ║
║ Experience: 440           ║  Key-value pairs
║ Armor: 13                 ║
║ Damage: 100-210           ║
╠═══════════════════════════╣
║ Loot:                     ║
║ • Gold Coin (70%)         ║  Bullet lists
║ • Demonic Essence (10%)   ║
╠═══════════════════════════╣
║ Related: [Links]          ║
╚═══════════════════════════╝
```

**Styling:**
- Border: 1px solid #ccc or #bbb
- Background: Light gray (#f6f6f6) or white
- Shadows: Optional subtle shadow
- Header background: Slightly darker
- Max-width: 320px, floated right in main content
- Responsive: Falls below content on mobile

### B. Stat Tables
```
┌─────────────────┬──────────┬──────────┐
│ Stat            │ Normal   │ Critical │
├─────────────────┼──────────┼──────────┤
│ Damage          │ 100-210  │ 150-300  │
│ Hit Chance      │ 90%      │ 95%      │
│ Critical Rate   │ 10%      │ 25%      │
└─────────────────┴──────────┴──────────┘
```

**Styling:**
- Border-collapse: Full borders
- Header background: #e6e6e6 or site primary color
- Row striping: Alternate #fff and #f9f9f9
- Text alignment: Center for stats, left for labels
- Border color: #ccc
- Padding: 8-12px per cell
- Font-size: 13-14px

### C. Tabs
```
[Active Tab] [Inactive Tab] [Inactive Tab]
┌──────────────────────────────────────┐
│  Content for active tab               │
│                                      │
│                                      │
└──────────────────────────────────────┘
```

**Styling:**
- Active tab: Underline (2-3px) in brand color, bold text
- Inactive tabs: Gray text, no underline
- Hover state: Gray background on inactive tabs
- Content area: Full width below tabs
- Border: 1px solid #ccc below tabs
- Padding: 12px inside tabs, 16px in content

### D. Cards/Entity Listings
```
┌─────────────────────────────┐
│  [Image/Icon]               │
│  Title                      │
│  Brief description or tags  │  100-200px width
│  [View More] button         │
└─────────────────────────────┘
```

**Grid Layout:**
- 3-4 columns on desktop
- 2 columns on tablet
- 1 column on mobile
- Gap: 16-20px
- Equal-height cards with bottom alignment for buttons

### E. Badges & Tags
```
[Status: Active] [Difficulty: Hard] [Type: Boss]
```

**Styling:**
- Inline-block elements
- Padding: 4-6px 8-12px
- Border-radius: 3-4px or 16px (rounded)
- Font-size: 11-12px
- Background: Light color per category
- Border: Optional 1px border
- Margin: 4-8px right

### F. Buttons
**Primary Button:**
```
┌────────────────────┐
│  Action Button     │  Background: Brand color
│                    │  Color: White
└────────────────────┘
```

**Secondary Button:**
```
┌────────────────────┐
│  Secondary Action  │  Border: 1px, Background: White
└────────────────────┘
```

- Padding: 10-12px 16-20px
- Border-radius: 3-4px
- Font-weight: 600
- Hover state: Darker background or shadow
- Active state: Pressed effect

---

## 6. RESPONSIVE DESIGN BREAKPOINTS

### Desktop (1200px+)
- Full three-column layout
- Sidebar sticky on scroll
- Table of contents visible
- All components at full width

### Tablet (768px - 1199px)
- Two-column layout (sidebar + main content)
- Right sidebar integrated into main column
- Infobox floats but within content column
- Sidebar collapses to hamburger menu (optional)
- Reduced font sizes (-1-2px)

### Mobile (< 768px)
- Single column layout
- Hamburger menu for navigation
- Infobox full-width at top of content
- Cards: 1-2 per row
- Table of contents: Collapsed menu
- Reduced padding/margins (12-16px)
- Font sizes optimized for mobile reading

### Breakpoints:
- `@media (max-width: 1200px)` - Large tablet transition
- `@media (max-width: 992px)` - Tablet
- `@media (max-width: 768px)` - Mobile

---

## 7. SEARCH & FILTERING UI PATTERNS

### Global Search
```
┌────────────────────────────────────────────────────┐
│  🔍 Search the wiki...                  [Clear ✕]  │
└────────────────────────────────────────────────────┘

// Autocomplete results appear below
┌────────────────────────────────────────────────────┐
│ Demon (Creature)                                   │
│ Demonic Essence (Item)                             │
│ Demon's Vale (Location)                            │
│ View all results for "Demon"                       │
└────────────────────────────────────────────────────┘
```

**Features:**
- Real-time autocomplete (debounced)
- Category badges (Creature, Item, NPC, etc.)
- Result thumbnails optional
- Clear button to reset
- "View all results" link
- Keyboard navigation support (↓↑ to navigate, Enter to select, Esc to close)

### Advanced Filtering (Listing Pages)
```
Filters:
┌─────────────────────────┐
│ Type                    │
│ ☑ All Creatures         │
│ ☐ Monsters              │
│ ☐ Bosses                │
│ ☐ Summons               │
│                         │
│ Difficulty              │
│ ☐ Easy                  │
│ ☐ Medium                │
│ ☐ Hard                  │
│ ☐ Very Hard             │
│                         │
│ [Apply Filters]         │
└─────────────────────────┘

Results Grid: [Filtered items displayed here]
```

**Features:**
- Collapsible filter categories
- Checkboxes for multi-select
- Range sliders for numeric values (HP, Level, etc.)
- Sort dropdown (Name A-Z, HP High-Low, etc.)
- Active filter badges with clear (✕) option
- URL parameters for sharing filters

---

## 8. ENTITY DETAIL PAGE LAYOUT

### Creature/Monster Page Example

```
┌─────────────────────────────────────────────────────────────┐
│                      HEADER                                  │
├─────────────┬───────────────────────────────┬────────────────┤
│             │ Home > Creatures > Bosses >   │                │
│  SIDEBAR    │ Demon                         │  TABLE OF      │
│             │                               │  CONTENTS      │
│  • Navigate │ ╔══════════════════════════╗  │  • Overview   │
│  • Categories│ ║    [Creature Image]      ║  │  • Stats     │
│             │ ║                          ║  │  • Loot      │
│             │ ║    Demon                 ║  │  • Location  │
│             │ ║ HP: 220                  ║  │  • Tactics   │
│             │ ║ Exp: 440                 ║  │  • Gallery   │
│             │ ║ Location: Castle         ║  │               │
│             │ ╚══════════════════════════╝  │                │
│             │                               │                │
│             │ ## Overview                   │                │
│             │ Detailed description of the  │                │
│             │ creature...                   │                │
│             │                               │                │
│             │ ## Statistics                 │                │
│             │ ┌─────────────┬──────────┐   │                │
│             │ │ HP          │ 220      │   │                │
│             │ │ Experience  │ 440      │   │                │
│             │ │ Armor       │ 13       │   │                │
│             │ └─────────────┴──────────┘   │                │
│             │                               │                │
│             │ ## Loot Table                 │                │
│             │ Organized by rarity...        │                │
│             │                               │                │
│             │ ## Related Creatures          │                │
│             │ [Card] [Card] [Card]          │                │
│             │                               │                │
│             │ ## Gallery                    │                │
│             │ [Image] [Image] [Image]       │                │
│             │                               │                │
├─────────────┴───────────────────────────────┴────────────────┤
│                       FOOTER                                  │
└─────────────────────────────────────────────────────────────┘
```

**Key Characteristics:**
- Infobox at top-right of content
- Image prominent (200-300px)
- Key stats in infobox format
- Description/overview first
- Statistics section with tables
- Related content cards
- Gallery at bottom
- Sticky table of contents

---

## 9. LISTING/BROWSE PAGE LAYOUT

### Creature Listing Example

```
┌─────────────────────────────────────────────────────────────┐
│                      HEADER                                  │
├─────────────┬───────────────────────────────┬────────────────┤
│  SIDEBAR    │ Home > Creatures              │                │
│  FILTERS    │                               │  TABLE OF      │
│  ┌────────┐ │ ## Creatures                  │  CONTENTS      │
│  │ Search │ │ Found 245 creatures           │  • Bosses      │
│  ├────────┤ │ [Sort ▼] [View as Grid ■]    │  • Monsters    │
│  │ Type   │ │                               │  • Summons     │
│  │ ☑ All  │ │ ┌─────────┬─────────┬──────┐ │                │
│  │ ☐ Boss │ │ │ Demon   │ Pig     │ Wolf │ │                │
│  │        │ │ │ [Image] │ [Image] │[Img] │ │                │
│  │Diff    │ │ │ Boss    │ Monster │Mob   │ │                │
│  │ ☐ Easy │ │ ├─────────┼─────────┼──────┤ │                │
│  │ ☐ Hard │ │ │ Slime   │ Orc     │Troll │ │                │
│  │        │ │ │ [Image] │ [Image] │[Img] │ │                │
│  │[Apply] │ │ │ Monster │ Boss    │Boss  │ │                │
│  └────────┘ │ └─────────┴─────────┴──────┘ │                │
│             │                               │                │
│             │ [1] [2] [3] [>] Next         │                │
│             │                               │                │
├─────────────┴───────────────────────────────┴────────────────┤
│                       FOOTER                                  │
└─────────────────────────────────────────────────────────────┘
```

**Features:**
- Expandable left sidebar for filters
- Multiple view options (grid, list, table)
- Sorting dropdown
- Search within results
- Card-based display for grid view
- Pagination at bottom
- Results counter

---

## 10. STYLING GUIDELINES

### Colors Used Across Pages
```
Primary: #003399 (Links, highlights, CTA buttons)
Secondary: #0645ad (Links on hover)
Visited: #663399 (Visited links)
Background: #FFFFFF (Main) / #F6F6F6 (Containers)
Text: #222222 (Body) / #333333 (Headers)
Borders: #CCCCCC (Primary) / #DDDDDD (Light)
Gray: #999999 (Tertiary text)
Success: #00AA00
Warning: #FF6600
Error: #CC0000
```

### Font Stack
- Headings: `"Georgia", serif` or clean sans-serif
- Body: `-apple-system, BlinkMacSystemFont, "Segoe UI", "Roboto", sans-serif`
- Monospace: `"Courier New", monospace`
- Font weights: 400 (regular), 600 (medium), 700 (bold)

### Shadows & Elevation
- Subtle: `0 1px 3px rgba(0,0,0,0.12)`
- Medium: `0 4px 6px rgba(0,0,0,0.16)`
- Cards: Subtle shadow on hover
- Infobox: Light box-shadow or border

---

## 11. INTERACTION PATTERNS

### Hover States
- **Links**: Underline appears, color changes to secondary
- **Buttons**: Background darkens, shadow appears
- **Cards**: Subtle lift effect (shadow increases)
- **Table rows**: Light background color (#f0f0f0)

### Click/Active States
- **Buttons**: Pressed down appearance
- **Tabs**: Underline and bold
- **Navigation items**: Highlighted in sidebar

### Loading States
- **Spinner**: Centered circular loader
- **Placeholder**: Skeleton screens for content areas
- **Timeout message**: "Loading... This is taking longer than expected"

### Empty States
```
╔═══════════════════════════════════╗
║                                   ║
║  🔍 No results found              ║
║                                   ║
║  Try adjusting your filters or    ║
║  search terms                     ║
║                                   ║
╚═══════════════════════════════════╝
```

---

## 12. ACCESSIBILITY FEATURES

### WCAG 2.1 AA Compliance
- **Color contrast**: Minimum 4.5:1 for text
- **Focus states**: Visible keyboard focus indicators
- **Skip links**: "Skip to main content" at top
- **ARIA labels**: For icon buttons and interactive elements
- **Semantic HTML**: Proper heading hierarchy
- **Alt text**: All images have descriptive alt text
- **Form labels**: Associated with inputs
- **Keyboard navigation**: Tab-able to all interactive elements

---

## 13. FANDOM-SPECIFIC PATTERNS ADOPTED

### What Works (Should implement):
1. **Sticky sidebar navigation** - Excellent UX for browsing
2. **Auto-generated table of contents** - Helps users navigate long pages
3. **Infobox pattern** - Efficiently displays key entity data
4. **Related content cards** - Encourages exploration
5. **Breadcrumb navigation** - Clear context
6. **Category/tag system** - Helps organization
7. **Search + filtering** - Essential for large datasets
8. **Responsive grid layouts** - Adapts to all screen sizes
9. **Color-coded badges** - Quick visual scanning
10. **Version history/editing hints** - Community wiki aspect

### What to Avoid/Improve (For Nightmares Wiki):
1. **Reduce ads** - Cleaner interface (Fandom has heavy ad load)
2. **Minimize clutter** - Focus on content
3. **Better typography** - Modern font choices
4. **Clearer hierarchy** - More visual distinction
5. **Faster load times** - Optimize assets
6. **Better dark mode** - Optional dark theme support

---

## SUMMARY

The Nightmares Wiki should adopt Fandom's proven UX patterns while improving clarity and aesthetics. Key takeaways:

- **Familiar structure** users already understand from Fandom wikis
- **Effective information architecture** for game data
- **Responsive and accessible** for all users
- **Scalable component system** for consistency
- **Strong navigation patterns** for discovery
- **Professional appearance** without the ad clutter

This creates a user experience that feels natural to wiki users while showcasing your game world with quality presentation.
