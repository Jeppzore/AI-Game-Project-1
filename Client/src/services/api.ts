import type { Enemy, CreateEnemyRequest, UpdateEnemyRequest, ApiResponse } from '../models/Enemy';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

class ApiClient {
  private baseUrl: string;

  constructor(baseUrl: string = API_BASE_URL) {
    this.baseUrl = baseUrl;
  }

  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<ApiResponse<T>> {
    const url = `${this.baseUrl}${endpoint}`;
    
    try {
      const response = await fetch(url, {
        ...options,
        headers: {
          'Content-Type': 'application/json',
          ...options.headers,
        },
      });

      if (!response.ok) {
        const errorData = await response.json();
        return {
          error: errorData.error || `HTTP Error: ${response.status}`,
        };
      }

      const data = await response.json();
      return data;
    } catch (error) {
      return {
        error: error instanceof Error ? error.message : 'An unknown error occurred',
      };
    }
  }

  async getAllEnemies(): Promise<ApiResponse<Enemy[]>> {
    return this.request<Enemy[]>('/enemies');
  }

  async getEnemyById(id: string): Promise<ApiResponse<Enemy>> {
    return this.request<Enemy>(`/enemies/${id}`);
  }

  async createEnemy(request: CreateEnemyRequest): Promise<ApiResponse<Enemy>> {
    return this.request<Enemy>('/enemies', {
      method: 'POST',
      body: JSON.stringify(request),
    });
  }

  async updateEnemy(id: string, request: UpdateEnemyRequest): Promise<ApiResponse<Enemy>> {
    return this.request<Enemy>(`/enemies/${id}`, {
      method: 'PUT',
      body: JSON.stringify(request),
    });
  }

  async deleteEnemy(id: string): Promise<ApiResponse<{ message: string }>> {
    return this.request<{ message: string }>(`/enemies/${id}`, {
      method: 'DELETE',
    });
  }
}

export default new ApiClient();
