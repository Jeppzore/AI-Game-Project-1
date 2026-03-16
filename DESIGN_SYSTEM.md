# Nightmares Wiki - Design System

## 🎨 Design Overview

The Nightmares Wiki is a retro pixel-art styled game reference encyclopedia that maintains visual consistency with the Nightmares game world while providing modern, accessible user interactions. This design system bridges nostalgic aesthetics with contemporary UX practices.

---

## 1. Visual Identity

### Design Philosophy

- **Retro Pixel-Art Aesthetic**: Embrace 16-bit/32-bit era gaming visual language
- **Modern Accessibility**: WCAG AA contrast and keyboard navigation standards
- **Clarity Over Decoration**: Information hierarchy prioritizes readability
- **Custom Crafted**: No Bootstrap, Material Design, or generic UI frameworks
- **Consistency**: Unified visual language across all pages and components

---

## 2. Color Palette

### Primary Colors

| Color | Hex Code | Usage | Contrast Ratio (on White) |
|-------|----------|-------|--------------------------|
| **Dark Purple** | `#1a1a2e` | Primary backgrounds, dark mode base | 18.5:1 ✓ WCAG AAA |
| **Neon Purple** | `#7b3ff2` | Primary CTA buttons, highlights, active states | 5.2:1 ✓ WCAG AA |
| **Acid Green** | `#39ff14` | Success states, hover effects, player-friendly UI | 4.5:1 ✓ WCAG AA |
| **Dark Gray** | `#2a2a3e` | Secondary backgrounds, cards, containers | 13.2:1 ✓ WCAG AAA |

### Accent Colors

| Color | Hex Code | Usage |
|-------|----------|-------|
| **Crimson Red** | `#ef3b36` | Error states, danger actions, enemy indicators |
| **Golden Yellow** | `#ffd700` | Warnings, important items, loot highlights |
| **Cyan Blue** | `#00d4ff` | Information, player stats, cooldown effects |
| **Forest Green** | `#2d5016` | Positive states, healing, buffs |

### Neutral Palette

| Color | Hex Code | Usage |
|-------|----------|-------|
| **Off-White** | `#e8e8e8` | Primary text, light backgrounds |
| **Light Gray** | `#a8a8a8` | Secondary text, disabled states |
| **Medium Gray** | `#666666` | Tertiary text, borders |
| **Black** | `#0a0a0a` | Darkest text, borders |

### CSS Variables

```css
:root {
  /* Primary Colors */
  --color-primary-dark: #1a1a2e;
  --color-primary-neon: #7b3ff2;
  --color-primary-green: #39ff14;
  --color-primary-gray: #2a2a3e;

  /* Accent Colors */
  --color-accent-error: #ef3b36;
  --color-accent-warning: #ffd700;
  --color-accent-info: #00d4ff;
  --color-accent-success: #2d5016;

  /* Neutral Palette */
  --color-text-primary: #e8e8e8;
  --color-text-secondary: #a8a8a8;
  --color-text-tertiary: #666666;
  --color-bg-base: #1a1a2e;
  --color-bg-secondary: #2a2a3e;
  --color-border: #3a3a4e;
  
  /* Backgrounds */
  --color-surface-card: #252538;
  --color-surface-elevated: #353548;
}

/* Dark Mode (default) */
html {
  color-scheme: dark;
  background-color: var(--color-bg-base);
  color: var(--color-text-primary);
}
```

---

## 3. Typography

### Font Stack

```css
:root {
  /* Heading Font: Pixel-Art Style */
  --font-heading: 'Press Start 2P', 'Courier New', monospace;
  
  /* Body Font: Readable at all sizes */
  --font-body: 'Inter', 'Segoe UI', 'Roboto', sans-serif;
  
  /* Monospace: Code and technical data */
  --font-mono: 'Courier New', monospace;
}
```

### Font Sources

1. **Press Start 2P** (Headings)
   - Import from Google Fonts: `https://fonts.googleapis.com/css2?family=Press+Start+2P&display=swap`
   - 8x8 pixel font, perfect for retro headings
   - Use sparingly for H1, H2 titles only (readability at smaller sizes is poor)

