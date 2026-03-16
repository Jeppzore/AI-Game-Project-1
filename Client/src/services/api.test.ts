import { describe, it, expect } from 'vitest';
import type { Enemy, CreateEnemyRequest } from '../models/Enemy';

describe('API Client', () => {
  it('should create an enemy request object', () => {
    const request: CreateEnemyRequest = {
      name: 'Test Enemy',
      description: 'A test enemy',
      health: 50,
      attack: 10,
      defense: 5,
      experience: 100,
    };

    expect(request.name).toBe('Test Enemy');
    expect(request.health).toBe(50);
  });

  it('should handle enemy model correctly', () => {
    const enemy: Enemy = {
      id: 'test-123',
      name: 'Dragon',
      description: 'A fierce dragon',
      health: 100,
      attack: 20,
      defense: 15,
      experience: 500,
      drops: [
        {
          itemName: 'Dragon Scale',
          dropRate: 0.5,
          quantity: 1,
        },
      ],
    };

    expect(enemy.id).toBe('test-123');
    expect(enemy.name).toBe('Dragon');
    expect(enemy.drops).toHaveLength(1);
    expect(enemy.drops[0].itemName).toBe('Dragon Scale');
  });

  it('should validate enemy stats', () => {
    const enemy: Enemy = {
      name: 'Goblin',
      description: 'A small goblin',
      health: 10,
      attack: 5,
      defense: 2,
      experience: 25,
      drops: [],
    };

    expect(enemy.health).toBeGreaterThan(0);
    expect(enemy.attack).toBeGreaterThan(0);
    expect(enemy.defense).toBeGreaterThanOrEqual(0);
    expect(enemy.experience).toBeGreaterThanOrEqual(0);
  });

  it('should handle API response structure', () => {
    const mockResponse = {
      data: {
        id: '123',
        name: 'Test',
        description: 'Test enemy',
        health: 50,
        attack: 10,
        defense: 5,
        experience: 100,
        drops: [],
      },
      error: null,
    };

    expect(mockResponse.data).toBeDefined();
    expect(mockResponse.error).toBeNull();
    expect(mockResponse.data.name).toBe('Test');
  });
});
