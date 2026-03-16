import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import type { Enemy } from '../models/Enemy';
import api from '../services/api';
import './EnemyDetailPage.css';

export default function EnemyDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [enemy, setEnemy] = useState<Enemy | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) {
      setError('Enemy ID is missing');
      setLoading(false);
      return;
    }

    const fetchEnemy = async () => {
      setLoading(true);
      const response = await api.getEnemyById(id);
      
      if (response.error) {
        setError(response.error);
      } else if (response.data) {
        setEnemy(response.data);
      }
      setLoading(false);
    };

    fetchEnemy();
  }, [id]);

  if (loading) {
    return <div className="loading">Loading enemy details...</div>;
  }

  if (error) {
    return (
      <div className="error-container">
        <div className="error">Error: {error}</div>
        <button onClick={() => navigate('/')}>Back to List</button>
      </div>
    );
  }

  if (!enemy) {
    return (
      <div className="error-container">
        <div className="error">Enemy not found</div>
        <button onClick={() => navigate('/')}>Back to List</button>
      </div>
    );
  }

  return (
    <div className="enemy-detail-container">
      <button className="back-btn" onClick={() => navigate('/')}>← Back to Enemies</button>
      
      <div className="enemy-detail">
        <h1>{enemy.name}</h1>
        <p className="description">{enemy.description}</p>

        <div className="stats-section">
          <h2>Stats</h2>
          <div className="stats-grid">
            <div className="stat-block">
              <span className="stat-label">Health</span>
              <span className="stat-number">{enemy.health}</span>
            </div>
            <div className="stat-block">
              <span className="stat-label">Attack</span>
              <span className="stat-number">{enemy.attack}</span>
            </div>
            <div className="stat-block">
              <span className="stat-label">Defense</span>
              <span className="stat-number">{enemy.defense}</span>
            </div>
            <div className="stat-block">
              <span className="stat-label">Experience Reward</span>
              <span className="stat-number">{enemy.experience}</span>
            </div>
          </div>
        </div>

        {enemy.drops && enemy.drops.length > 0 && (
          <div className="drops-section">
            <h2>Loot Drops</h2>
            <table className="drops-table">
              <thead>
                <tr>
                  <th>Item</th>
                  <th>Drop Rate</th>
                  <th>Quantity</th>
                </tr>
              </thead>
              <tbody>
                {enemy.drops.map((drop, idx) => (
                  <tr key={idx}>
                    <td>{drop.itemName}</td>
                    <td>{Math.round(drop.dropRate * 100)}%</td>
                    <td>{drop.quantity}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {enemy.createdAt && (
          <div className="metadata">
            <p>Created: {new Date(enemy.createdAt).toLocaleDateString()}</p>
            {enemy.updatedAt && (
              <p>Updated: {new Date(enemy.updatedAt).toLocaleDateString()}</p>
            )}
          </div>
        )}
      </div>
    </div>
  );
}