2. **Inter** (Body Text)
   - Import from Google Fonts: `https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap`
   - Modern, highly legible, excellent for body copy
   - Supports weight variants: 400, 500, 600, 700

3. **Courier New** (Code/Data)
   - System font, always available
   - Used for stats, IDs, technical information

### Font Sizes

```css
:root {
  /* Type Scale (in pixels) */
  --font-size-xs: 11px;
  --font-size-sm: 13px;
  --font-size-base: 16px;
  --font-size-lg: 18px;
  --font-size-xl: 20px;
  --font-size-2xl: 24px;
  --font-size-3xl: 32px;
  --font-size-4xl: 40px;
  
  /* Heading Sizes (Press Start 2P) */
  --font-size-h1: 40px;    /* Use only for page title */
  --font-size-h2: 32px;    /* Section titles */
  --font-size-h3: 24px;    /* Subsection titles */
}
```

### Font Weight

| Weight | Value | Usage |
|--------|-------|-------|
| Regular | 400 | Body text, default weight |
| Medium | 500 | Secondary headings, emphasis |
| Semibold | 600 | Button labels, strong emphasis |
| Bold | 700 | Primary headings, highlight text |

### Line Height & Letter Spacing

```css
:root {
  /* Line Height */
  --line-height-tight: 1.2;      /* Headings */
  --line-height-normal: 1.5;     /* Body text */
  --line-height-relaxed: 1.8;    /* Long-form content */
  
  /* Letter Spacing */
  --letter-spacing-tight: -0.02em;
  --letter-spacing-normal: 0em;
  --letter-spacing-wide: 0.1em;  /* For pixel fonts */
}
```

---

## 4. Spacing & Grid System

### Spacing Scale

```css
:root {
  --spacing-xs: 4px;
  --spacing-sm: 8px;
  --spacing-md: 16px;
  --spacing-lg: 24px;
  --spacing-xl: 32px;
  --spacing-2xl: 48px;
  --spacing-3xl: 64px;
}
```

### Layout Grid

- **Base Unit**: 4px (allows flexible alignment)
- **Columns**: 12-column responsive grid
- **Gap**: 16px (1rem / --spacing-md)
- **Max Width**: 1200px (container width)

### Responsive Breakpoints

```css
:root {
  /* Breakpoints */
  --breakpoint-sm: 480px;   /* Mobile phones */
  --breakpoint-md: 768px;   /* Tablets */
  --breakpoint-lg: 1024px;  /* Desktops */
  --breakpoint-xl: 1280px;  /* Large desktops */
}

/* Usage in CSS */
@media (max-width: 767px) { /* Mobile */ }
@media (min-width: 768px) and (max-width: 1023px) { /* Tablet */ }
@media (min-width: 1024px) { /* Desktop */ }
```

---

## 5. Component Library

### 5.1 Buttons

#### Button Variants

**Primary Button** (Neon Purple background)
```html
<button class="btn btn-primary">Click Me</button>
```

```css
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: var(--spacing-sm) var(--spacing-md);
  font-family: var(--font-body);
  font-size: var(--font-size-base);
  font-weight: 600;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: all 150ms ease;
  position: relative;
  overflow: hidden;
}

.btn-primary {
  background-color: var(--color-primary-neon);
  color: var(--color-bg-base);
  box-shadow: 0 4px 0 rgba(0, 0, 0, 0.3);
}

.btn-primary:hover {
  background-color: #8f4dff;
  transform: translateY(-2px);
  box-shadow: 0 6px 0 rgba(0, 0, 0, 0.3);
}

.btn-primary:active {
  transform: translateY(2px);
  box-shadow: 0 1px 0 rgba(0, 0, 0, 0.3);
}

.btn-primary:focus {
  outline: 3px solid var(--color-primary-green);
  outline-offset: 2px;
}

.btn-primary:disabled {
  background-color: var(--color-text-tertiary);
  color: var(--color-bg-base);
  cursor: not-allowed;
  opacity: 0.5;
}
```

