# UI Component Implementation Guide

## Quick Reference for Developers

This guide provides ready-to-use CSS and React component patterns aligned with the Design System.

---

## 1. CSS Import & Initialization

### Global Styles Setup

```css
/* Global.css - Import at app root */

@import url('https://fonts.googleapis.com/css2?family=Press+Start+2P&display=swap');
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');

/* CSS Variables */
:root {
  /* Colors - Primary */
  --color-primary-dark: #1a1a2e;
  --color-primary-neon: #7b3ff2;
  --color-primary-green: #39ff14;
  --color-primary-gray: #2a2a3e;

  /* Colors - Accents */
  --color-accent-error: #ef3b36;
  --color-accent-warning: #ffd700;
  --color-accent-info: #00d4ff;
  --color-accent-success: #2d5016;

  /* Colors - Neutral */
  --color-text-primary: #e8e8e8;
  --color-text-secondary: #a8a8a8;
  --color-text-tertiary: #666666;
  --color-bg-base: #1a1a2e;
  --color-bg-secondary: #2a2a3e;
  --color-border: #3a3a4e;

  /* Colors - Surfaces */
  --color-surface-card: #252538;
  --color-surface-elevated: #353548;

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

  /* Breakpoints */
  --breakpoint-sm: 480px;
  --breakpoint-md: 768px;
  --breakpoint-lg: 1024px;
  --breakpoint-xl: 1280px;

  /* Shadows */
  --shadow-sm: 0 2px 4px rgba(0, 0, 0, 0.4);
  --shadow-md: 0 4px 8px rgba(0, 0, 0, 0.4);
  --shadow-lg: 0 8px 16px rgba(123, 63, 242, 0.2);
  --shadow-xl: 0 20px 60px rgba(0, 0, 0, 0.8);

  /* Transitions */
  --transition-fast: 150ms ease;
  --transition-normal: 200ms ease;
  --transition-slow: 300ms ease;
}

/* Base Styles */
html {
  color-scheme: dark;
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

/* Typography */
h1, h2, h3, h4, h5, h6 {
  margin: var(--spacing-md) 0 var(--spacing-sm) 0;
  line-height: var(--line-height-tight);
  font-weight: 700;
}

h1 {
  font-family: var(--font-heading);
  font-size: var(--font-size-h1);
  color: var(--color-primary-green);
}

h2 {
  font-family: var(--font-heading);
  font-size: var(--font-size-h2);
  color: var(--color-primary-neon);
}

h3 {
  font-family: var(--font-heading);
  font-size: var(--font-size-h3);
  color: var(--color-primary-neon);
}

p {
  margin: 0 0 var(--spacing-md) 0;
}

a {
  color: var(--color-primary-neon);
  text-decoration: none;
  transition: color var(--transition-fast);
}

a:hover {
  color: var(--color-primary-green);
  text-decoration: underline;
}

a:focus {
  outline: 3px solid var(--color-primary-green);
  outline-offset: 2px;
  border-radius: 2px;
}

/* Respect Reduced Motion */
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

---

## 2. Button Component

### HTML Structure
```html
<!-- Primary Button -->
<button class="btn btn-primary">Click Me</button>

<!-- Secondary Button -->
<button class="btn btn-secondary">Cancel</button>

<!-- Danger Button -->
<button class="btn btn-danger">Delete</button>

<!-- Disabled State -->
<button class="btn btn-primary" disabled>Loading...</button>

<!-- With Icon -->
<button class="btn btn-primary">
  <svg class="icon" viewBox="0 0 24 24" aria-hidden="true">
    <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2z"/>
  </svg>
  Click Me
</button>
```

### CSS
```css
/* Button Base */
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
  text-decoration: none;
  white-space: nowrap;
  user-select: none;
}

/* Button Primary */
.btn-primary {
  background-color: var(--color-primary-neon);
  color: var(--color-primary-dark);
  box-shadow: 0 4px 0 rgba(0, 0, 0, 0.3);
}

.btn-primary:hover:not(:disabled) {
  background-color: #8f4dff;
  transform: translateY(-2px);
  box-shadow: 0 6px 0 rgba(0, 0, 0, 0.3);
}

.btn-primary:active:not(:disabled) {
  transform: translateY(2px);
  box-shadow: 0 1px 0 rgba(0, 0, 0, 0.3);
}

/* Button Secondary */
.btn-secondary {
  background-color: transparent;
  color: var(--color-primary-neon);
  border: 2px solid var(--color-primary-neon);
}

.btn-secondary:hover:not(:disabled) {
  background-color: var(--color-primary-neon);
  color: var(--color-primary-dark);
}

/* Button Danger */
.btn-danger {
  background-color: var(--color-accent-error);
  color: white;
  box-shadow: 0 4px 0 rgba(0, 0, 0, 0.3);
}

.btn-danger:hover:not(:disabled) {
  background-color: #ff4d47;
  transform: translateY(-2px);
  box-shadow: 0 6px 0 rgba(0, 0, 0, 0.3);
}

/* Button Sizes */
.btn-sm {
  padding: var(--spacing-xs) var(--spacing-sm);
  font-size: var(--font-size-sm);
}

