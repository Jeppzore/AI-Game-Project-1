# API Specification

## Base URL
```
http://localhost:5000/api
```

## Authentication
The API is currently read-only public. No authentication is required.

## Response Format
All API responses follow a standard format:

```json
{
  "data": { /* response data */ },
  "error": null
}
```

Or on error:

```json
{
  "data": null,
  "error": "Error message describing what went wrong"
}
```

## Endpoints

### GET /enemies
Retrieve all enemies in the wiki.

**Parameters:** None

**Response:** 
- Status: `200 OK`
- Body: `ApiResponse<Enemy[]>`

**Example:**
```bash
curl http://localhost:5000/api/enemies
```

**Response:**
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439011",
      "name": "Goblin",
      "description": "A small green creature",
      "health": 10,
      "attack": 5,
      "defense": 2,
      "experience": 25,
      "drops": [
        {
          "itemName": "Gold",
          "dropRate": 0.8,
          "quantity": 10
        }
      ],
      "createdAt": "2026-03-11T07:30:00Z",
      "updatedAt": "2026-03-11T07:30:00Z"
    }
  ],
  "error": null
}
```

---

### GET /enemies/{id}
Retrieve a specific enemy by ID.

**Parameters:**
- `id` (path, required): MongoDB ObjectId of the enemy

**Response:**
- Status: `200 OK` or `404 Not Found`
- Body: `ApiResponse<Enemy>`

**Example:**
```bash
curl http://localhost:5000/api/enemies/507f1f77bcf86cd799439011
```

---

### POST /enemies
Create a new enemy in the wiki.

**Body:** `CreateEnemyRequest`
```json
{
  "name": "Dragon",
  "description": "A powerful dragon",
  "health": 100,
  "attack": 20,
  "defense": 15,
  "experience": 500,
  "drops": [
    {
      "itemName": "Dragon Scale",
      "dropRate": 0.5,
      "quantity": 1
    }
  ]
}
```

**Response:**
- Status: `201 Created`
- Body: `ApiResponse<Enemy>` (includes the created enemy with ID)

**Example:**
```bash
curl -X POST http://localhost:5000/api/enemies \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Dragon",
    "description": "A powerful dragon",
    "health": 100,
    "attack": 20,
    "defense": 15,
    "experience": 500,
    "drops": []
  }'
```

---

### PUT /enemies/{id}
Update an existing enemy.

**Parameters:**
- `id` (path, required): MongoDB ObjectId of the enemy

**Body:** `UpdateEnemyRequest` (all fields optional)
```json
{
  "name": "Dragon Lord",
  "description": "An even more powerful dragon",
  "health": 150
}
```

**Response:**
- Status: `200 OK` or `404 Not Found`
- Body: `ApiResponse<Enemy>` (includes updated enemy)

**Example:**
```bash
curl -X PUT http://localhost:5000/api/enemies/507f1f77bcf86cd799439011 \
  -H "Content-Type: application/json" \
  -d '{"name": "Dragon Lord"}'
```

---

### DELETE /enemies/{id}
Delete an enemy from the wiki.

**Parameters:**
- `id` (path, required): MongoDB ObjectId of the enemy

**Response:**
- Status: `200 OK` or `404 Not Found`
- Body: `ApiResponse<{ message: string }>`

**Example:**
```bash
curl -X DELETE http://localhost:5000/api/enemies/507f1f77bcf86cd799439011
```

---

## Data Models

### Enemy
Represents a game enemy in the wiki.

```typescript
interface Enemy {
  id?: string;                    // MongoDB ObjectId (optional on POST)
  name: string;                   // Enemy name (required)
  description: string;            // Enemy description (required)
  health: number;                 // Health points (required)
  attack: number;                 // Attack power (required)
  defense: number;                // Defense rating (required)
  experience: number;             // XP reward for defeating (required)
  drops: LootDrop[];             // Array of loot drops (optional)
  createdAt?: Date;              // Creation timestamp (server-generated)
  updatedAt?: Date;              // Last update timestamp (server-generated)
}
```

### LootDrop
Represents an item that an enemy may drop.

```typescript
interface LootDrop {
  itemName: string;              // Name of the item (required)
  dropRate: number;              // Probability 0.0-1.0 (required)
  quantity: number;              // How many drop (required)
}
```

### ApiResponse<T>
Generic response wrapper for all endpoints.

```typescript
interface ApiResponse<T> {
  data?: T;                       // Response data (null on error)
  error?: string;                 // Error message (null on success)
}
```

---

## HTTP Status Codes

| Status | Meaning |
|--------|---------|
| 200    | Success - operation completed |
| 201    | Created - resource successfully created |
| 400    | Bad Request - invalid input data |
| 404    | Not Found - resource doesn't exist |
| 500    | Server Error - internal server error |

---

## Error Handling

The API returns consistent error responses:

```json
{
  "data": null,
  "error": "Enemy not found"
}
```

Common error messages:
- `"Enemy not found"` - Resource doesn't exist
- `"Invalid request data"` - Validation failed
- `"An error occurred while [operation]"` - Unexpected server error

---

## Example Workflow

### 1. Create an enemy
```bash
curl -X POST http://localhost:5000/api/enemies \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Orc",
    "description": "A fierce warrior",
    "health": 30,
    "attack": 12,
    "defense": 5,
    "experience": 100,
    "drops": [
      {"itemName": "Orc Axe", "dropRate": 0.3, "quantity": 1},
      {"itemName": "Gold", "dropRate": 0.9, "quantity": 50}
    ]
  }'
```

### 2. Get all enemies
```bash
curl http://localhost:5000/api/enemies
```

### 3. Get specific enemy
```bash
curl http://localhost:5000/api/enemies/507f1f77bcf86cd799439011
```

### 4. Update enemy
```bash
curl -X PUT http://localhost:5000/api/enemies/507f1f77bcf86cd799439011 \
  -H "Content-Type: application/json" \
  -d '{"health": 35}'
```

### 5. Delete enemy
```bash
curl -X DELETE http://localhost:5000/api/enemies/507f1f77bcf86cd799439011
```