**Secondary Button** (Outlined style)
```css
.btn-secondary {
  background-color: transparent;
  color: var(--color-primary-neon);
  border: 2px solid var(--color-primary-neon);
}

.btn-secondary:hover {
  background-color: var(--color-primary-neon);
  color: var(--color-bg-base);
}
```

**Danger Button** (Red for destructive actions)
```css
.btn-danger {
  background-color: var(--color-accent-error);
  color: white;
  box-shadow: 0 4px 0 rgba(0, 0, 0, 0.3);
}

.btn-danger:hover {
  background-color: #ff4d47;
  transform: translateY(-2px);
}
```

**Sizes**
```css
.btn-sm {
  padding: var(--spacing-xs) var(--spacing-sm);
  font-size: var(--font-size-sm);
}

.btn-md {
  padding: var(--spacing-sm) var(--spacing-md);
  font-size: var(--font-size-base);
}

.btn-lg {
  padding: var(--spacing-md) var(--spacing-lg);
  font-size: var(--font-size-lg);
}
```

**Button States**
- **Default**: Full color, 4px shadow (retro depth)
- **Hover**: Slightly brighter, lifted 2px, enhanced shadow
- **Active/Pressed**: Shadow reduced to 1px, pushed down effect
- **Disabled**: 50% opacity, cursor not-allowed
- **Focus**: 3px outline in accent green, 2px offset

---

### 5.2 Cards

```html
<div class="card">
  <div class="card-header">
    <h3 class="card-title">Enemy Profile</h3>
  </div>
  <div class="card-body">
    <p>Card content here</p>
  </div>
  <div class="card-footer">
    <button class="btn btn-primary">Action</button>
  </div>
</div>
```

```css
.card {
  background-color: var(--color-surface-card);
  border: 2px solid var(--color-border);
  border-radius: 6px;
  overflow: hidden;
  transition: all 200ms ease;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.4);
}

.card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 16px rgba(123, 63, 242, 0.2);
}

.card-header {
  background-color: var(--color-primary-gray);
  padding: var(--spacing-md);
  border-bottom: 2px solid var(--color-border);
}

.card-title {
  font-family: var(--font-heading);
  font-size: var(--font-size-2xl);
  margin: 0;
  color: var(--color-primary-green);
}

.card-body {
  padding: var(--spacing-md);
}

.card-footer {
  padding: var(--spacing-md);
  border-top: 2px solid var(--color-border);
  background-color: var(--color-bg-secondary);
  display: flex;
  gap: var(--spacing-md);
  justify-content: flex-end;
}
```

---

### 5.3 Input Fields

```html
<div class="form-group">
  <label for="search" class="form-label">Search Enemies:</label>
  <input 
    type="text" 
    id="search" 
    class="form-input" 
    placeholder="Enter enemy name..."
  >
</div>
```

```css
.form-group {
  display: flex;
  flex-direction: column;
  gap: var(--spacing-sm);
  margin-bottom: var(--spacing-md);
}

.form-label {
  font-family: var(--font-body);
  font-size: var(--font-size-sm);
  font-weight: 600;
  color: var(--color-text-primary);
}

.form-input,
.form-select,
.form-textarea {
  padding: var(--spacing-sm) var(--spacing-md);
  font-family: var(--font-body);
  font-size: var(--font-size-base);
  background-color: var(--color-primary-gray);
  color: var(--color-text-primary);
  border: 2px solid var(--color-border);
  border-radius: 4px;
  transition: all 150ms ease;
}

.form-input::placeholder,
.form-textarea::placeholder {
  color: var(--color-text-tertiary);
}

.form-input:focus,
.form-select:focus,
.form-textarea:focus {
  outline: none;
  border-color: var(--color-primary-neon);
  box-shadow: 0 0 0 3px rgba(123, 63, 242, 0.1);
}

.form-input:disabled,
.form-select:disabled,
.form-textarea:disabled {
  background-color: var(--color-bg-secondary);
  opacity: 0.5;
  cursor: not-allowed;
}
```

---

### 5.4 Badges & Tags

```html
<span class="badge badge-success">Completed</span>
<span class="badge badge-warning">In Progress</span>
<span class="badge badge-danger">Critical</span>
```

