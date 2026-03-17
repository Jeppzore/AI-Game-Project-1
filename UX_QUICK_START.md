# Nightmares Wiki - Quick Start Guide for Developers

## TL;DR - What You Need to Know

The Nightmares Wiki is building a professional game wiki matching Fandom.com quality. You've been given:

### 📄 Three Core Design Documents

1. **FANDOM_DESIGN_ANALYSIS.md** (18,862 chars)
   - Analysis of proven wiki design patterns
   - Layout structure, navigation, typography, colors
   - Responsive breakpoints (1200px, 768px)
   - What works, what to improve

2. **WIREFRAMES_DETAIL_PAGES.md** (50,126 chars)
   - Visual wireframes for 3 main page types:
     - Creature Detail Page (Boss/Enemy)
     - Item Detail Page
     - NPC Detail Page
   - Desktop, tablet, and mobile layouts
   - ASCII diagrams + annotations
   - Component placement and responsive behavior

3. **COMPONENT_SPECIFICATIONS.md** (38,846 chars)
   - 28 reusable UI components fully specified
   - Props, styling, usage examples
   - Accessibility notes
   - Component usage matrix showing which components go on which pages

---

## 🎨 Design System at a Glance

### Colors
```
Primary Brand: #003399 (Deep Blue)
Text: #222222 (Dark Gray)
Borders: #CCCCCC (Light Gray)
Background: #FFFFFF (White) / #F6F6F6 (Light Gray)
Success: #00AA00 | Warning: #FF6600 | Danger: #CC0000
```

### Typography
- Headers: Georgia serif or system sans-serif, bold
- Body: System fonts (-apple-system, "Segoe UI", Roboto)
- Base size: 14px body, 12px small, up to 32px for titles

### Spacing
- 8px grid system: 4, 8, 12, 16, 20, 24, 32, 40px

### Responsive Breakpoints
- **Desktop (1200px+)**: Full 3-column layout
- **Tablet (768-1199px)**: 2-column, hamburger menu
- **Mobile (<768px)**: 1-column, stacked layout

---

## 🏗️ Page Layout Template

All detail pages follow this structure:

```
[HEADER / GLOBAL NAV]
[BREADCRUMB]
[3-COLUMN LAYOUT]
  | Sidebar      | Main Content      | Table of Contents |
  | (250px)      | (60-65%)          | (280px)          |
  | • Navigation | • Infobox         | • Auto-generated |
  | • Categories | • Sections        | • TOC            |
  |              | • Tables          | • Sticky on      |
  |              | • Cards           | scroll           |
[FOOTER]
```

### Responsive Changes
- **Tablet**: Sidebar becomes hamburger, TOC integrates into main column
- **Mobile**: Single column, hero image full-width, sections expandable

---

## 🧩 Essential Components (28 Total)

### Must-Have Components
```
Layout:        PageLayout, Card, Section
Navigation:    Breadcrumb, SidebarNav, TableOfContents, SearchBox
Display:       Infobox, EntityCard, StatsTable, LootTable
Interaction:   Button, Badge, Tabs, Accordion, Modal
Content:       Gallery, RichTextContent, Divider
Feedback:      LoadingSpinner, EmptyState, Tooltip
```

### Component Usage by Page

**Creature Detail**: Infobox + StatsTable + Tabs + LootTable + EntityCards (related) + Gallery + Badge
**Item Detail**: Infobox + StatsTable + Tabs + ComparisonTable + EntityCards (related) + Accordion + Gallery
**NPC Detail**: Infobox + StatsTable + LootTable (trades) + EntityCards (related) + Gallery + Accordion

---

## 📱 Responsive Design Priorities

### Desktop First
- Full 3-column layout with sticky sidebars
- All tables fully expanded
- Infobox floats right
- Hover effects and advanced interactions

### Tablet Adaptations
- Right sidebar collapses into main column
- Infobox moves to top (full-width)
- Tables become scrollable or simplified
- Hamburger menu for left sidebar

### Mobile Optimization
- Single column layout
- Expandable/accordion sections to reduce scrolling
- Full-width images and content
- Touch-friendly buttons (44px minimum)
- Simplified tables (card layout or horizontal scroll)

