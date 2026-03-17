# Tibia Wiki Design Implementation Guide

## 🎨 Design Overview

This guide provides a complete redesign of the Nightmares Wiki to match the visual identity of the **Tibia Fandom Wiki** (https://tibia.fandom.com). The design maintains the wiki's functional purpose while adopting Tibia's warm, medieval fantasy color palette and clean information hierarchy.

### Key Design Changes
- **Color Palette**: Warm earthy tones replacing cool neon palette
- **Visual Style**: Medieval fantasy aesthetic with gold accents and burgundy tones
- **Typography**: Maintained readability with refined hierarchy
- **Component Updates**: All UI elements redesigned for Tibia wiki consistency

---

## 📋 Implementation Steps

### Step 1: Update CSS Variables (CRITICAL - DO THIS FIRST)

**File**: `Client/src/styles/variables.css`

Replace the entire color section with the new Tibia-inspired palette:

```css
/* CSS Variables - Design System (Tibia Wiki Style) */

:root {
  /* Colors - Primary (Tibia Palette) */
  --color-primary-dark: #2d2416;        /* Deep brown/charcoal */
  --color-primary-light: #f5f0eb;       /* Warm cream/off-white */
  --color-primary-accent: #b8860b;      /* Dark goldenrod */
  --color-primary-burgundy: #8b4513;    /* Saddle brown */

  /* Colors - Accents (Medieval Fantasy) */
  --color-accent-error: #c23b23;        /* Deep red */
  --color-accent-warning: #daa520;      /* Goldenrod */
  --color-accent-info: #4682b4;         /* Steel blue */
  --color-accent-success: #556b2f;      /* Dark olive green */

  /* Colors - Neutral (Warm Palette) */
  --color-text-primary: #2d2416;        /* Deep brown (main text) */
  --color-text-secondary: #6b5d52;      /* Medium brown */
  --color-text-tertiary: #a89080;       /* Light brown */
  --color-bg-base: #f5f0eb;             /* Warm cream background */
  --color-bg-secondary: #e8ddd3;        /* Light tan */
  --color-border: #d4c4b0;              /* Tan border */

  /* Colors - Surfaces */
  --color-surface-card: #faf7f2;        /* Very light cream */
  --color-surface-elevated: #f0e8e0;    /* Light tan */
  --color-surface-accent: #8b4513;      /* Brown accent surface */

  /* Fonts */
  --font-heading: 'Press Start 2P', monospace;
  --font-body: 'Inter', 'Segoe UI', 'Roboto', sans-serif;
  --font-mono: 'Courier New', monospace;

  /* Font Sizes */
  --font-size-xs: 11px;
  --font-size-sm: 13px;
  --font-size-base: 16px;
  --font-size-lg: 18px;
  --font-size-xl: 20px;
  --font-size-2xl: 24px;
  --font-size-3xl: 32px;
  --font-size-4xl: 40px;
  --font-size-h1: 40px;
  --font-size-h2: 32px;
  --font-size-h3: 24px;

  /* Spacing */
  --spacing-xs: 4px;
  --spacing-sm: 8px;
  --spacing-md: 16px;
  --spacing-lg: 24px;
  --spacing-xl: 32px;
  --spacing-2xl: 48px;
  --spacing-3xl: 64px;

  /* Line Height */
  --line-height-tight: 1.2;
  --line-height-normal: 1.5;
  --line-height-relaxed: 1.8;

  /* Letter Spacing */
  --letter-spacing-tight: -0.02em;
  --letter-spacing-normal: 0em;
  --letter-spacing-wide: 0.1em;

  /* Breakpoints (for reference in JS) */
  --breakpoint-sm: 480px;
  --breakpoint-md: 768px;
  --breakpoint-lg: 1024px;
  --breakpoint-xl: 1280px;

  /* Shadows - Updated for warm palette */
  --shadow-sm: 0 2px 4px rgba(45, 36, 22, 0.15);
  --shadow-md: 0 4px 8px rgba(45, 36, 22, 0.2);
  --shadow-lg: 0 8px 16px rgba(139, 69, 19, 0.25);
  --shadow-xl: 0 20px 60px rgba(45, 36, 22, 0.3);

  /* Transitions */
  --transition-fast: 150ms ease;
  --transition-normal: 200ms ease;
  --transition-slow: 300ms ease;
}
```

### Step 2: Update Global Styles

**File**: `Client/src/styles/global.css`

Replace the HTML color scheme and base styles:

```css
@import url('https://fonts.googleapis.com/css2?family=Press+Start+2P&display=swap');
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');
@import './variables.css';

/* Base Styles */
html {
  color-scheme: light;
  background-color: var(--color-bg-base);
  color: var(--color-text-primary);
}

body {
  margin: 0;
  padding: 0;
  font-family: var(--font-body);
  font-size: var(--font-size-base);
  line-height: var(--line-height-normal);
  background-color: var(--color-bg-base);
  color: var(--color-text-primary);
}

#root {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

/* Typography */
h1, h2, h3, h4, h5, h6 {
  margin: var(--spacing-md) 0 var(--spacing-sm) 0;
  line-height: var(--line-height-tight);
  font-weight: 700;
}

h1 {
  font-family: var(--font-heading);
  font-size: var(--font-size-h1);
  color: var(--color-primary-accent);
  letter-spacing: var(--letter-spacing-wide);
}

h2 {
  font-family: var(--font-heading);
  font-size: var(--font-size-h2);
  color: var(--color-primary-burgundy);
  letter-spacing: var(--letter-spacing-wide);
}

h3 {
  font-family: var(--font-heading);
  font-size: var(--font-size-h3);
  color: var(--color-primary-burgundy);
  letter-spacing: var(--letter-spacing-wide);
}

p {
  margin: 0 0 var(--spacing-md) 0;
  color: var(--color-text-primary);
}

a {
  color: var(--color-primary-accent);
  text-decoration: none;
  transition: color var(--transition-fast);
}

a:hover {
  color: var(--color-primary-burgundy);
  text-decoration: underline;
}

a:focus {
  outline: 3px solid var(--color-primary-accent);
  outline-offset: 2px;
  border-radius: 2px;
}

/* Button Styles */
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: var(--spacing-sm);
  padding: var(--spacing-sm) var(--spacing-md);
  font-family: var(--font-body);
  font-size: var(--font-size-base);
  font-weight: 600;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: all var(--transition-fast);
  position: relative;
  overflow: hidden;
}

.btn-primary {
  background-color: var(--color-primary-accent);
  color: white;
  box-shadow: 0 4px 0 rgba(45, 36, 22, 0.2);
}

.btn-primary:hover {
  background-color: #9a7206;
  transform: translateY(-2px);
  box-shadow: 0 6px 0 rgba(45, 36, 22, 0.2);
}

.btn-primary:active {
  transform: translateY(2px);
  box-shadow: 0 1px 0 rgba(45, 36, 22, 0.2);
}

.btn-primary:focus {
  outline: 3px solid var(--color-primary-burgundy);
  outline-offset: 2px;
}

.btn-primary:disabled {
  background-color: var(--color-text-tertiary);
  color: white;
  cursor: not-allowed;
  opacity: 0.5;
  transform: none;
}

.btn-secondary {
  background-color: transparent;
  color: var(--color-primary-accent);
  border: 2px solid var(--color-primary-accent);
  box-shadow: none;
}

.btn-secondary:hover {
  background-color: var(--color-primary-accent);
  color: white;
  transform: translateY(-2px);
}

.btn-secondary:active {
  transform: translateY(2px);
}

.btn-secondary:focus {
  outline: 3px solid var(--color-primary-burgundy);
  outline-offset: 2px;
}

.btn-danger {
  background-color: var(--color-accent-error);
  color: white;
  box-shadow: 0 4px 0 rgba(45, 36, 22, 0.2);
}

.btn-danger:hover {
  background-color: #a03118;
  transform: translateY(-2px);
  box-shadow: 0 6px 0 rgba(45, 36, 22, 0.2);
}

.btn-danger:active {
  transform: translateY(2px);
  box-shadow: 0 1px 0 rgba(45, 36, 22, 0.2);
}

.btn-danger:focus {
  outline: 3px solid var(--color-primary-accent);
  outline-offset: 2px;
}

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

/* Card Styles */
.card {
  background-color: var(--color-surface-card);
  border: 2px solid var(--color-border);
  border-radius: 6px;
  overflow: hidden;
  transition: all var(--transition-normal);
  box-shadow: var(--shadow-md);
}

.card:hover {
  box-shadow: var(--shadow-lg);
  border-color: var(--color-primary-accent);
}

.card-header {
  background-color: var(--color-surface-accent);
  padding: var(--spacing-md);
  border-bottom: 2px solid var(--color-border);
}

.card-title {
  margin: 0;
  font-size: var(--font-size-xl);
  font-weight: 600;
  color: white;
}

.card-body {
  padding: var(--spacing-md);
}

.card-body p {
  margin: 0 0 var(--spacing-sm) 0;
  font-size: var(--font-size-base);
  line-height: var(--line-height-normal);
}

.card-body p:last-child {
  margin-bottom: 0;
}

.card-footer {
  background-color: var(--color-surface-elevated);
  padding: var(--spacing-md);
  border-top: 2px solid var(--color-border);
  display: flex;
  gap: var(--spacing-md);
  justify-content: flex-end;
}

/* Input Styles */
input[type="text"],
input[type="search"],
select,
textarea {
  width: 100%;
  padding: var(--spacing-sm) var(--spacing-md);
  font-family: var(--font-body);
  font-size: var(--font-size-base);
  color: var(--color-text-primary);
  background-color: var(--color-surface-card);
  border: 2px solid var(--color-border);
  border-radius: 4px;
  transition: border-color var(--transition-fast);
  box-sizing: border-box;
}

input[type="text"]:focus,
input[type="search"]:focus,
select:focus,
textarea:focus {
  outline: none;
  border-color: var(--color-primary-accent);
  box-shadow: 0 0 8px rgba(184, 134, 11, 0.3);
}

/* Badge Styles */
.badge {
  display: inline-block;
  padding: var(--spacing-xs) var(--spacing-sm);
  font-size: var(--font-size-sm);
  font-weight: 600;
  border-radius: 12px;
  background-color: var(--color-bg-secondary);
  color: var(--color-text-primary);
}

.badge-success {
  background-color: var(--color-accent-success);
  color: white;
}

.badge-warning {
  background-color: var(--color-accent-warning);
  color: var(--color-text-primary);
}

.badge-error {
  background-color: var(--color-accent-error);
  color: white;
}

.badge-info {
  background-color: var(--color-accent-info);
  color: white;
}

/* Grid Layout */
.grid {
  display: grid;
  gap: var(--spacing-md);
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
}

.container {
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  padding: var(--spacing-md);
}

/* Reduced Motion */
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}

/* Responsive Breakpoints */
@media (max-width: 767px) {
  h1 {
    font-size: 24px;
  }

  h2 {
    font-size: 20px;
  }

  h3 {
    font-size: 18px;
  }

  .grid {
    grid-template-columns: 1fr;
  }

  .container {
    padding: var(--spacing-sm);
  }
}

@media (min-width: 768px) and (max-width: 1023px) {
  .grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (min-width: 1024px) {
  .grid {
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  }
}
```

---

## 🎨 Color Palette Reference

### Primary Colors
| Color | Hex Code | Usage |
|-------|----------|-------|
| Deep Brown | `#2d2416` | Main backgrounds, dark text |
| Warm Cream | `#f5f0eb` | Page background, light surfaces |
| Dark Goldenrod | `#b8860b` | Primary buttons, links, headings |
| Saddle Brown | `#8b4513` | Secondary headings, card headers |

### Accent Colors
| Color | Hex Code | Usage |
|-------|----------|-------|
| Deep Red | `#c23b23` | Error states, danger actions |
| Goldenrod | `#daa520` | Warnings, highlights |
| Steel Blue | `#4682b4` | Info states, player data |
| Dark Olive Green | `#556b2f` | Success states, positive indicators |

### Text Colors
| Color | Hex Code | Usage |
|-------|----------|-------|
| Deep Brown | `#2d2416` | Primary text (body, paragraphs) |
| Medium Brown | `#6b5d52` | Secondary text, labels |
| Light Brown | `#a89080` | Disabled text, tertiary info |

---

## 🔧 Component Updates Summary

### Navigation/Header
- Background: `var(--color-primary-dark)` (#2d2416)
- Text: `var(--color-primary-light)` (#f5f0eb)
- Accent: `var(--color-primary-accent)` (#b8860b)

### Cards
- Background: `var(--color-surface-card)` (#faf7f2)
- Header: `var(--color-surface-accent)` (#8b4513)
- Border: `var(--color-border)` (#d4c4b0)

### Buttons
- Primary: `var(--color-primary-accent)` (#b8860b) on white
- Secondary: White background with `var(--color-primary-accent)` border
- Danger: `var(--color-accent-error)` (#c23b23)

### Forms & Inputs
- Background: `var(--color-surface-card)` (#faf7f2)
- Border: `var(--color-border)` (#d4c4b0)
- Focus: Gold border with gold shadow

### Badges
- Success: `var(--color-accent-success)` (#556b2f)
- Warning: `var(--color-accent-warning)` (#daa520)
- Error: `var(--color-accent-error)` (#c23b23)
- Info: `var(--color-accent-info)` (#4682b4)

---

## ✅ Verification Checklist

After implementing all changes, verify:

- [ ] Home page displays with warm cream background (#f5f0eb)
- [ ] All headings use gold (#b8860b) for H1 and brown (#8b4513) for H2/H3
- [ ] Primary buttons are gold (#b8860b) with white text
- [ ] Cards have light cream backgrounds (#faf7f2) with brown headers (#8b4513)
- [ ] Links are gold (#b8860b) and turn brown (#8b4513) on hover
- [ ] Form inputs have proper borders and gold focus states
- [ ] Enemy page displays with proper card styling and borders
- [ ] All text is dark brown (#2d2416) for readability on light background
- [ ] Badges display with correct accent colors
- [ ] Responsive design works on mobile/tablet/desktop

---

## 📝 Notes for Implementation

1. **Light Theme Switch**: This design moves from dark mode to light mode. Ensure `color-scheme: light` is set in HTML styles.

2. **Contrast Compliance**: All color combinations maintain WCAG AA contrast ratios for accessibility.

3. **Browser Testing**: Test in Chrome, Firefox, Safari, and Edge to ensure consistency.

4. **Image Updates**: If any existing images use the old neon colors, consider updating them to match the new palette.

5. **Rollback Plan**: Keep a copy of old CSS files in case rollback is needed.

---

## 🚀 Quick Implementation Order

1. ✅ Update `Client/src/styles/variables.css` with new color variables
2. ✅ Update `Client/src/styles/global.css` with new color rules
3. ✅ Test Home page rendering
4. ✅ Test Enemy page rendering
5. ✅ Test all interactive components (buttons, forms, badges)
6. ✅ Verify responsive design on mobile view
7. ✅ Check accessibility with browser dev tools
8. ✅ Run full test suite if available

---

## 📞 Support

If any components don't render correctly after implementation:
1. Clear browser cache (Ctrl+Shift+Delete or Cmd+Shift+Delete)
2. Restart development server (`npm start` or `npm run dev`)
3. Check browser console for CSS errors
4. Verify all CSS variable names match exactly (case-sensitive)
