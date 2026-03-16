# AI-Game-Project-1: Wiki Application

A full-stack wiki application for documenting game entities (enemies, items, NPCs) from the "Nightmares" game world. Built with .NET, React, and MongoDB.

## 🎮 Features

- **Browse Game Enemies**: View a complete wiki of enemies with stats and loot drops
- **Enemy Details**: See detailed information for each enemy including health, attack, defense, experience rewards, and loot tables
- **RESTful API**: Well-documented API for querying and managing enemy data
- **Responsive UI**: Modern web interface built with React and TypeScript
- **Type-Safe**: Full TypeScript on frontend, C# on backend for reliability

## 📋 Project Structure

```
AI-Game-Project-1/
├── Server/              # .NET 10 backend API
│   ├── Controllers/     # API endpoints
│   ├── Models/          # Data models
│   ├── Services/        # Business logic
│   └── Middleware/      # Request/response handling
│
├── Server.Tests/        # Backend unit tests (xUnit)
│
├── Client/              # React + Vite frontend
│   ├── src/
│   │   ├── pages/       # Page components
│   │   ├── models/      # TypeScript interfaces
│   │   ├── services/    # API client
│   │   └── App.tsx      # Main app component
│   └── package.json
│
├── API_SPECIFICATION.md # API documentation
├── ARCHITECTURE.md      # Architecture decisions
└── README.md           # This file
```

## 🚀 Getting Started

### Prerequisites

- **.NET 10 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Node.js 18+** - [Download](https://nodejs.org/)
- **MongoDB** - [Download](https://www.mongodb.com/try/download/community) or use [Docker](https://hub.docker.com/_/mongo)
- **Git** - [Download](https://git-scm.com/)

### Quick Start

#### 1. Start MongoDB

**Option A: Local Installation**
```bash
mongod --dbpath /path/to/data
```

**Option B: Docker**
```bash
docker run -d -p 27017:27017 --name nightmares-mongo mongo:latest
```

#### 2. Start Backend API

```bash
cd Server
dotnet restore
dotnet run
```

Backend will be available at `http://localhost:5000`

The API will automatically create the database and collections on first request.

#### 3. Start Frontend

```bash
cd Client
npm install
npm run dev
```

Frontend will be available at `http://localhost:5173`

### 4. Create Sample Data

Use the API to seed some enemies:

```bash
curl -X POST http://localhost:5000/api/enemies \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Goblin",
    "description": "A small green creature with big ears",
    "health": 10,
    "attack": 5,
    "defense": 2,
    "experience": 25,
    "drops": [
      {"itemName": "Gold", "dropRate": 0.8, "quantity": 5},
      {"itemName": "Goblin Dagger", "dropRate": 0.3, "quantity": 1}
    ]
  }'
```

Then visit `http://localhost:5173` to see the enemy in the wiki!

## 📚 API Documentation

Full API specification is available in [API_SPECIFICATION.md](./API_SPECIFICATION.md).

### Main Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/enemies` | List all enemies |
| GET | `/api/enemies/{id}` | Get specific enemy |
| POST | `/api/enemies` | Create new enemy |
| PUT | `/api/enemies/{id}` | Update enemy |
| DELETE | `/api/enemies/{id}` | Delete enemy |

## 🏗️ Architecture

See [ARCHITECTURE.md](./ARCHITECTURE.md) for detailed architecture decisions and design rationale.

### Tech Stack

**Backend:**
- .NET 10 with C#
- MongoDB for data storage
- xUnit for testing
- ASP.NET Core Web API

**Frontend:**
- React 18+ with TypeScript
- Vite for fast builds
- React Router for navigation
- Vitest for testing

**Database:**
- MongoDB 5.0+
- Local development support
- Cloud-ready (MongoDB Atlas compatible)

## 🧪 Testing

### Backend Tests
```bash
cd Server.Tests
dotnet test
```

### Frontend Tests
```bash
cd Client
npm test
```

## 📝 Development

### Backend Development

```bash
cd Server

# Watch mode with auto-rebuild
dotnet watch run

# Run specific tests
dotnet test --filter "TestName"

# Build for production
dotnet publish -c Release
```

### Frontend Development

```bash
cd Client

# Development server with HMR
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview
```

## 🔧 Configuration

### Backend Configuration

Edit `Server/appsettings.json`:

```json
{
  "MongoDB": {
    "DatabaseName": "nightmares-wiki"
  },
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  }
}
```

### Frontend Configuration

Edit `Client/.env.development`:

```
VITE_API_URL=http://localhost:5000/api
```

## 📦 Production Deployment

### Backend
```bash
cd Server
dotnet publish -c Release -o ./publish
# Upload publish folder to server and run: dotnet Server.dll
```

### Frontend
```bash
cd Client
npm run build
# Upload dist/ folder to static web server (nginx, Apache, or CDN)
```

### Environment Variables
Set these on production servers:

**Backend:**
- `MongoDB:DatabaseName`
- `ConnectionStrings:MongoDB` (use MongoDB Atlas URL)

**Frontend:**
- `VITE_API_URL` (production API URL)

## 🤝 Contributing

1. Create a feature branch: `git checkout -b feature/my-feature`
2. Make your changes and commit: `git commit -am 'Add feature'`
3. Push to the branch: `git push origin feature/my-feature`
4. Submit a pull request

### Code Style

**Backend (C#):**
- PascalCase for classes, methods, properties
- camelCase for local variables
- Follow Microsoft C# Coding Conventions

**Frontend (TypeScript/React):**
- PascalCase for components and types
- camelCase for functions and variables
- Use functional components with hooks
- Avoid `any` type - use explicit types

## 🐛 Troubleshooting

### MongoDB Connection Error
**Problem:** `Error: connect ECONNREFUSED 127.0.0.1:27017`

**Solution:** Make sure MongoDB is running:
```bash
# Check if mongod is running
ps aux | grep mongod

# Or start it
mongod --dbpath /path/to/data
```

### Port Already in Use
**Problem:** `Address already in use`

**Solution:** Change the port or kill the existing process:
```bash
# Backend (default 5000)
dotnet run -- --urls http://localhost:5001

# Frontend (default 5173)
npm run dev -- --host 127.0.0.1 --port 5174
```

### CORS Errors
**Problem:** Frontend can't reach backend API

**Solution:** Verify:
1. Backend is running on correct port (5000)
2. CORS is enabled in `Server/Program.cs`
3. Frontend `.env.development` has correct API URL
4. Check browser console for exact error

## 📖 Additional Resources

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [React Documentation](https://react.dev/)
- [MongoDB Documentation](https://docs.mongodb.com/)
- [Vite Documentation](https://vitejs.dev/)

## 📄 License

This project is licensed under the MIT License - see [LICENSE](./LICENSE) file for details.

## 🎯 Roadmap

- [x] Basic enemy CRUD API
- [x] Enemy wiki frontend
- [ ] User authentication & admin panel
- [ ] More entity types (Items, NPCs, Locations, Skills)
- [ ] Advanced search and filtering
- [ ] Image/media support
- [ ] Version history and audit trail
- [ ] Mobile app (React Native)

## 📞 Support

For issues, questions, or suggestions, please open an [issue](https://github.com/yourusername/AI-Game-Project-1/issues) on GitHub.