---

## 🎯 Component Implementation Checklist

### For Each Component You Build:
```
□ Props definition (TypeScript interface)
□ Default prop values
□ Styling (colors, spacing, fonts from standards)
□ Responsive behavior (mobile, tablet, desktop)
□ Hover/active/focus states
□ Accessibility (ARIA labels, keyboard nav)
□ Dark mode support (if applicable)
□ Loading state (if applicable)
□ Empty state (if applicable)
□ Error state (if applicable)
□ Storybook documentation
□ Unit tests
□ Visual regression tests
```

---

## 🔍 Common Component Patterns

### Infobox (Right-Floated Box)
```
Props:
- image: { src, alt, caption? }
- title: string
- sections: Array of { items: [{ label, value }] }
- actions?: ReactNode[]

Desktop: Float right, 320px width
Tablet: Full-width at top
Mobile: Full-width, stacked at top
```

### Entity Card (Grid Display)
```
Props:
- id, title, type, image
- description?, badges?, stats?
- href, onClick

Grid: 4 cols desktop, 3 cols tablet, 2 cols mobile
Hover: Slight lift with shadow increase
```

### Stats Table (Key-Value Display)
```
Props:
- rows: { label, value, unit?, comparison? }[]
- striped, bordered, hoverable

Header: Bold, gray background
Rows: Alternate white/light-gray
```

### Tabs (Section Navigation)
```
Props:
- tabs: { id, label, icon?, disabled? }[]
- activeTab, onTabChange

Active: Underline + bold
Mobile: Consider converting to Accordion

ARIA: tablist, tab, tabpanel roles
```

---

## 📊 Data Structure Examples

### Creature Entity
```typescript
interface Creature {
  id: string;
  name: string;
  image: string;
  type: 'boss' | 'mob' | 'summon';
  difficulty: 1-5;
  description: string;
  stats: {
    hp: number;
    experience: number;
    armor: number;
    meleeDamage: [min, max];
    magicResist: number;
    // ... more stats
  };
  abilities: Array<{
    name: string;
    type: 'melee' | 'ranged' | 'magic' | 'special';
    description: string;
    cooldown?: number;
  }>;
  loot: Array<{
    itemId: string;
    name: string;
    rarity: 'common' | 'uncommon' | 'rare' | 'epic' | 'legendary';
    dropChance: number;
    minQuantity?: number;
    maxQuantity?: number;
  }>;
  location: {
    name: string;
    coordinates: { x: number; y: number };
    levelRequirement: number;
  };
}
```

### Item Entity
```typescript
interface Item {
  id: string;
  name: string;
  image: string;
  type: 'weapon' | 'armor' | 'accessory' | 'consumable' | 'crafting';
  rarity: 'common' | 'uncommon' | 'rare' | 'epic' | 'legendary';
  itemLevel: number;
  description: string;
  stats: {
    attackPower?: number;
    magicPower?: number;
    defensePower?: number;
    bonuses?: { [stat: string]: number };
  };
  acquisition: {
    purchasePrice?: number;
    sellValue?: number;
    dropFrom?: Array<{ creatureId: string; chance: number }>;
    vendors?: string[];
    quests?: string[];
    recipe?: { materials: Array<{ itemId: string; quantity: number }> };
  };
  usedBy?: Array<{ classId: string; usageRate: number }>;
}
```

---

## 🛠️ Development Setup

### Project Structure
```
/Client (React/TypeScript)
  /src
    /components        # Reusable components
      /Layout         # Layout components
      /Navigation     # Nav components
      /Entity         # Entity display components
      /Tables         # Data table components
      /Forms          # Form components
      /Common         # Shared/utility components
    /pages            # Page components
    /services         # API services
    /styles           # Global styles, SCSS
    /types            # TypeScript interfaces
    /utils            # Helper functions
    /hooks            # Custom React hooks
  /public             # Static assets
  tsconfig.json       # Strict mode enabled
  .storybook         # Storybook configuration

/Server (.NET)
  /Models            # Entity models (match data structures)
  /Controllers       # API endpoints
  /Services          # Business logic
  /Data              # MongoDB context
```