.btn-lg {
  padding: var(--spacing-md) var(--spacing-lg);
  font-size: var(--font-size-lg);
}

/* Button States */
.btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none !important;
}

.btn:focus-visible {
  outline: 3px solid var(--color-primary-green);
  outline-offset: 2px;
}

/* Icon in Button */
.btn .icon {
  width: 1.2em;
  height: 1.2em;
}
```

### React Component
```typescript
import React from 'react';
import './Button.css';

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'danger';
  size?: 'sm' | 'md' | 'lg';
  icon?: React.ReactNode;
  loading?: boolean;
  children: React.ReactNode;
}

export const Button: React.FC<ButtonProps> = ({
  variant = 'primary',
  size = 'md',
  icon,
  loading = false,
  children,
  disabled,
  ...props
}) => {
  return (
    <button
      className={`btn btn-${variant} btn-${size}`}
      disabled={disabled || loading}
      {...props}
    >
      {icon && <span className="btn-icon">{icon}</span>}
      {loading ? 'Loading...' : children}
    </button>
  );
};
```

---

## 3. Card Component

### HTML Structure
```html
<div class="card">
  <div class="card-header">
    <h3 class="card-title">Enemy Profile</h3>
  </div>
  <div class="card-body">
    <p>Card content goes here</p>
  </div>
  <div class="card-footer">
    <button class="btn btn-primary">Action</button>
  </div>
</div>
```

### CSS
```css
.card {
  background-color: var(--color-surface-card);
  border: 2px solid var(--color-border);
  border-radius: 6px;
  overflow: hidden;
  transition: all var(--transition-normal);
  box-shadow: var(--shadow-md);
  display: flex;
  flex-direction: column;
}

.card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-lg);
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
  flex: 1;
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

### React Component
```typescript
import React from 'react';
import './Card.css';

interface CardProps {
  title?: string;
  children: React.ReactNode;
  footer?: React.ReactNode;
  hoverable?: boolean;
}

export const Card: React.FC<CardProps> = ({
  title,
  children,
  footer,
  hoverable = true,
}) => {
  return (
    <div className={`card ${hoverable ? 'card-hoverable' : ''}`}>
      {title && (
        <div className="card-header">
          <h3 className="card-title">{title}</h3>
        </div>
      )}
      <div className="card-body">{children}</div>
      {footer && <div className="card-footer">{footer}</div>}
    </div>
  );
};
```

---

## 4. Input Field Component

### HTML Structure
```html
<div class="form-group">
  <label for="search" class="form-label">Search</label>
  <input
    type="text"
    id="search"
    class="form-input"
    placeholder="Enter search term..."
    aria-describedby="search-hint"
  />
  <small id="search-hint">Search by name or stats</small>
</div>
```

### CSS
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
  transition: all var(--transition-fast);
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

.form-input::selection {
  background-color: var(--color-primary-neon);
  color: var(--color-primary-dark);
}
```

### React Component
```typescript
import React from 'react';
import './FormInput.css';

interface FormInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  hint?: string;
}