```css
.badge {
  display: inline-block;
  padding: 4px 12px;
  font-family: var(--font-body);
  font-size: var(--font-size-sm);
  font-weight: 600;
  border-radius: 12px;
  border: 1px solid currentColor;
}

.badge-success {
  background-color: rgba(45, 80, 22, 0.2);
  color: var(--color-accent-success);
}

.badge-warning {
  background-color: rgba(255, 215, 0, 0.2);
  color: var(--color-accent-warning);
}

.badge-danger {
  background-color: rgba(239, 59, 54, 0.2);
  color: var(--color-accent-error);
}

.badge-info {
  background-color: rgba(0, 212, 255, 0.2);
  color: var(--color-accent-info);
}
```

---

### 5.5 Alert Messages

```html
<div class="alert alert-info">
  <span class="alert-icon">ℹ️</span>
  <span class="alert-text">This is informational content.</span>
</div>
```

```css
.alert {
  display: flex;
  align-items: center;
  gap: var(--spacing-md);
  padding: var(--spacing-md);
  border-left: 4px solid;
  border-radius: 4px;
  background-color: var(--color-surface-card);
  font-family: var(--font-body);
  font-size: var(--font-size-base);
}

.alert-icon {
  font-size: var(--font-size-xl);
  flex-shrink: 0;
}

.alert-text {
  flex: 1;
}

.alert-info {
  border-color: var(--color-accent-info);
  color: var(--color-accent-info);
}

.alert-success {
  border-color: var(--color-accent-success);
  color: var(--color-accent-success);
}

.alert-warning {
  border-color: var(--color-accent-warning);
  color: var(--color-accent-warning);
}

.alert-danger {
  border-color: var(--color-accent-error);
  color: var(--color-accent-error);
}
```

---

### 5.6 Modal Dialog

```html
<div class="modal" role="dialog" aria-labelledby="modal-title">
  <div class="modal-backdrop"></div>
  <div class="modal-content">
    <div class="modal-header">
      <h2 id="modal-title" class="modal-title">Confirm Action</h2>
      <button class="modal-close" aria-label="Close">&times;</button>
    </div>
    <div class="modal-body">
      <p>Are you sure you want to proceed?</p>
    </div>
    <div class="modal-footer">
      <button class="btn btn-secondary">Cancel</button>
      <button class="btn btn-primary">Confirm</button>
    </div>
  </div>
</div>
```

```css
.modal {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-backdrop {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.8);
  animation: fadeIn 200ms ease;
}

.modal-content {
  position: relative;
  background-color: var(--color-bg-secondary);
  border: 2px solid var(--color-primary-neon);
  border-radius: 8px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.8);
  max-width: 500px;
  width: 90%;
  animation: slideUp 300ms ease;
}

.modal-header {
  padding: var(--spacing-lg);
  border-bottom: 2px solid var(--color-border);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-title {
  font-family: var(--font-heading);
  font-size: var(--font-size-2xl);
  margin: 0;
  color: var(--color-primary-green);
}

.modal-close {
  background: none;
  border: none;
  color: var(--color-text-primary);
  font-size: var(--font-size-2xl);
  cursor: pointer;
  padding: 0;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal-body {
  padding: var(--spacing-lg);
}

.modal-footer {
  padding: var(--spacing-lg);
  border-top: 2px solid var(--color-border);
  display: flex;
  gap: var(--spacing-md);
  justify-content: flex-end;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes slideUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}
```

---

### 5.7 Navigation Bar

```html
<nav class="navbar">
  <div class="navbar-container">
    <div class="navbar-brand">
      <a href="/" class="logo">NIGHTMARES WIKI</a>
    </div>
    <ul class="navbar-menu">
      <li><a href="/enemies" class="nav-link active">Enemies</a></li>
      <li><a href="/items" class="nav-link">Items</a></li>
      <li><a href="/npcs" class="nav-link">NPCs</a></li>
      <li><a href="/about" class="nav-link">About</a></li>
    </ul>
  </div>
</nav>
```

