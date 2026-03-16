import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import type { Enemy } from '../models/Enemy';
import api from '../services/api';
import { Card } from '../components/Card';
import { Button } from '../components/Button';
import { Badge } from '../components/Badge';
import './EnemyDetailPage.css';

export default function EnemyDetailPage() {
  const { id } = useParams<{ id: string }>();
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
    return (
      <div className="container">
        <div className="loading-state">
          <p>Loading enemy details...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container">
        <div className="error-state">
          <p>Error: {error}</p>
          <Link to="/">
            <Button variant="primary">Back to Enemies</Button>
          </Link>
        </div>
      </div>
    );
  }

  if (!enemy) {
    return (
      <div className="container">
        <div className="error-state">
          <p>Enemy not found</p>
          <Link to="/">
            <Button variant="primary">Back to Enemies</Button>
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="enemy-detail-page">
      <div className="container">
        <Link to="/">
          <Button variant="secondary" size="md">
            ← Back to Enemies
          </Button>
        </Link>

        <div className="detail-header">
          <h1>{enemy.name}</h1>
          <p className="enemy-description">{enemy.description}</p>
        </div>

        <div className="detail-grid">
          {/* Stats Section */}
          <Card header="Core Stats" className="stats-card">
            <div className="stat-block-grid">
              <div className="stat-display">
                <div className="stat-display-label">Health</div>
                <div className="stat-display-bar">
                  <div className="stat-bar-fill" style={{width: `${Math.min((enemy.health / 100) * 100, 100)}%`}}></div>
                  <span className="stat-display-value">{enemy.health}</span>
                </div>
              </div>

              <div className="stat-display">
                <div className="stat-display-label">Attack</div>
                <div className="stat-bar-fill" style={{width: `${Math.min((enemy.attack / 50) * 100, 100)}%`, backgroundColor: 'var(--color-accent-error)'}}></div>
                <span className="stat-display-value">{enemy.attack}</span>
              </div>

              <div className="stat-display">
                <div className="stat-display-label">Defense</div>
                <div className="stat-bar-fill" style={{width: `${Math.min((enemy.defense / 50) * 100, 100)}%`, backgroundColor: 'var(--color-accent-info)'}}></div>
                <span className="stat-display-value">{enemy.defense}</span>
              </div>

              <div className="stat-display">
                <div className="stat-display-label">Experience</div>
                <span className="stat-display-value">{enemy.experience} XP</span>
              </div>
            </div>
          </Card>

          {/* Drops Section */}
          {enemy.drops && enemy.drops.length > 0 && (
            <Card header="Loot Drops" className="drops-card">
              <div className="drops-container">
                {enemy.drops.map((drop, idx) => (
                  <div key={idx} className="drop-item-detail">
                    <div className="drop-info">
                      <span className="drop-name">{drop.itemName}</span>
                      <Badge variant="info">{Math.round(drop.dropRate * 100)}% chance</Badge>
                    </div>
                    <div className="drop-qty-detail">× {drop.quantity}</div>
                  </div>
                ))}
              </div>
            </Card>
          )}

          {/* Metadata Section */}
          {enemy.createdAt && (
            <Card header="Information" className="metadata-card">
              <div className="metadata-content">
                <div className="metadata-item">
                  <span className="metadata-label">Created:</span>
                  <span className="metadata-value">{new Date(enemy.createdAt).toLocaleDateString()}</span>
                </div>
                {enemy.updatedAt && (
                  <div className="metadata-item">
                    <span className="metadata-label">Updated:</span>
                    <span className="metadata-value">{new Date(enemy.updatedAt).toLocaleDateString()}</span>
                  </div>
                )}
              </div>
            </Card>
          )}
        </div>
      </div>
    </div>
  );
}
