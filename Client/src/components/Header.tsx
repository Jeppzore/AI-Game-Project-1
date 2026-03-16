import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import './Header.css';

export const Header: React.FC = () => {
  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <header className="header">
      <nav className="header-nav">
        <div className="header-logo">
          <Link to="/">
            <span className="logo-icon">▲</span>
            <span className="logo-text">NIGHTMARES WIKI</span>
          </Link>
        </div>

        <button
          className="header-menu-toggle"
          onClick={() => setMenuOpen(!menuOpen)}
          aria-label="Toggle menu"
          aria-expanded={menuOpen}
        >
          <span className="menu-icon">☰</span>
        </button>

        <ul className={`header-links ${menuOpen ? 'open' : ''}`}>
          <li>
            <Link to="/" onClick={() => setMenuOpen(false)}>
              Enemies
            </Link>
          </li>
          <li>
            <a href="#items" onClick={() => setMenuOpen(false)}>
              Items
            </a>
          </li>
          <li>
            <a href="#npcs" onClick={() => setMenuOpen(false)}>
              NPCs
            </a>
          </li>
          <li>
            <a href="#about" onClick={() => setMenuOpen(false)}>
              About
            </a>
          </li>
        </ul>
      </nav>
    </header>
  );
};