export const FormInput: React.FC<FormInputProps> = ({
  label,
  error,
  hint,
  id,
  ...props
}) => {
  return (
    <div className="form-group">
      {label && <label htmlFor={id} className="form-label">{label}</label>}
      <input {...props} id={id} className="form-input" />
      {error && <small className="form-error">{error}</small>}
      {hint && <small className="form-hint">{hint}</small>}
    </div>
  );
};
```

---

## 5. Badge Component

### HTML Structure
```html
<span class="badge badge-success">Completed</span>
<span class="badge badge-warning">In Progress</span>
<span class="badge badge-danger">Critical</span>
<span class="badge badge-info">Information</span>
```

### CSS
```css
.badge {
  display: inline-block;
  padding: 4px 12px;
  font-family: var(--font-body);
  font-size: var(--font-size-sm);
  font-weight: 600;
  border-radius: 12px;
  border: 1px solid currentColor;
  white-space: nowrap;
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

### React Component
```typescript
import React from 'react';
import './Badge.css';

type BadgeVariant = 'success' | 'warning' | 'danger' | 'info';

interface BadgeProps {
  variant: BadgeVariant;
  children: React.ReactNode;
}

export const Badge: React.FC<BadgeProps> = ({ variant, children }) => {
  return <span className={`badge badge-${variant}`}>{children}</span>;
};
```

---

## 6. Alert Component

### HTML Structure
```html
<div class="alert alert-info" role="alert">
  <span class="alert-icon">ℹ️</span>
  <span class="alert-text">This is informational content.</span>
</div>
```

### CSS
```css
.alert {
  display: flex;
  align-items: flex-start;
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
  line-height: 1;
}

.alert-text {
  flex: 1;
  line-height: var(--line-height-normal);
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

### React Component
```typescript
import React from 'react';
import './Alert.css';

type AlertVariant = 'info' | 'success' | 'warning' | 'danger';

interface AlertProps {
  variant: AlertVariant;
  icon?: string;
  children: React.ReactNode;
}

export const Alert: React.FC<AlertProps> = ({
  variant,
  icon = '!',
  children,
}) => {
  return (
    <div className={`alert alert-${variant}`} role="alert">
      <span className="alert-icon">{icon}</span>
      <span className="alert-text">{children}</span>
    </div>
  );
};
```

---

## 7. Grid Container

### HTML Structure
```html
<div class="grid-container">
  <div class="grid-item">
    <!-- Card or content -->
  </div>
  <div class="grid-item">
    <!-- Card or content -->
  </div>
</div>
```

### CSS
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

@media (max-width: 768px) {
  .grid-container {
    grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
    gap: var(--spacing-md);
  }
}

@media (max-width: 480px) {
  .grid-container {
    grid-template-columns: 1fr;
    gap: var(--spacing-md);
  }
}
```

---

## 8. Stat Bar Component

### HTML Structure
```html
<div class="stat-bar">
  <div class="stat-label">Attack</div>
  <div class="stat-value">85</div>
  <div class="stat-container">
    <div class="stat-fill" style="width: 85%"></div>
  </div>
</div>
```

### CSS
```css
.stat-bar {
  display: grid;
  grid-template-columns: 80px 40px 1fr;
  gap: var(--spacing-md);
  align-items: center;
  margin-bottom: var(--spacing-md);
}

.stat-label {
  font-weight: 600;
  font-size: var(--font-size-sm);
  color: var(--color-text-secondary);
}

.stat-value {
  text-align: center;
  font-weight: 700;
  font-family: var(--font-mono);
  color: var(--color-primary-neon);
}

.stat-container {
  position: relative;
  height: 24px;
  background-color: var(--color-primary-gray);
  border: 1px solid var(--color-border);
  border-radius: 2px;
  overflow: hidden;
}

.stat-fill {
  height: 100%;
  background: linear-gradient(90deg, var(--color-primary-green), var(--color-accent-warning));
  transition: width 300ms ease;
}
```

### React Component
```typescript
import React from 'react';
import './StatBar.css';

interface StatBarProps {
  label: string;
  value: number;
  max?: number;
}

export const StatBar: React.FC<StatBarProps> = ({ label, value, max = 100 }) => {
  const percentage = (value / max) * 100;

  return (
    <div className="stat-bar">
      <div className="stat-label">{label}</div>
      <div className="stat-value">{value}</div>
      <div className="stat-container">
        <div className="stat-fill" style={{ width: `${percentage}%` }} />
      </div>
    </div>
  );
};
```

---

## 9. Common Patterns

### Loading Skeleton
```css
.skeleton {
  background: linear-gradient(
    90deg,
    var(--color-primary-gray) 0%,
    var(--color-bg-secondary) 50%,
    var(--color-primary-gray) 100%
  );
  background-size: 200% 100%;
  animation: loading 1.5s infinite;
}

@keyframes loading {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}
```

### Focus Ring
```css
:focus-visible {
  outline: 3px solid var(--color-primary-green);
  outline-offset: 2px;
  border-radius: 2px;
}
```

### Responsive Container
```css
.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 var(--spacing-lg);
}

@media (max-width: 768px) {
  .container {
    padding: 0 var(--spacing-md);
  }
}
```

---

## 10. Testing Components

### Example: Button Component Test
```typescript
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Button } from './Button';

describe('Button', () => {
  it('renders primary button', () => {
    render(<Button>Click Me</Button>);
    const btn = screen.getByRole('button', { name: /click me/i });
    expect(btn).toBeInTheDocument();
    expect(btn).toHaveClass('btn-primary');
  });

  it('handles click event', async () => {
    const handleClick = jest.fn();
    render(<Button onClick={handleClick}>Click Me</Button>);
    
    const btn = screen.getByRole('button');
    await userEvent.click(btn);
    
    expect(handleClick).toHaveBeenCalledTimes(1);
  });

  it('respects disabled state', () => {
    render(<Button disabled>Click Me</Button>);
    const btn = screen.getByRole('button');
    expect(btn).toBeDisabled();
  });
});
```

---

## 11. Accessibility Checklist

When implementing components:

- ✅ Semantic HTML (`<button>`, `<input>`, `<label>`, etc.)
- ✅ ARIA labels for icon-only buttons
- ✅ Proper `for` attribute on labels
- ✅ Focus visible indicators
- ✅ Keyboard navigation support
- ✅ Color contrast meets WCAG AA
- ✅ Form inputs have associated labels
- ✅ Error messages are announced
- ✅ Disabled states are clear
- ✅ Touch targets at least 44x44px

---

## 12. Performance Tips

1. **CSS Variables**: Use for theming, not for every property
2. **Transitions**: Limit to necessary properties (avoid `all`)
3. **Shadows**: Use pre-calculated shadow values
4. **Grid Layout**: Use CSS Grid instead of floats
5. **Images**: Lazy-load with `loading="lazy"`
6. **Fonts**: Use `font-display: swap` for Google Fonts

---

**Document Version**: 1.0  
**Last Updated**: 2026-03-16  
**Status**: Ready for Development