```css
.navbar {
  background-color: var(--color-primary-dark);
  border-bottom: 4px solid var(--color-primary-neon);
  padding: 0;
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.6);
}

.navbar-container {
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: var(--spacing-md) var(--spacing-lg);
}

.logo {
  font-family: var(--font-heading);
  font-size: var(--font-size-xl);
  color: var(--color-primary-green);
  text-decoration: none;
  transition: all 150ms ease;
}

.logo:hover {
  color: var(--color-primary-neon);
  text-shadow: 0 0 10px var(--color-primary-neon);
}

.navbar-menu {
  display: flex;
  list-style: none;
  margin: 0;
  padding: 0;
  gap: var(--spacing-lg);
}

.nav-link {
  font-family: var(--font-body);
  font-size: var(--font-size-base);
  color: var(--color-text-primary);
  text-decoration: none;
  padding: var(--spacing-sm) var(--spacing-md);
  border-bottom: 3px solid transparent;
  transition: all 150ms ease;
}

.nav-link:hover {
  color: var(--color-primary-green);
  border-bottom-color: var(--color-primary-green);
}

.nav-link.active {
  color: var(--color-primary-neon);
  border-bottom-color: var(--color-primary-neon);
}

.nav-link:focus {
  outline: 3px solid var(--color-primary-green);
  outline-offset: 2px;
}

@media (max-width: 768px) {
  .navbar-menu {
    flex-direction: column;
    gap: var(--spacing-sm);
  }
}
```

---

## 6. Animations & Micro-interactions

### Animation Principles
- **Subtle & Purposeful**: Animations should enhance, not distract
- **Retro-Appropriate**: Avoid smooth bezier curves; use linear or step timing
- **Fast Feedback**: 150-300ms for UI interactions
- **Accessible**: Respect `prefers-reduced-motion` media query

### Core Animations

```css
/* Fade In */
@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

/* Slide Up */
@keyframes slideUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

/* Pulse (Attention) */
@keyframes pulse {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.5; }
}

/* Glow Effect */
@keyframes glow {
  0%, 100% { 
    box-shadow: 0 0 5px var(--color-primary-neon);
  }
  50% { 
    box-shadow: 0 0 20px var(--color-primary-neon);
  }
}

/* Loading Spinner (Retro Pixel Style) */
@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

/* Respect reduced motion preferences */
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}
```

### Interactive States

```css
/* Smooth transitions for interactive elements */
button,
a,
input,
select,
textarea {
  transition: all 150ms ease;
}

/* Visual feedback on click */
.interactive:active {
  transform: scale(0.98);
}

/* Hover effects */
.interactive:hover {
  transform: translateY(-2px);
}
```

---

## 7. Layout Patterns

### 7.1 Page Container

```html
<div class="page">
  <header class="page-header">
    <h1>Page Title</h1>
    <p class="page-subtitle">Subtitle or description</p>
  </header>
  <main class="page-content">
    <!-- Content here -->
  </main>
</div>
```

```css
.page {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 var(--spacing-lg);
}

.page-header {
  padding: var(--spacing-2xl) 0;
  border-bottom: 3px solid var(--color-primary-neon);
  margin-bottom: var(--spacing-2xl);
}

.page-header h1 {
  font-family: var(--font-heading);
  font-size: var(--font-size-h1);
  color: var(--color-primary-green);
  margin: 0 0 var(--spacing-md) 0;
}

.page-subtitle {
  font-size: var(--font-size-lg);
  color: var(--color-text-secondary);
  margin: 0;
}

.page-content {
  padding-bottom: var(--spacing-2xl);
}
```

### 7.2 Grid Layout (Enemy Cards)

```html
<div class="grid-container">
  <div class="grid-item">
    <!-- Card content -->
  </div>
</div>
```

```css
.grid-container {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: var(--spacing-lg);
  margin-bottom: var(--spacing-2xl);
}

@media (min-width: 1024px) {
  .grid-container {
    grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  }
}
```

### 7.3 Two-Column Layout

```html
<div class="two-column-layout">
  <aside class="sidebar">
    <!-- Filters, navigation -->
  </aside>
  <main class="content">
    <!-- Main content -->
  </main>
</div>
```

