# Nightmares Wiki - UX Design Package Summary

## Overview

This design package contains a complete UX specification for the Nightmares Wiki project. It includes detailed analysis of proven wiki design patterns, wireframes for key pages, and comprehensive component specifications.

---

## 📋 Documents Included

### 1. **FANDOM_DESIGN_ANALYSIS.md**
A comprehensive analysis of Tibia.fandom.com design patterns and industry-standard wiki layouts.

**Contains:**
- Overall page layout structure (header, sidebar, main content, footer)
- Navigation patterns (breadcrumbs, sidebars, table of contents)
- Information hierarchy and visual organization
- Component patterns (infoboxes, stat tables, tabs, cards, badges)
- Typography and color schemes
- Responsive design breakpoints (desktop, tablet, mobile)
- Search and filtering UI patterns
- Entity detail presentation strategies
- Listing/browse page organization
- Styling guidelines and color palette

**Key Findings:**
- Three-column layout (18% sidebar | 60-65% content | 17-22% right sidebar)
- Sticky navigation for better UX
- Auto-generated table of contents for long pages
- Infobox pattern for quick entity data
- Grid-based card layouts for listings
- Color scheme: Blue (#003399) primary, grays for backgrounds
- Responsive breakpoints: 1200px (desktop), 992px (tablet), 768px (mobile)

---

### 2. **WIREFRAMES_DETAIL_PAGES.md**
Detailed wireframes for the three main entity detail page types with responsive variations.

**Wireframes Included:**

#### A. **Creature Detail Page** (Boss/Enemy)
**Desktop Layout:**
- Hero image (infobox right-aligned, 280-320px)
- Title with metadata badges
- Overview section with lore/description
- Statistics table (HP, Experience, Armor, Damage types)
- Abilities/Behavior section with tabs (Combat/Movement)
- Loot table with rarity and drop percentages
- Habitat section (location, level requirement, group size)
- Related creatures in card grid
- Gallery with screenshots

**Responsive Adaptations:**
- Tablet: Infobox becomes full-width at top
- Mobile: Single column, expanded sections, smaller images

#### B. **Item Detail Page** (Weapons/Equipment/Consumables)
**Desktop Layout:**
- Item image (infobox right-aligned)
- Title with rarity badge and item type
- Overview description
- Item statistics table (Attack Power, Magic Power, Bonuses, Weight, Requirements)
- Special abilities with tabs (Passive/Active)
- Value & acquisition section (price, drop rates, vendors, quests, crafting)
- Enhancement/Crafting information
- "Used By" section showing which classes/professions use it
- Related items in card grid
- Gallery/screenshots

**Responsive Adaptations:**
- Tablet: Sections collapse into accordion
- Mobile: Stats table becomes expandable, cards stack

#### C. **NPC Detail Page** (Merchants/Quest Givers/Trainers)
**Desktop Layout:**
- NPC portrait (infobox format)
- Profile & background section
- Location & access information (coordinates, availability)
- Dialogue/greetings section with tabs
- Trading & commerce section (shop inventory table)
- Quests section (available, in-progress, completed)
- Related NPCs, locations, and quests in cards
- Reputation tracking and interaction history

**Responsive Adaptations:**
- Tablet: Shop inventory becomes scrollable
- Mobile: Tabs convert to accordion, portrait full-width

**Key Annotation Notes:**
- Information architecture flowing from general to specific
- Mobile-optimized with expandable sections
- Visual hierarchy maintained across all breakpoints
- Consistent use of infobox pattern across all entity types

---

### 3. **COMPONENT_SPECIFICATIONS.md**
Complete specification for all 28 reusable UI components with props, styling, and usage examples.

**Component Categories:**

**Layout Components (3)**
- PageLayout: Main three-column structure
- Card: Flexible content container
- Section: Semantic section wrapper with spacing

**Navigation Components (4)**
- Breadcrumb: Hierarchical navigation path
- SidebarNav: Expandable hierarchical menu
- TableOfContents: Auto-generated page sections
- SearchBox: Global search with autocomplete

**Entity Display Components (2)**
- Infobox: Floating information box for entity key data
- EntityCard: Small card for grid/list displays

**Data Presentation Components (3)**
- StatsTable: Structured key-value data tables
- LootTable: Drop table with rarity and percentages
- ComparisonTable: Side-by-side entity comparison

**Form & Input Components (4)**
- SearchBox: With autocomplete (detailed separate entry)
- FilterSidebar: Multi-field filtering interface
- Button: Primary/Secondary/Tertiary variants
- Badge: Category/status labels

**Interaction Components (5)**
- Tabs: Underline/pill/boxed variants
- Accordion: Collapsible sections
- Modal: Overlay dialog
- Tooltip: Hover information
- Divider: Visual section separator

**Content Components (2)**
- RichTextContent: Markdown/HTML support
- Gallery: Image grid with lightbox

**Media Components (4)**
- Image: Optimized responsive images
- VideoPlayer: Embedded video with controls
- HeroBanner: Large decorative header image
- RelatedItems: Links to related entities

**Feedback Components (2)**
- LoadingSpinner: Loading indicator
- EmptyState: No results/content state

**Component Usage Matrix:**
- Creature Detail: 13 components
- Item Detail: 12 components
- NPC Detail: 11 components
- Listing/Browse: 8 components
- Search Results: 6 components
- Home Page: 5 components

---

## 🎨 Design System Highlights

### Color Palette
- **Primary Brand Color**: #003399 (Deep Blue)
- **Primary Hover**: #002a70 (Darker Blue)
- **Secondary Links**: #0645ad (Medium Blue)
- **Visited Links**: #663399 (Purple)
- **Text Primary**: #222222 (Dark Gray)
- **Text Secondary**: #666666 (Medium Gray)
- **Backgrounds**: #FFFFFF (White) / #F6F6F6 (Light Gray)
- **Borders**: #CCCCCC (Primary) / #DDDDDD (Light)
- **Semantic Colors**: Green (#00AA00), Orange (#FF6600), Red (#CC0000)

### Typography
- **Headers**: Georgia serif or clean sans-serif (600-700 weight)
- **Body**: System font stack (-apple-system, "Segoe UI", Roboto)
- **Monospace**: "Courier New" for code
- **Font Sizes**: Scaled from 12px (small) to 32px (largest headers)
- **Line Height**: 1.6 for body text (readability)

### Spacing Scale
8px-based system: 4px, 8px, 12px, 16px, 20px, 24px, 32px, 40px

### Responsive Breakpoints
- **Desktop**: 1200px+ (full 3-column layout)
- **Tablet**: 768-1199px (2-column, hamburger menu)
- **Mobile**: <768px (1-column, stacked layout)

---

## 🎯 Key Design Principles Applied

1. **Information Hierarchy**
   - Most important info (infobox) prominent and quickly accessible
   - Secondary info in natural reading order
   - Related content encourages exploration

2. **Progressive Disclosure**
   - Expandable sections on mobile reduce scrolling
   - Tabs organize related information
   - Accordions for optional/detailed info

3. **Consistency**
   - All entity types follow the same layout pattern
   - Reusable components maintain visual consistency
   - Color coding for rarity, difficulty, status

4. **Accessibility**
   - WCAG 2.1 AA compliance targeted
   - Semantic HTML with proper ARIA labels
   - Keyboard navigation support
   - Sufficient color contrast (4.5:1 minimum)

5. **Performance**
   - Lazy loading for images
   - Optimized component sizes
   - Responsive images with srcSet

6. **Mobile-First**
   - Starts with mobile layout then enhances
   - Touch-friendly interactive elements (44px minimum)
   - Full-width utilization on small screens

---

## 📐 Layout Standards

### Page Structure Template

```
┌─────────────────────────────────────────────┐
│           HEADER / GLOBAL NAV               │
├──────────┬──────────────────────┬───────────┤
│ SIDEBAR  │  BREADCRUMB          │   TOC     │
│ (250px)  │  TITLE               │ (280px)   │
│          │  INFOBOX (float:right)│           │
│          │                      │           │
│          │  MAIN CONTENT        │           │
│          │  (Sections)          │           │
│          │  - Overview          │           │
│          │  - Stats             │           │
│          │  - Details           │           │
│          │  - Related Cards     │           │
│          │  - Gallery           │           │
│          │                      │           │
├──────────┴──────────────────────┴───────────┤
│              FOOTER                        │
└─────────────────────────────────────────────┘
```

### Infobox Pattern (Right-Aligned)
```
┌──────────────────┐
│  [Image 300x300] │
├──────────────────┤
│  Title           │
├──────────────────┤
│ Key: Value       │
│ Key: Value       │
│ Key: Value       │
├──────────────────┤
│ • List Item 1    │
│ • List Item 2    │
├──────────────────┤
│ [Action Button]  │
└──────────────────┘
```

---

## 🔄 Responsive Behavior Examples

### Desktop (1200px+)
- Three-column layout visible
- Sidebars sticky on scroll
- Infobox floats right within content
- Table of Contents shows full structure
- All tables fully expanded
- Images at full resolution

### Tablet (768-1199px)
- Two-column layout (sidebar + content)
- Right sidebar integrates into main column
- Infobox moves to top of content (full-width)
- Hamburger menu available for sidebar
- Tables may become scrollable
- Reduced font sizes by 1-2px

### Mobile (<768px)
- Single column layout
- Hamburger menu for navigation
- Infobox full-width at top
- Expandable sections reduce vertical scroll
- Tables convert to cards or stacked rows
- Images scale to 100% width
- Buttons full-width or stacked

---

## 🛠️ Implementation Recommendations

### Frontend Technology Stack
- **Framework**: React with TypeScript
- **Styling**: SCSS Modules or CSS-in-JS (Styled Components/Emotion)
- **Component Library**: Storybook for documentation
- **State Management**: Context API or Redux
- **Testing**: Jest + React Testing Library

### Component Development Approach
1. **Atomic Design**: Atoms (Button, Badge) → Molecules (Card, Infobox) → Organisms (PageLayout) → Templates → Pages
2. **Story-Driven**: Implement components based on usage in wireframes
3. **TDD**: Write tests before implementing components
4. **Accessibility First**: Include ARIA labels and keyboard navigation from start

### Design System Tools
- **Storybook**: Showcase all components with variations
- **Chromatic**: Visual regression testing
- **axe DevTools**: Automated accessibility testing
- **Figma**: Design tokens and component definitions

---

## 📊 Design Workflow for Developers

1. **Review Wireframes**: Understand layout and component placement
2. **Check Component Specs**: Find component props and variations
3. **Implement Base Styles**: Use color palette and typography standards
4. **Build Layout Components**: PageLayout, Card, Section
5. **Build Navigation**: Breadcrumb, SidebarNav, TableOfContents
6. **Build Entity Components**: Infobox, EntityCard, RelatedItems
7. **Build Data Tables**: StatsTable, LootTable, ComparisonTable
8. **Add Interactivity**: Tabs, Accordion, Modal, Tooltip
9. **Implement Responsive**: Test at all breakpoints
10. **Accessibility Review**: WCAG compliance testing

---

## 🎓 Learning Resources from Analysis

### What Works Well (Fandom Patterns to Keep)
1. **Sticky sidebar navigation** - Excellent for wiki browsing
2. **Auto-generated table of contents** - Helps users navigate long content
3. **Infobox pattern** - Efficiently displays key entity data
4. **Related content cards** - Encourages wiki exploration
5. **Breadcrumb navigation** - Clear context at all times
6. **Category/tag system** - Enables content organization
7. **Search with filtering** - Essential for large datasets
8. **Responsive grid layouts** - Works on all screen sizes

### What to Improve (Nightmares Wiki Advantages)
1. **Cleaner aesthetic** - Reduce ad clutter vs. Fandom
2. **Better typography** - Modern font choices and sizing
3. **Improved visual hierarchy** - Clearer section distinction
4. **Faster load times** - Optimize assets and lazy loading
5. **Optional dark mode** - Support user preferences
6. **Accessible design** - WCAG AA from the start
7. **Custom branding** - Unique visual identity for Nightmares world

---

## 📚 Document Cross-References

**For Developers Building Components:**
- Start with COMPONENT_SPECIFICATIONS.md
- Reference WIREFRAMES_DETAIL_PAGES.md for placement examples
- Consult FANDOM_DESIGN_ANALYSIS.md for styling standards

**For Designers Creating Mockups:**
- Use WIREFRAMES_DETAIL_PAGES.md as foundation
- Apply colors from FANDOM_DESIGN_ANALYSIS.md
- Reference component examples in COMPONENT_SPECIFICATIONS.md

**For Product Managers/Stakeholders:**
- Review FANDOM_DESIGN_ANALYSIS.md for strategic direction
- Check WIREFRAMES_DETAIL_PAGES.md for visual preview
- Reference COMPONENT_SPECIFICATIONS.md for feature completeness

---

## ✅ Design Quality Checklist

### Visual Design
- [ ] Color palette applied consistently
- [ ] Typography follows standards (sizes, weights, spacing)
- [ ] Spacing follows 8px grid system
- [ ] Components match specifications exactly
- [ ] Visual hierarchy is clear (size, color, position)

### Responsive Design
- [ ] Works at 1200px+ (desktop)
- [ ] Works at 768-1199px (tablet)
- [ ] Works at <768px (mobile)
- [ ] All text readable at all sizes
- [ ] Touch targets minimum 44px on mobile

### Accessibility
- [ ] Color contrast 4.5:1 minimum
- [ ] All images have alt text
- [ ] Form labels associated with inputs
- [ ] Keyboard navigable (Tab, Arrow keys, Enter, Escape)
- [ ] Screen reader compatible (ARIA labels)
- [ ] Focus states visible

### User Experience
- [ ] Information hierarchy intuitive
- [ ] Navigation clear and consistent
- [ ] Related content discoverable
- [ ] Empty states handled gracefully
- [ ] Loading states communicated
- [ ] Error messages helpful

---

## 🚀 Next Steps

1. **Review & Feedback**: Stakeholder review of wireframes and components
2. **Design Tokens**: Convert specifications to design system (Figma, Storybook)
3. **Component Library**: Build React components matching specifications
4. **Prototype**: Create interactive prototype with key flows
5. **User Testing**: Validate design with actual users
6. **Refinement**: Iterate based on feedback
7. **Implementation**: Develop full wiki with all pages
8. **Testing**: QA across browsers and devices
9. **Launch**: Roll out with documentation and training

---

## 📞 Design Documentation Contact

For questions about:
- **Wireframes & Layout**: See WIREFRAMES_DETAIL_PAGES.md
- **Components & Props**: See COMPONENT_SPECIFICATIONS.md
- **Visual Design & Styling**: See FANDOM_DESIGN_ANALYSIS.md

---

## 📝 Version History

- **v1.0** - Initial design package
  - Fandom analysis complete
  - Wireframes for 3 main page types
  - 28 components fully specified
  - Color palette and typography defined
  - Responsive breakpoints documented

---

**Nightmares Wiki Design Package Complete** ✨

This comprehensive design specification provides everything needed to build a professional, user-friendly wiki experience that matches Fandom.com quality while establishing a unique identity for the Nightmares game world.
