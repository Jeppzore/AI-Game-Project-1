import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import type { Enemy } from '../models/Enemy';
import api from '../services/api';
import { Card } from '../components/Card';
import { Button } from '../components/Button';
import { Badge } from '../components/Badge';
import './EnemyListPage.css';

export default function EnemyListPage() {
  const [enemies, setEnemies] = useState<Enemy[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    const fetchEnemies = async () => {
      setLoading(true);
      const response = await api.getAllEnemies();
      
      if (response.error) {
        setError(response.error);
      } else {
        setEnemies(response.data || []);
      }
      setLoading(false);
    };

    fetchEnemies();
  }, []);

  const filteredEnemies = enemies.filter(enemy =>
    enemy.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (loading) {
    return (
      <div className="container enemy-list-container">
        <h1>ENEMIES WIKI</h1>
        <div className="loading-state">
          <p>Loading enemies...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container enemy-list-container">
        <h1>ENEMIES WIKI</h1>
        <div className="error-state">
          <p>Error: {error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="enemy-list-page">
      <div className="container">
        <div className="page-header">
          <h1>ENEMIES WIKI</h1>
          <p className="page-subtitle">Explore the creatures of Nightmares</p>
        </div>

        <div className="search-bar">
          <input
            type="search"
            placeholder="Search enemies..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="search-input"
            aria-label="Search enemies"
          />
        </div>

        {filteredEnemies.length === 0 ? (
          <div className="empty-state">
            <p>
              {searchTerm
                ? `No enemies found matching "${searchTerm}"`
                : 'No enemies in the wiki yet.'}
            </p>
          </div>
        ) : (
          <>
            <div className="results-info">
              <p>Found {filteredEnemies.length} enemies</p>
            </div>
            <div className="enemies-grid">
              {filteredEnemies.map((enemy) => (
                <Card
                  key={enemy.id}
                  header={enemy.name}
                  footer={
                    <Link to={`/enemy/${enemy.id}`}>
                      <Button variant="primary" size="md">
                        View Details
                      </Button>
                    </Link>
                  }
                  className="enemy-card"
                >
                  <p className="enemy-description">{enemy.description}</p>

                  <div className="stats-grid">
                    <div className="stat-item">
                      <span className="stat-label">HP</span>
                      <span className="stat-value">{enemy.health}</span>
                    </div>
                    <div className="stat-item">
                      <span className="stat-label">ATK</span>
                      <span className="stat-value">{enemy.attack}</span>
                    </div>
                    <div className="stat-item">
                      <span className="stat-label">DEF</span>
                      <span className="stat-value">{enemy.defense}</span>
                    </div>
                    <div className="stat-item">
                      <span className="stat-label">EXP</span>
                      <span className="stat-value">{enemy.experience}</span>
                    </div>
                  </div>

                  {enemy.drops && enemy.drops.length > 0 && (
                    <div className="drops-section">
                      <h4>Drops</h4>
                      <ul className="drops-list">
                        {enemy.drops.map((drop, idx) => (
                          <li key={idx}>
                            <span className="drop-item">{drop.itemName}</span>
                            <Badge variant="info">
                              {Math.round(drop.dropRate * 100)}%
                            </Badge>
                            <span className="drop-qty">x{drop.quantity}</span>
                          </li>
                        ))}
                      </ul>
                    </div>
                  )}
                </Card>
              ))}
            </div>
          </>
        )}
      </div>
    </div>
  );
}