### Key Files to Create/Update
1. `tsconfig.json` - Enable strict mode
2. `src/theme.ts` - Design tokens (colors, spacing, typography)
3. `src/components/` - All 28 components from specs
4. `.storybook/` - Component documentation

---

## ✅ Code Quality Standards

### TypeScript
- No `any` types - use explicit interfaces
- Strict mode enabled
- Props always typed (interfaces)
- Return types on functions

### Component Props
```typescript
// ✅ Good
interface ButtonProps {
  children: ReactNode;
  variant?: 'primary' | 'secondary' | 'danger';
  size?: 'small' | 'medium' | 'large';
  disabled?: boolean;
  onClick?: () => void;
}

// ❌ Avoid
interface ButtonProps {
  [key: string]: any;
}
```

### Accessibility
- Semantic HTML (`<button>`, `<nav>`, `<article>`)
- ARIA labels on non-semantic elements
- Keyboard navigation (Tab, Arrow, Enter, Escape)
- Focus indicators visible
- 4.5:1 color contrast minimum

### Testing
- Unit tests for component logic
- Visual regression tests for styling
- Accessibility tests (axe-core)
- E2E tests for user flows

---

## 🚀 Implementation Order (Suggested)

1. **Phase 1 - Foundation** (Week 1)
   - [ ] Design tokens/theme configuration
   - [ ] Layout components (PageLayout, Card, Section)
   - [ ] Navigation components (Breadcrumb, SidebarNav)

2. **Phase 2 - Core Components** (Week 2-3)
   - [ ] Entity components (Infobox, EntityCard)
   - [ ] Data tables (StatsTable, LootTable)
   - [ ] Interaction components (Button, Badge, Tabs)

3. **Phase 3 - Pages** (Week 4-5)
   - [ ] Creature detail page
   - [ ] Item detail page
   - [ ] NPC detail page
   - [ ] Listing/browse pages

4. **Phase 4 - Polish** (Week 6)
   - [ ] Responsive testing (all breakpoints)
   - [ ] Accessibility audit
   - [ ] Performance optimization
   - [ ] Dark mode (if planned)

---

## 📚 Quick Reference Links

### Within Repository
- **Full Wireframes**: `WIREFRAMES_DETAIL_PAGES.md`
- **Component Specs**: `COMPONENT_SPECIFICATIONS.md`
- **Design Analysis**: `FANDOM_DESIGN_ANALYSIS.md`
- **This Guide**: `UX_DESIGN_PACKAGE_SUMMARY.md`

### External Resources
- [Fandom.com](https://fandom.com) - Reference for patterns
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/) - Accessibility standards
- [Web Content Accessibility Guidelines](https://www.w3.org/WAI/WCAG21/Understanding/) - Detailed explanations

---

## 💡 Common Questions

**Q: Should I follow the wireframes exactly?**
A: Wireframes are guidelines. Use them as a starting point, but let implementation and testing guide refinements. The component specs are more rigid.

**Q: What about dark mode?**
A: Not specified in design. Can be added later by creating CSS variable overrides. Use CSS custom properties for colors to enable this.

**Q: How do I handle real data vs. placeholder?**
A: Components should accept data as props. Use Storybook to show both real and placeholder examples.

**Q: What about animation/transitions?**
A: Keep it subtle. Use CSS transitions for hover states, smooth scroll for tab switches. Don't distract from content.

**Q: How granular should components be?**
A: Follow Atomic Design: Atoms (basic) → Molecules (simple groups) → Organisms (complex) → Templates → Pages.

---

## 🤝 Design System Maintenance

### As You Build:
1. Keep component specs updated if you make changes
2. Document non-standard decisions
3. Create Storybook stories for each variant
4. Maintain design token consistency
5. Test responsiveness at all breakpoints

### Before Launch:
1. Accessibility audit (axe-core + manual testing)
2. Cross-browser testing (Chrome, Firefox, Safari, Edge)
3. Mobile device testing (real devices preferred)
4. Performance profiling
5. Design review with stakeholders

---

**Ready to Build? Start with Component Specifications → Review Wireframes → Implement Components → Test Responsively**

Good luck! 🚀
