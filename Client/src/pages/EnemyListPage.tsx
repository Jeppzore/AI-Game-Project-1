import { useState, useEffect } from 'react';
import type { Enemy } from '../models/Enemy';
import api from '../services/api';
import './EnemyListPage.css';

export default function EnemyListPage() {
  const [enemies, setEnemies] = useState<Enemy[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

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

  if (loading) {
    return <div className="loading">Loading enemies...</div>;
  }

  if (error) {
    return <div className="error">Error: {error}</div>;
  }

  return (
    <div className="enemy-list-container">
      <h1>Game Enemies Wiki</h1>
      
      {enemies.length === 0 ? (
        <div className="no-enemies">No enemies found. The wiki is empty.</div>
      ) : (
        <div className="enemies-grid">
          {enemies.map((enemy) => (
            <div key={enemy.id} className="enemy-card">
              <h2>{enemy.name}</h2>
              <p className="description">{enemy.description}</p>
              
              <div className="stats">
                <div className="stat">
                  <span className="stat-label">Health:</span>
                  <span className="stat-value">{enemy.health}</span>
                </div>
                <div className="stat">
                  <span className="stat-label">Attack:</span>
                  <span className="stat-value">{enemy.attack}</span>
                </div>
                <div className="stat">
                  <span className="stat-label">Defense:</span>
                  <span className="stat-value">{enemy.defense}</span>
                </div>
                <div className="stat">
                  <span className="stat-label">Experience:</span>
                  <span className="stat-value">{enemy.experience}</span>
                </div>
              </div>

              {enemy.drops && enemy.drops.length > 0 && (
                <div className="drops">
                  <h3>Drops</h3>
                  <ul>
                    {enemy.drops.map((drop, idx) => (
                      <li key={idx}>
                        {drop.itemName} ({Math.round(drop.dropRate * 100)}%) x{drop.quantity}
                      </li>
                    ))}
                  </ul>
                </div>
              )}

              <a href={`/enemy/${enemy.id}`} className="view-details-btn">
                View Details
              </a>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