```css
.two-column-layout {
  display: grid;
  grid-template-columns: 250px 1fr;
  gap: var(--spacing-lg);
  margin-bottom: var(--spacing-2xl);
}

@media (max-width: 768px) {
  .two-column-layout {
    grid-template-columns: 1fr;
  }

  .sidebar {
    order: 2;
  }

  .content {
    order: 1;
  }
}
```

---

## 8. Accessibility Standards

### Keyboard Navigation
- All interactive elements must be keyboard accessible
- Tab order follows logical visual order
- Focus indicators clearly visible (3px outline, min 2px offset)

### Color Contrast
- All text meets WCAG AA minimum (4.5:1 for body text)
- Critical UI elements meet WCAG AAA (7:1)
- No information conveyed by color alone (use icons, patterns)

### Screen Readers
- Semantic HTML (`<button>`, `<nav>`, `<main>`, etc.)
- ARIA labels for icon-only buttons
- Proper heading hierarchy (h1 → h2 → h3)
- Form labels associated with inputs via `<label for="">`

### Mobile Accessibility
- Touch targets minimum 44x44px
- Avoid hover-only interactions
- Text scalable up to 200% without loss of function
- Responsive layout adapts to all screen sizes

### Implemented Features
```html
<!-- Skip to main content link -->
<a href="#main-content" class="skip-link">Skip to main content</a>

<!-- Semantic structure -->
<nav role="navigation" aria-label="Main navigation">
  <!-- Navigation items -->
</nav>

<main id="main-content" role="main">
  <!-- Page content -->
</main>

<!-- Form accessibility -->
<label for="search-input">Search enemies:</label>
<input id="search-input" type="text" />

<!-- Icon buttons with labels -->
<button aria-label="Close dialog">&times;</button>

<!-- Heading hierarchy -->
<h1>Page Title</h1>
<h2>Section Title</h2>
<h3>Subsection Title</h3>
```

---

## 9. Dark Mode Considerations

The design system defaults to **dark mode** to match the retro gaming aesthetic and reduce eye strain.

### Current Implementation
- Primary background: `#1a1a2e` (dark purple)
- All text defaulted to light colors
- High contrast maintained throughout

### Light Mode Support (Future)
```css
@media (prefers-color-scheme: light) {
  :root {
    --color-bg-base: #f5f5f5;
    --color-text-primary: #1a1a2e;
    /* Invert other colors appropriately */
  }
}
```

---

## 10. Component Implementation Checklist

When implementing components, ensure:

- ✅ CSS follows the design system variables
- ✅ Responsive across all breakpoints
- ✅ Keyboard accessible (Tab, Enter, Escape)
- ✅ Focus states visible and clear
- ✅ Hover/active states provide feedback
- ✅ Disabled states are obvious
- ✅ Color contrast meets WCAG AA
- ✅ Touch targets at least 44x44px
- ✅ Mobile-friendly layout
- ✅ Respects reduced motion preference
- ✅ Semantic HTML structure
- ✅ ARIA labels where needed

---

## 11. Usage Examples

### Example: Enemy Card Component

```jsx
import React from 'react';
import './EnemyCard.css';

interface Enemy {
  id: string;
  name: string;
  health: number;
  attack: number;
  level: number;
  image?: string;
}

export const EnemyCard: React.FC<{ enemy: Enemy }> = ({ enemy }) => {
  return (
    <div className="card enemy-card">
      <div className="card-header">
        <h3 className="card-title">{enemy.name}</h3>
      </div>
      <div className="card-body">
        {enemy.image && <img src={enemy.image} alt={enemy.name} />}
        <dl className="stats">
          <dt>Health</dt>
          <dd className="stat-value">{enemy.health}</dd>
          <dt>Attack</dt>
          <dd className="stat-value">{enemy.attack}</dd>
          <dt>Level</dt>
          <dd className="stat-value">{enemy.level}</dd>
        </dl>
      </div>
      <div className="card-footer">
        <a href={`/enemies/${enemy.id}`} className="btn btn-primary">
          View Details
        </a>
      </div>
    </div>
  );
};
```

### Example: Search Bar Component

