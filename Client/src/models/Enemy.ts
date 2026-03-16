export interface LootDrop {
  itemName: string;
  dropRate: number;
  quantity: number;
}

export interface Enemy {
  id?: string;
  name: string;
  description: string;
  health: number;
  attack: number;
  defense: number;
  experience: number;
  drops: LootDrop[];
  createdAt?: Date;
  updatedAt?: Date;
}

export interface CreateEnemyRequest {
  name: string;
  description: string;
  health: number;
  attack: number;
  defense: number;
  experience: number;
  drops?: LootDrop[];
}

export interface UpdateEnemyRequest {
  name?: string;
  description?: string;
  health?: number;
  attack?: number;
  defense?: number;
  experience?: number;
  drops?: LootDrop[];
}

export interface ApiResponse<T> {
  data?: T;
  error?: string;
}
