# Nightmares Wiki - UI Component Specifications

## Component Library for the Nightmares Wiki

This document defines all UI components needed for the Nightmares Wiki, including their visual design, props/configuration, and usage examples.

---

# TABLE OF CONTENTS

1. [Layout Components](#layout-components)
2. [Navigation Components](#navigation-components)
3. [Entity Display Components](#entity-display-components)
4. [Data Presentation Components](#data-presentation-components)
5. [Form & Input Components](#form--input-components)
6. [Interaction & Feedback Components](#interaction--feedback-components)
7. [Content Components](#content-components)
8. [Media Components](#media-components)

---

# LAYOUT COMPONENTS

## 1. Page Layout Wrapper

**Component Name:** `PageLayout`

**Purpose:** Main layout container that manages the three-column structure and responsive behavior.

**Visual Design:**
- Desktop (1200px+): Sidebar (18%) | Content (60-65%) | Right Sidebar (17-22%)
- Tablet (768-1199px): Sidebar (hamburger) | Content (full-width) | Right Sidebar (integrated)
- Mobile (<768px): Full-width single column

**Props:**
```typescript
interface PageLayoutProps {
  sidebarContent?: ReactNode;      // Left navigation sidebar
  mainContent: ReactNode;           // Main article/content area
  rightSidebarContent?: ReactNode; // TOC, infobox, related content
  showSidebar?: boolean;            // Toggle sidebar visibility
  sidebarSticky?: boolean;          // Make sidebar sticky on scroll
  rightSidebarSticky?: boolean;     // Make right sidebar sticky
  maxWidth?: string;                // Max container width (default: 1400px)
  contentGap?: string;              // Spacing between columns (default: 20px)
}
```

**Responsive Behavior:**
- Hamburger menu on mobile for left sidebar
- Right sidebar collapses into main column on tablet
- Full stacking on mobile

**Usage Example:**
```jsx
<PageLayout
  sidebarContent={<NavigationSidebar />}
  mainContent={<CreatureDetailContent />}
  rightSidebarContent={<TableOfContents />}
  sidebarSticky={true}
  rightSidebarSticky={true}
/>
```

**CSS Classes:** `.page-layout`, `.page-layout--sticky`, `.page-layout--mobile`

---

## 2. Card Container

**Component Name:** `Card`

**Purpose:** Flexible container for grouped content with consistent styling and spacing.

**Visual Design:**
- Border: 1px solid #ddd
- Border-radius: 4px
- Background: #ffffff
- Shadow: 0 1px 3px rgba(0,0,0,0.1)
- Hover: Box shadow increases to 0 4px 8px rgba(0,0,0,0.15)
- Padding: 16px
- Margin: 16px (configurable)

**Props:**
```typescript
interface CardProps {
  children: ReactNode;
  className?: string;
  shadow?: 'none' | 'subtle' | 'medium' | 'large'; // default: subtle
  onClick?: () => void;
  elevated?: boolean;           // Increases shadow on hover
  interactive?: boolean;        // Adds cursor pointer and hover effects
  padding?: string | number;    // default: 16px
  variant?: 'default' | 'highlight' | 'muted'; // Color variant
}
```

**Visual Variants:**
- `default`: White bg, light border, subtle shadow
- `highlight`: Light blue bg (#f0f5ff), colored border
- `muted`: Light gray bg (#f6f6f6), minimal shadow

**Usage Example:**
```jsx
<Card elevated interactive>
  <h3>Demon (Boss)</h3>
  <p>A fearsome creature of darkness...</p>
</Card>
```

**CSS Classes:** `.card`, `.card--elevated`, `.card--interactive`, `.card--highlight`

---

## 3. Section Container

**Component Name:** `Section`

**Purpose:** Semantic section wrapper with consistent spacing and optional background.

**Visual Design:**
- Margin-top: 24px
- Margin-bottom: 24px
- Padding: 16px (optional background)
- Background: White or light gray (#f6f6f6)
- Border-top: 1px solid #eee (optional)

**Props:**
```typescript
interface SectionProps {
  children: ReactNode;
  title?: string;               // Optional section heading
  background?: 'white' | 'gray'; // default: white
  divider?: boolean;            // Add top border
  spacing?: 'compact' | 'normal' | 'loose'; // Margin/padding
  id?: string;                  // For TOC anchoring
}
```

**Usage Example:**
```jsx
<Section title="Statistics" background="gray" divider>
  <StatsTable stats={demonStats} />
</Section>
```

**CSS Classes:** `.section`, `.section--gray`, `.section--divider`

---

# NAVIGATION COMPONENTS

## 4. Breadcrumb Navigation

**Component Name:** `Breadcrumb`

**Purpose:** Show the current page's location in the site hierarchy.

**Visual Design:**
```
Home > Creatures > Bosses > Demon
```
- Font-size: 12-13px
- Color: #666
- Separator: " > " or "/" 
- Last item: Darker color (#222) or non-clickable
- Hover: Links underline on hover

**Props:**
```typescript
interface BreadcrumbItem {
  label: string;
  href?: string;
  current?: boolean;
}

interface BreadcrumbProps {
  items: BreadcrumbItem[];
  separator?: string;           // default: " > "
  variant?: 'text' | 'icon';   // Icon separators optional
}
```

**Usage Example:**
```jsx
<Breadcrumb
  items={[
    { label: 'Home', href: '/' },
    { label: 'Creatures', href: '/creatures' },
    { label: 'Bosses', href: '/creatures?type=boss' },
    { label: 'Demon', current: true }
  ]}
/>
```

**Accessibility:** 
- Use `<nav aria-label="breadcrumb">` wrapper
- Current page not clickable
- Keyboard navigable

**CSS Classes:** `.breadcrumb`, `.breadcrumb__item`, `.breadcrumb__separator`

---

## 5. Sidebar Navigation

**Component Name:** `SidebarNav`

**Purpose:** Hierarchical navigation menu with expandable categories.

**Visual Design:**
- Width: 250px (desktop)
- Background: #ffffff or #f9f9f9
- Borders: 1px solid #e6e6e6
- Font-size: 14px
- Current page highlight: Left border 3px solid #003399, bold text
- Hover: Light background #f0f0f0

**Props:**
```typescript
interface NavItem {
  label: string;
  href?: string;
  icon?: ReactNode;
  children?: NavItem[];
  active?: boolean;
  expandedByDefault?: boolean;
}

interface SidebarNavProps {
  items: NavItem[];
  onNavigate?: (path: string) => void;
  sticky?: boolean;              // Position: sticky
  collapsible?: boolean;         // Show collapse button on mobile
  showIcons?: boolean;           // Show icon if available
}
```

**Responsive Behavior:**
- Desktop: Full sidebar
- Tablet: Collapsible/hamburger menu
- Mobile: Full-width drawer overlay

**Usage Example:**
```jsx
<SidebarNav
  items={[
    { label: 'Home', href: '/', icon: <HomeIcon /> },
    {
      label: 'Creatures',
      icon: <CreatureIcon />,
      children: [
        { label: 'Bosses', href: '/creatures?type=boss' },
        { label: 'Mobs', href: '/creatures?type=mob' },
        { label: 'Summons', href: '/creatures?type=summon' }
      ]
    },
    { label: 'Items', href: '/items', icon: <ItemIcon /> }
  ]}
  sticky
/>
```

**CSS Classes:** `.sidebar-nav`, `.sidebar-nav__item--active`, `.sidebar-nav__item--expanded`

---

## 6. Table of Contents

**Component Name:** `TableOfContents`

**Purpose:** Auto-generated navigation for headings on the current page.

**Visual Design:**
- Width: 250-300px
- Font-size: 12-13px
- Nesting: Indented by 16px per level
- Active/current: Bold text, left border 2px solid #003399
- Link color: #0645ad (blue)
- Border: 1px solid #e6e6e6

**Props:**
```typescript
interface HeadingItem {
  id: string;
  text: string;
  level: number; // 1-6 (h1-h6)
  children?: HeadingItem[];
}

interface TableOfContentsProps {
  headings: HeadingItem[];  // Auto-extracted from H2/H3 tags
  onHeadingClick?: (id: string) => void;
  activeHeading?: string;   // ID of currently viewed heading
  sticky?: boolean;         // Sticky positioning
  maxDepth?: number;        // Max nesting level to show (default: 3)
}
```

**Auto-Generation:**
- Scans for `<h2>` and `<h3>` tags in main content
- Auto-generates `id` attributes if missing
- Updates active heading as user scrolls

**Usage Example:**
```jsx
<TableOfContents
  headings={extractHeadingsFromDOM(contentRef)}
  activeHeading={currentHeadingId}
  sticky
/>
```

**Keyboard Navigation:**
- Arrow keys to navigate
- Enter to jump to section

**CSS Classes:** `.toc`, `.toc__item--active`, `.toc__item--level-1`, `.toc__item--level-2`

---

# ENTITY DISPLAY COMPONENTS

## 7. Infobox

**Component Name:** `Infobox`

**Purpose:** Display key entity information in a compact, visually distinct box (floats right on desktop).

**Visual Design:**
```
╔════════════════════════╗
║   [Image]             ║
╠════════════════════════╣
║ Demon                 ║  Title
╠════════════════════════╣
║ HP: 220              ║
║ Type: Boss           ║  Key-value pairs
║ Location: Castle     ║
╠════════════════════════╣
║ • Loot Item 1 (50%)  ║
║ • Loot Item 2 (20%)  ║
╚════════════════════════╝
```

- Width: 280-320px
- Border: 1px solid #bbb or #ccc
- Background: #f6f6f6 or white
- Border-radius: 4px
- Shadow: Subtle 0 1px 3px rgba(0,0,0,0.12)
- Margin: 16px 0 16px 16px (float: right on desktop)

**Props:**
```typescript
interface InfoboxSection {
  items: { label: string; value: ReactNode }[];
}

interface InfoboxProps {
  image?: {
    src: string;
    alt: string;
    caption?: string;
  };
  title: string;
  sections: (InfoboxSection | ReactNode)[];
  actions?: ReactNode[];     // Buttons at bottom
  className?: string;
  variant?: 'default' | 'creature' | 'item' | 'location';
}
```

**Responsive Behavior:**
- Desktop (1200px+): Float right, width 320px
- Tablet (768-1199px): Full-width at top
- Mobile (<768px): Full-width, stack at top before content

**Usage Example:**
```jsx
<Infobox
  image={{ src: '/demon.png', alt: 'Demon Boss' }}
  title="Demon"
  sections={[
    {
      items: [
        { label: 'Type', value: 'Boss' },
        { label: 'HP', value: '220' },
        { label: 'Location', value: 'Castle Depths' }
      ]
    },
    { items: [{ label: 'Loot', value: <LootList items={loot} /> }] }
  ]}
  actions={[
    <Button>Add to Inventory</Button>,
    <Button variant="secondary">Compare</Button>
  ]}
/>
```

**CSS Classes:** `.infobox`, `.infobox__title`, `.infobox__section`, `.infobox__image`

---

## 8. Entity Card

**Component Name:** `EntityCard`

**Purpose:** Small card for displaying entities in grids/lists (creatures, items, NPCs).

**Visual Design:**
```
┌──────────────────┐
│   [Image]        │
│   (Aspect 1:1)   │
├──────────────────┤
│ Entity Name      │
│ Type Badge       │
│ Brief Info       │
├──────────────────┤
│ [View Details]   │
└──────────────────┘
```

- Width: 200-250px (flexible)
- Aspect ratio: 1:1 for images
- Border: 1px solid #ddd
- Background: White
- Border-radius: 4px
- Hover: Shadow increases, slight scale (1.02x)
- Padding: 12px

**Props:**
```typescript
interface EntityCardProps {
  id: string;
  image: { src: string; alt: string };
  title: string;
  type: 'creature' | 'item' | 'npc' | 'location';
  description?: string;
  badges?: { label: string; variant?: string }[];
  stats?: { label: string; value: string }[];
  href?: string;
  onClick?: () => void;
  size?: 'small' | 'medium' | 'large'; // default: medium
  interactive?: boolean;
}
```

**Grid Layout:**
- Desktop: 4 columns (calc(25% - 15px))
- Tablet: 3 columns (calc(33.33% - 15px))
- Mobile: 2 columns (calc(50% - 15px))
- Gap: 20px

**Usage Example:**
```jsx
<EntityCard
  id="demon-123"
  title="Demon"
  type="creature"
  image={{ src: '/demon.png', alt: 'Demon Boss' }}
  description="Boss-level creature"
  badges={[
    { label: 'Boss', variant: 'danger' },
    { label: 'Undead', variant: 'dark' }
  ]}
  stats={[
    { label: 'HP', value: '220' },
    { label: 'XP', value: '440' }
  ]}
  href="/creatures/demon"
  interactive
/>
```

**CSS Classes:** `.entity-card`, `.entity-card--creature`, `.entity-card--item`, `.entity-card--interactive`

---

# DATA PRESENTATION COMPONENTS

## 9. Statistics Table

**Component Name:** `StatsTable`

**Purpose:** Structured display of numeric/key-value data in table format.

**Visual Design:**
```
┌──────────────────┬──────────┐
│ Stat             │ Value    │
├──────────────────┼──────────┤
│ Hit Points       │ 220      │
│ Experience       │ 440      │
│ Armor Rating     │ 13       │
└──────────────────┴──────────┘
```

- Border-collapse: Collapse
- Header background: #e6e6e6 or #f0f0f0
- Header text: Bold, color #222
- Row borders: 1px solid #ddd
- Row striping: Alternate #fff and #f9f9f9
- Cell padding: 10px 12px
- Font-size: 13-14px
- Text alignment: Left for labels, center/right for values

**Props:**
```typescript
interface TableRow {
  label: string;
  value: string | number | ReactNode;
  unit?: string;     // Optional unit (e.g., "%", "lbs")
  comparison?: string; // Optional comparison text
}

interface StatsTableProps {
  rows: TableRow[];
  columns?: 2 | 3; // default: 2
  variant?: 'default' | 'compact' | 'detailed';
  striped?: boolean;
  bordered?: boolean;
  hoverable?: boolean;
  sortable?: boolean;
  className?: string;
}
```

**Variants:**
- `default`: Full padding, clear borders
- `compact`: Reduced padding, minimal borders
- `detailed`: Extra columns for descriptions/comparisons

**Usage Example:**
```jsx
<StatsTable
  rows={[
    { label: 'Hit Points', value: 220, unit: 'HP' },
    { label: 'Experience', value: 440, unit: 'XP' },
    { label: 'Armor Rating', value: 13 },
    { label: 'Melee Damage', value: '100-210' },
    { label: 'Magic Resist', value: '40%' }
  ]}
  striped
  hoverable
/>
```

**Responsive Behavior:**
- Desktop: Multi-column layout
- Mobile: Stack into single column or horizontal scroll

**CSS Classes:** `.stats-table`, `.stats-table--striped`, `.stats-table--hoverable`

---

## 10. Loot Table

**Component Name:** `LootTable`

**Purpose:** Display drop table items with rarity, quantity, and percentage.

**Visual Design:**
```
┌────────────────────┬─────────┬────┐
│ Item               │ Rarity  │ %  │
├────────────────────┼─────────┼────┤
│ Gold Coin (50-200) │ Common  │ 70 │
│ Demonic Essence    │ Rare    │ 10 │
│ Dark Staff         │ Epic    │ 3  │
└────────────────────┴─────────┴────┘
```

- Rarity badges with color coding:
  - Common: Gray (#999)
  - Uncommon: Green (#00aa00)
  - Rare: Blue (#0645ad)
  - Epic: Purple (#663399)
  - Legendary: Orange (#ff8800)
- Percentage bar optional
- Item links to item detail page

**Props:**
```typescript
interface LootItem {
  id: string;
  name: string;
  minQuantity?: number;
  maxQuantity?: number;
  rarity: 'common' | 'uncommon' | 'rare' | 'epic' | 'legendary';
  dropChance: number; // 0-100
  href?: string;
}

interface LootTableProps {
  items: LootItem[];
  showChanceBar?: boolean;
  showQuantity?: boolean;
  sortByChance?: boolean;
  className?: string;
}
```

**Responsive Behavior:**
- Desktop: Full table with all columns
- Mobile: Stack into cards or abbreviated table

**Usage Example:**
```jsx
<LootTable
  items={[
    { name: 'Gold Coin (50-200)', rarity: 'common', dropChance: 70, minQuantity: 50, maxQuantity: 200 },
    { name: 'Demonic Essence', rarity: 'rare', dropChance: 10 },
    { name: 'Dark Staff', rarity: 'epic', dropChance: 3 }
  ]}
  showChanceBar
  showQuantity
/>
```

**CSS Classes:** `.loot-table`, `.loot-item`, `.loot-item--rare`, `.loot-item--epic`

---

## 11. Comparison Table

**Component Name:** `ComparisonTable`

**Purpose:** Side-by-side comparison of multiple entities (items, creatures, etc.).

**Visual Design:**
- Column-based layout
- First column: Label
- Remaining columns: Entity data
- Header row: Entity names/images
- Highlight differences
- Color-code better values

**Props:**
```typescript
interface ComparisonEntity {
  id: string;
  name: string;
  image?: { src: string; alt: string };
  data: Record<string, string | number>;
}

interface ComparisonTableProps {
  entities: ComparisonEntity[];
  attributes: { key: string; label: string }[];
  highlightBest?: boolean;
  highlightDifferences?: boolean;
  className?: string;
}
```

**Usage Example:**
```jsx
<ComparisonTable
  entities={[
    { id: 'demon', name: 'Demon', data: { hp: 220, dmg: '100-210' } },
    { id: 'shadow', name: 'Shadow Beast', data: { hp: 180, dmg: '80-150' } }
  ]}
  attributes={[
    { key: 'hp', label: 'Hit Points' },
    { key: 'dmg', label: 'Damage' }
  ]}
  highlightBest
/>
```

**CSS Classes:** `.comparison-table`, `.comparison-table__best`, `.comparison-table__differs`

---

# FORM & INPUT COMPONENTS

## 12. Search Box

**Component Name:** `SearchBox`

**Purpose:** Global and local search input with autocomplete.

**Visual Design:**
```
┌─────────────────────────────────────────────┐
│ 🔍 Search the wiki...           [Clear ✕]  │
└─────────────────────────────────────────────┘
```

- Height: 40-44px
- Border: 1px solid #ddd
- Border-radius: 4px
- Padding: 8px 12px
- Icon: 20px left padding
- Focus: Border color changes to #003399
- Autocomplete dropdown below

**Props:**
```typescript
interface SearchBoxProps {
  placeholder?: string;
  onSearch?: (query: string) => void;
  onResultSelect?: (result: SearchResult) => void;
  showResults?: boolean;
  results?: SearchResult[];
  loading?: boolean;
  debounceMs?: number;        // default: 300
  minChars?: number;          // default: 2
  categories?: string[];      // Filter by type
  className?: string;
}
```

**Autocomplete Results:**
```
┌──────────────────────────────────────┐
│ Demon (Creature)                     │
│ Demonic Essence (Item)               │
│ Demon's Vale (Location)              │
│ View all results for "Demon" (12)    │
└──────────────────────────────────────┘
```

**Usage Example:**
```jsx
<SearchBox
  placeholder="Search creatures, items, NPCs..."
  onSearch={handleSearch}
  onResultSelect={handleSelectResult}
  showResults={showResults}
  results={searchResults}
  categories={['creatures', 'items', 'npcs']}
/>
```

**Keyboard Shortcuts:**
- Arrow up/down: Navigate results
- Enter: Select highlighted result
- Escape: Close results

**CSS Classes:** `.search-box`, `.search-box__input`, `.search-box__results`

---

## 13. Filter Sidebar

**Component Name:** `FilterSidebar`

**Purpose:** Multi-field filtering for listing/search pages.

**Visual Design:**
- Width: 250px (desktop)
- Background: #f6f6f6
- Border: 1px solid #ddd
- Sections: Collapsible categories
- Checkboxes: Standard browser style or custom
- Apply button: Full-width at bottom

**Props:**
```typescript
interface FilterOption {
  id: string;
  label: string;
  value: string | number;
  count?: number;
}

interface FilterSection {
  id: string;
  title: string;
  type: 'checkbox' | 'radio' | 'range' | 'select';
  options?: FilterOption[];
  min?: number;
  max?: number;
  expandedByDefault?: boolean;
}

interface FilterSidebarProps {
  sections: FilterSection[];
  onApply?: (filters: Record<string, any>) => void;
  onChange?: (filters: Record<string, any>) => void;
  onClear?: () => void;
  showCounts?: boolean;
  sticky?: boolean;
}
```

**Usage Example:**
```jsx
<FilterSidebar
  sections={[
    {
      id: 'type',
      title: 'Type',
      type: 'checkbox',
      options: [
        { id: 'boss', label: 'Bosses', value: 'boss', count: 12 },
        { id: 'mob', label: 'Mobs', value: 'mob', count: 45 },
        { id: 'summon', label: 'Summons', value: 'summon', count: 8 }
      ]
    },
    {
      id: 'difficulty',
      title: 'Difficulty',
      type: 'range',
      min: 1,
      max: 100
    }
  ]}
  onApply={handleFilterApply}
  sticky
/>
```

**Responsive Behavior:**
- Desktop: Side panel
- Tablet: Collapsible drawer
- Mobile: Full-screen overlay

**CSS Classes:** `.filter-sidebar`, `.filter-section`, `.filter-section--expanded`

---

# INTERACTION & FEEDBACK COMPONENTS

## 14. Button

**Component Name:** `Button`

**Purpose:** Clickable action trigger.

**Visual Design:**

**Primary Button:**
- Background: #003399 (brand color)
- Color: White
- Padding: 10px 20px
- Border-radius: 4px
- Border: None
- Font-weight: 600
- Cursor: pointer
- Hover: Background #002a70 (darker)
- Active: Box-shadow inset
- Disabled: Opacity 0.5, cursor not-allowed

**Secondary Button:**
- Background: White
- Color: #003399
- Border: 1px solid #003399
- Padding: 10px 20px
- Hover: Background #f0f5ff

**Tertiary/Link Button:**
- Background: Transparent
- Color: #0645ad (link blue)
- Border: None
- Text-decoration: Underline on hover

**Props:**
```typescript
interface ButtonProps {
  children: ReactNode;
  onClick?: () => void;
  variant?: 'primary' | 'secondary' | 'tertiary' | 'danger';
  size?: 'small' | 'medium' | 'large'; // default: medium
  disabled?: boolean;
  loading?: boolean;
  icon?: ReactNode;              // Optional icon
  iconPosition?: 'left' | 'right'; // default: left
  fullWidth?: boolean;
  type?: 'button' | 'submit' | 'reset';
  className?: string;
}
```

**Sizes:**
- small: 8px 16px, font 12px
- medium: 10px 20px, font 14px
- large: 12px 28px, font 16px

**Usage Example:**
```jsx
<Button variant="primary" size="medium" onClick={handleAction}>
  Click Me
</Button>

<Button variant="secondary">Secondary Action</Button>

<Button variant="danger" disabled>Delete (Disabled)</Button>

<Button icon={<IconComponent />}>Icon Button</Button>
```

**Accessibility:**
- Focus state: Visible outline
- Disabled state: Proper aria attributes

**CSS Classes:** `.btn`, `.btn--primary`, `.btn--secondary`, `.btn--large`, `.btn--disabled`

---

## 15. Badge

**Component Name:** `Badge`

**Purpose:** Small label/tag for categorization and quick visual identification.

**Visual Design:**
```
[Status: Active] [Difficulty: Hard] [Type: Boss]
```

- Padding: 4px 8px / 6px 12px
- Border-radius: 3px or 16px (pill)
- Font-size: 11-12px
- Font-weight: 600
- Display: inline-block
- Margin: 4px 8px 4px 0

**Color Variants:**
- `default`: #e6e6e6 (gray)
- `primary`: #e6f0ff (blue)
- `success`: #d4f4dd (green)
- `danger`: #ffe6e6 (red)
- `warning`: #fff4e6 (orange)
- `dark`: #f0f0f0 (dark gray)

**Props:**
```typescript
interface BadgeProps {
  children: ReactNode;
  variant?: 'default' | 'primary' | 'success' | 'danger' | 'warning' | 'dark';
  shape?: 'rounded' | 'pill'; // default: rounded
  size?: 'small' | 'medium'; // default: medium
  closeable?: boolean;
  onClose?: () => void;
  icon?: ReactNode;
}
```

**Usage Example:**
```jsx
<Badge variant="danger" shape="pill">Boss</Badge>
<Badge variant="warning">Rare</Badge>
<Badge variant="success" closeable onClose={handleClose}>Active</Badge>
```

**CSS Classes:** `.badge`, `.badge--danger`, `.badge--pill`, `.badge--closeable`

---

## 16. Tabs

**Component Name:** `Tabs`

**Purpose:** Organize content into multiple related sections with tab navigation.

**Visual Design:**
```
[Active Tab] [Inactive Tab] [Inactive Tab]
────────────────────────────────────────────
Content for active tab...
```

- Tab height: 44px
- Tab padding: 0 16px
- Font-size: 14px
- Active underline: 3px solid #003399
- Inactive text: Gray (#666)
- Border-bottom: 1px solid #ddd
- Hover on inactive: Background #f5f5f5

**Props:**
```typescript
interface Tab {
  id: string;
  label: string;
  icon?: ReactNode;
  disabled?: boolean;
}

interface TabsProps {
  tabs: Tab[];
  activeTab?: string;
  onTabChange?: (tabId: string) => void;
  variant?: 'underline' | 'pills' | 'boxed'; // default: underline
  size?: 'small' | 'medium' | 'large';
  className?: string;
}
```

**Variants:**
- `underline`: Underline on active (default)
- `pills`: Pill-shaped background on active
- `boxed`: Boxed background on all tabs

**Usage Example:**
```jsx
<Tabs
  tabs={[
    { id: 'combat', label: 'Combat', icon: <CombatIcon /> },
    { id: 'movement', label: 'Movement', icon: <MovementIcon /> },
    { id: 'abilities', label: 'Abilities' }
  ]}
  activeTab={selectedTab}
  onTabChange={setSelectedTab}
  variant="underline"
/>
```

**Responsive Behavior:**
- Desktop: Full tab bar
- Tablet: Scrollable tab bar if needed
- Mobile: Consider converting to accordion on very small screens

**Accessibility:**
- ARIA roles: `tablist`, `tab`, `tabpanel`
- Keyboard: Arrow keys to navigate, Enter to select

**CSS Classes:** `.tabs`, `.tabs__tab--active`, `.tabs__panel`, `.tabs--pills`

---

## 17. Accordion

**Component Name:** `Accordion`

**Purpose:** Collapsible sections for expandable content (mobile-friendly tabs alternative).

**Visual Design:**
- Header: Clickable bar with chevron icon
- Expanded: Shows content below, chevron rotates
- Collapsed: Only shows header
- Border: 1px solid #ddd between items

**Props:**
```typescript
interface AccordionItem {
  id: string;
  title: string;
  content: ReactNode;
  icon?: ReactNode;
  disabled?: boolean;
}

interface AccordionProps {
  items: AccordionItem[];
  activeItems?: string[];
  onToggle?: (itemId: string) => void;
  allowMultiple?: boolean;     // default: false
  variant?: 'default' | 'filled';
}
```

**Usage Example:**
```jsx
<Accordion
  items={[
    { id: 'stats', title: 'Statistics', content: <StatsTable /> },
    { id: 'abilities', title: 'Abilities', content: <AbilitiesList /> },
    { id: 'loot', title: 'Loot Table', content: <LootTable /> }
  ]}
  activeItems={['stats']}
  onToggle={handleToggle}
/>
```

**CSS Classes:** `.accordion`, `.accordion__item--expanded`, `.accordion__header`, `.accordion__content`

---

## 18. Modal / Dialog

**Component Name:** `Modal`

**Purpose:** Display important content in an overlay that requires user interaction.

**Visual Design:**
- Overlay: Semi-transparent black background (rgba(0,0,0,0.5))
- Modal box: White, border-radius 8px
- Width: 90% on mobile, 500-700px on desktop
- Padding: 24px
- Close button: X icon top-right
- Stacked z-index above other content

**Props:**
```typescript
interface ModalProps {
  isOpen: boolean;
  onClose?: () => void;
  title?: string;
  children: ReactNode;
  footer?: ReactNode;
  size?: 'small' | 'medium' | 'large'; // default: medium
  closeOnEscape?: boolean;
  closeOnBackdropClick?: boolean;
  className?: string;
}
```

**Usage Example:**
```jsx
<Modal
  isOpen={showModal}
  onClose={handleCloseModal}
  title="Confirm Action"
  size="medium"
>
  <p>Are you sure you want to delete this item?</p>
  <div className="modal__footer">
    <Button variant="secondary" onClick={handleCloseModal}>Cancel</Button>
    <Button variant="danger" onClick={handleConfirmDelete}>Delete</Button>
  </div>
</Modal>
```

**Accessibility:**
- Focus trap within modal
- ARIA roles: `dialog`, `alertdialog`
- Keyboard: Escape to close

**CSS Classes:** `.modal`, `.modal__overlay`, `.modal__content`, `.modal__header`, `.modal__footer`

---

## 19. Tooltip

**Component Name:** `Tooltip`

**Purpose:** Display contextual information on hover.

**Visual Design:**
- Background: Dark gray/black (#333-444)
- Color: White
- Padding: 8px 12px
- Border-radius: 4px
- Font-size: 12px
- Max-width: 250px
- Arrow pointer to target element
- Z-index: High (above most elements)

**Props:**
```typescript
interface TooltipProps {
  children: ReactNode;
  content: ReactNode;
  placement?: 'top' | 'bottom' | 'left' | 'right'; // default: top
  delay?: number;             // ms before showing
  className?: string;
}
```

**Usage Example:**
```jsx
<Tooltip content="Click to view more details" placement="top">
  <span>Hover me</span>
</Tooltip>
```

**CSS Classes:** `.tooltip`, `.tooltip--top`, `.tooltip--bottom`

---

# CONTENT COMPONENTS

## 20. Hero Image / Banner

**Component Name:** `HeroBanner`

**Purpose:** Large decorative image at top of page.

**Visual Design:**
- Height: 300-400px on desktop, 200-250px on mobile
- Width: 100%
- Object-fit: Cover
- Optional overlay with text
- Border-bottom: 1px solid #ddd

**Props:**
```typescript
interface HeroBannerProps {
  image: { src: string; alt: string };
  height?: string | number;
  overlay?: {
    color?: string;
    opacity?: number;
    content?: ReactNode;
  };
  blur?: boolean;
}
```

**Usage Example:**
```jsx
<HeroBanner
  image={{ src: '/demon-banner.png', alt: 'Demon Boss Battle' }}
  height={300}
  overlay={{
    content: <h1>Demon Boss Guide</h1>,
    opacity: 0.4
  }}
/>
```

**CSS Classes:** `.hero-banner`, `.hero-banner__overlay`

---

## 21. Rich Text Editor / Markdown Viewer

**Component Name:** `RichTextContent`

**Purpose:** Display formatted text content (support markdown or HTML).

**Props:**
```typescript
interface RichTextContentProps {
  content: string;           // Markdown or HTML string
  format?: 'markdown' | 'html'; // default: markdown
  className?: string;
}
```

**Usage Example:**
```jsx
<RichTextContent
  content="# The Demon\n\nA fearsome creature of darkness..."
  format="markdown"
/>
```

**CSS Classes:** `.rich-text`, `.rich-text h2`, `.rich-text p`

---

## 22. Image Gallery

**Component Name:** `Gallery`

**Purpose:** Display multiple images with lightbox.

**Visual Design:**
- Grid layout (3-4 columns)
- Thumbnails with 16:9 aspect ratio
- Hover: Slight zoom, play icon overlay
- Lightbox: Full-screen image with navigation

**Props:**
```typescript
interface GalleryImage {
  src: string;
  thumbnail?: string;
  alt: string;
  caption?: string;
}

interface GalleryProps {
  images: GalleryImage[];
  columns?: 2 | 3 | 4; // default: 3
  gap?: string;        // default: 16px
  enableLightbox?: boolean;
  className?: string;
}
```

**Usage Example:**
```jsx
<Gallery
  images={[
    { src: '/demon-full.png', thumbnail: '/demon-thumb.png', alt: 'Demon' },
    { src: '/demon-2.png', alt: 'Demon Combat' }
  ]}
  columns={3}
  enableLightbox
/>
```

**Lightbox Features:**
- Next/previous navigation
- Close button
- Image counter
- Keyboard navigation (arrow keys, escape)

**CSS Classes:** `.gallery`, `.gallery__item`, `.gallery__lightbox`

---

## 23. Related Items/Links Component

**Component Name:** `RelatedItems`

**Purpose:** Display related entities or pages.

**Visual Design:**
```
## Related Creatures
[Card] [Card] [Card]
[Card] [Card]
```

- Grid of entity cards (3-4 columns)
- Title above
- Cards clickable to detail pages

**Props:**
```typescript
interface RelatedItemsProps {
  title: string;
  items: RelatedItem[];
  itemType: 'creatures' | 'items' | 'npcs' | 'quests';
  columns?: 3 | 4; // default: 3
  limit?: number;  // Max items to show
}

interface RelatedItem {
  id: string;
  title: string;
  image?: { src: string; alt: string };
  href: string;
  relationship?: string; // e.g., "Boss", "Related Drop", "Quest Giver"
}
```

**Usage Example:**
```jsx
<RelatedItems
  title="Related Creatures"
  items={[
    { id: 'shadow', title: 'Shadow Beast', href: '/creatures/shadow' },
    { id: 'lesser', title: 'Lesser Demon', href: '/creatures/lesser-demon' }
  ]}
  itemType="creatures"
  columns={3}
/>
```

**CSS Classes:** `.related-items`, `.related-items__grid`

---

# MEDIA COMPONENTS

## 24. Image Component

**Component Name:** `Image`

**Purpose:** Optimized image display with lazy loading and responsive sizing.

**Props:**
```typescript
interface ImageProps {
  src: string;
  alt: string;
  width?: number;
  height?: number;
  aspectRatio?: string;     // e.g., "16/9", "1/1"
  lazy?: boolean;           // default: true
  quality?: 'low' | 'medium' | 'high'; // default: high
  srcSet?: string;          // Responsive sources
  sizes?: string;           // Responsive sizes
  className?: string;
}
```

**Usage Example:**
```jsx
<Image
  src="/demon.png"
  alt="Demon Boss"
  width={400}
  height={400}
  aspectRatio="1/1"
  lazy
/>
```

**CSS Classes:** `.image`, `.image--lazy`

---

## 25. Video Component

**Component Name:** `VideoPlayer`

**Purpose:** Embed and play videos.

**Props:**
```typescript
interface VideoPlayerProps {
  src: string;
  poster?: string;          // Thumbnail image
  width?: string | number;
  height?: string | number;
  controls?: boolean;       // default: true
  autoplay?: boolean;
  loop?: boolean;
  muted?: boolean;
}
```

**Usage Example:**
```jsx
<VideoPlayer
  src="/demon-gameplay.mp4"
  poster="/demon-thumbnail.png"
  width="100%"
  controls
/>
```

**CSS Classes:** `.video-player`

---

## 26. Divider / Separator

**Component Name:** `Divider`

**Purpose:** Visual separation between sections.

**Visual Design:**
- Line: 1px solid #ddd
- Margin: 24px 0 (or custom)
- Optional text in center

**Props:**
```typescript
interface DividerProps {
  text?: string;
  spacing?: 'compact' | 'normal' | 'loose'; // default: normal
  className?: string;
}
```

**Usage Example:**
```jsx
<Divider />
<Divider text="or" />
<Divider spacing="loose" />
```

**CSS Classes:** `.divider`, `.divider--text`

---

## 27. Loading Spinner

**Component Name:** `LoadingSpinner`

**Purpose:** Indicate loading state.

**Visual Design:**
- Circular spinner animation
- Brand color (#003399)
- Multiple sizes (small, medium, large)
- Optional text below

**Props:**
```typescript
interface LoadingSpinnerProps {
  size?: 'small' | 'medium' | 'large'; // default: medium
  text?: string;
  fullPage?: boolean;       // Center on page with overlay
  color?: string;
}
```

**Usage Example:**
```jsx
<LoadingSpinner size="medium" text="Loading creatures..." />
```

**CSS Classes:** `.spinner`, `.spinner--large`, `.spinner--fullpage`

---

## 28. Empty State

**Component Name:** `EmptyState`

**Purpose:** Display when no content/results available.

**Visual Design:**
```
╔════════════════════════╗
║        🔍             ║
║  No Results Found      ║
║  Try adjusting filters ║
║  or search terms       ║
║  [Clear Filters] [Go Home]
╚════════════════════════╝
```

- Icon: Large (48-64px)
- Title: Bold, 18-20px
- Description: Gray text
- Actions: Optional buttons

**Props:**
```typescript
interface EmptyStateProps {
  icon?: ReactNode;
  title: string;
  description?: string;
  actions?: { label: string; onClick: () => void }[];
  className?: string;
}
```

**Usage Example:**
```jsx
<EmptyState
  title="No Items Found"
  description="Try adjusting your search or filters"
  actions={[
    { label: 'Clear Filters', onClick: handleClearFilters },
    { label: 'Browse All', onClick: handleBrowseAll }
  ]}
/>
```

**CSS Classes:** `.empty-state`, `.empty-state__icon`, `.empty-state__actions`

---

# COMPONENT USAGE MATRIX

| Page Type | Components Used |
|-----------|-----------------|
| **Creature Detail** | PageLayout, Infobox, Breadcrumb, SidebarNav, TableOfContents, Section, Tabs, StatsTable, LootTable, EntityCard (related), Gallery, Badge, Button, Divider |
| **Item Detail** | PageLayout, Infobox, Breadcrumb, SidebarNav, TableOfContents, Section, Tabs, StatsTable, ComparisonTable, EntityCard (related), Accordion, Gallery, Badge, Button |
| **NPC Detail** | PageLayout, Breadcrumb, SidebarNav, TableOfContents, Section, StatsTable, Tabs, Accordion, LootTable (trades), EntityCard (related), Image, Gallery, Button |
| **Listing/Browse** | PageLayout, SidebarNav, FilterSidebar, SearchBox, EntityCard (grid), Pagination, EmptyState, Button |
| **Search Results** | PageLayout, SearchBox, EntityCard (list), Pagination, EmptyState, Badge (type indicator) |
| **Home Page** | HeroBanner, Card (featured items), EntityCard (popular creatures), Section |

---

# STYLING GUIDELINES

## Color Palette

```css
/* Brand Colors */
--primary: #003399;        /* Links, highlights */
--primary-dark: #002a70;   /* Hover states */
--secondary: #0645ad;      /* Alternative links */

/* Neutral Colors */
--text-primary: #222222;
--text-secondary: #666666;
--text-tertiary: #999999;
--background: #FFFFFF;
--background-light: #F6F6F6;
--border: #CCCCCC;
--border-light: #DDDDDD;

/* State Colors */
--success: #00AA00;
--warning: #FF6600;
--danger: #CC0000;
--info: #0645AD;
```

## Typography

```css
/* Font Stack */
--font-serif: Georgia, serif;
--font-sans: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
--font-mono: "Courier New", monospace;

/* Font Sizes */
--font-sm: 12px;
--font-base: 14px;
--font-md: 16px;
--font-lg: 18px;
--font-xl: 20px;
--font-2xl: 24px;
--font-3xl: 28px;
--font-4xl: 32px;

/* Font Weights */
--font-regular: 400;
--font-medium: 600;
--font-bold: 700;
```

## Spacing Scale

```css
--spacing-xs: 4px;
--spacing-sm: 8px;
--spacing-md: 12px;
--spacing-lg: 16px;
--spacing-xl: 20px;
--spacing-2xl: 24px;
--spacing-3xl: 32px;
--spacing-4xl: 40px;
```

---

# IMPLEMENTATION NOTES FOR DEVELOPERS

1. **Component Library**: Consider using a CSS-in-JS solution (Styled Components, Emotion) or SCSS modules for scoped styling

2. **TypeScript**: All components should have full TypeScript definitions as shown in props

3. **Accessibility**: 
   - Use semantic HTML (`<button>`, `<nav>`, `<article>`)
   - Include ARIA roles and labels
   - Ensure keyboard navigation
   - Test with screen readers

4. **Performance**:
   - Lazy load images
   - Code split components
   - Memoize expensive components
   - Optimize re-renders

5. **Testing**:
   - Unit tests for component logic
   - Visual regression tests for styling
   - Accessibility tests (axe-core)
   - E2E tests for user flows

6. **Documentation**:
   - Storybook for component documentation
   - Visual examples with code snippets
   - Props documentation
   - Accessibility notes

---

This component specification provides a complete design system for the Nightmares Wiki. All components are designed to work together seamlessly while remaining flexible enough to accommodate future features and variations.