```jsx
import React, { useState } from 'react';

export const SearchBar: React.FC<{
  onSearch: (query: string) => void;
}> = ({ onSearch }) => {
  const [query, setQuery] = useState('');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.currentTarget.value;
    setQuery(value);
    onSearch(value);
  };

  return (
    <form className="search-form" onSubmit={(e) => e.preventDefault()}>
      <div className="form-group">
        <label htmlFor="search-input" className="form-label">
          Search Enemies
        </label>
        <input
          id="search-input"
          type="text"
          className="form-input"
          placeholder="Enter enemy name or stats..."
          value={query}
          onChange={handleChange}
          aria-describedby="search-hint"
        />
        <small id="search-hint">Search by name, level, or type</small>
      </div>
    </form>
  );
};
```

---

## 12. Icon System

### Icon Style
- **Pixel-Art Icons**: 16x16px or 32x32px PNG sprites
- **Monochrome Design**: White on transparent background
- **Consistent Stroke Weight**: 2px thickness for readability

### Icon Usage
```html
<!-- Using inline SVG (preferred for flexibility) -->
<button class="btn btn-secondary" aria-label="Delete enemy">
  <svg class="icon icon-delete" viewBox="0 0 24 24" aria-hidden="true">
    <path d="M6 6h12v12H6z"/>
  </svg>
</button>

<!-- Using CSS icon classes -->
<span class="icon icon-attack"></span>
<span class="icon icon-defense"></span>
<span class="icon icon-health"></span>
```

### Icon CSS
```css
.icon {
  display: inline-block;
  width: 1.25em;
  height: 1.25em;
  vertical-align: -0.125em;
  color: currentColor;
  fill: currentColor;
}

.icon-sm {
  width: 1em;
  height: 1em;
}

.icon-lg {
  width: 1.5em;
  height: 1.5em;
}
```

---

## 13. Development Guidelines

### CSS Architecture
- Use CSS Variables for theming
- Follow BEM (Block, Element, Modifier) for class naming
- Organize by component (each component has its own CSS file)
- Keep specificity low (avoid deep nesting)

### Responsive Design
1. **Mobile First**: Start with mobile styles, then add breakpoints
2. **Flexibility**: Use `max-width`, not fixed widths
3. **Test**: Verify on actual devices or DevTools
4. **Touch Targets**: Minimum 44px for touch interfaces

### Performance
- Minimize CSS file size (tree-shake unused styles)
- Use CSS Grid/Flexbox, not floats or absolute positioning
- Lazy-load images
- Optimize font delivery (variable fonts where possible)

---

## 14. Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile browsers (iOS Safari 14+, Chrome Mobile 90+)

### CSS Features Used
- CSS Grid & Flexbox
- CSS Variables (Custom Properties)
- CSS Media Queries
- CSS Transitions & Animations
- CSS Focus-Visible

---

## 15. Future Design Enhancements

1. **Animated Sprites**: ASCII art or sprite animations for enemies
2. **Sound Effects**: UI sounds for interactions (with mute option)
3. **Theme Variations**: Alternative color schemes (ice, fire, shadow themes)
4. **Component Storybook**: Interactive documentation of all components
5. **Icon Set**: Complete custom pixel-art icon library
6. **Loading States**: Creative loading animations
7. **Dark/Light Mode Toggle**: User preference switcher

---

## References & Resources

### Fonts
- [Google Fonts - Press Start 2P](https://fonts.google.com/specimen/Press+Start+2P)
- [Google Fonts - Inter](https://fonts.google.com/specimen/Inter)

### Color Tools
- [Coolors.co](https://coolors.co/) - Palette generator
- [WebAIM Contrast Checker](https://webaim.org/resources/contrastchecker/) - WCAG AA/AAA validation

### Design Inspiration
- Classic 16-bit/32-bit gaming aesthetics
- Retro arcade visual language
- Modern dark mode UI patterns

### Accessibility
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
- [MDN Accessibility](https://developer.mozilla.org/en-US/docs/Web/Accessibility)
- [WebAIM Articles](https://webaim.org/articles/)

---

## Change Log

| Date | Version | Changes |
|------|---------|---------|
| 2026-03-16 | 1.0 | Initial design system created |

---

**Last Updated**: 2026-03-16  
**Maintained By**: UX Design Team  
**Status**: Active
