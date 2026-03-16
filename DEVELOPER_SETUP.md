# Developer Setup Guide

This guide will help you set up the AI-Game-Project-1 development environment on your machine.

## Prerequisites

Before starting, ensure you have the following installed:

### Windows / macOS / Linux

1. **Git** (v2.30+)
   - [Download](https://git-scm.com/downloads)
   - Verify: `git --version`

2. **.NET 10 SDK**
   - [Download](https://dotnet.microsoft.com/download/dotnet/10.0)
   - Verify: `dotnet --version` (should show 10.x.x)

3. **Node.js** (v18+)
   - [Download](https://nodejs.org/)
   - Verify: `node --version` and `npm --version`

4. **MongoDB** (v5.0+)
   - Option A: [Local Installation](https://docs.mongodb.com/manual/installation/)
   - Option B: [MongoDB Atlas](https://www.mongodb.com/cloud/atlas) (cloud)
   - Option C: [Docker](https://hub.docker.com/_/mongo) (recommended for consistency)

5. **Code Editor** (recommended)
   - [Visual Studio Code](https://code.visualstudio.com/) with extensions:
     - C# Dev Kit (Microsoft)
     - ES7+ React/Redux/React-Native snippets
   - Or [Visual Studio 2022](https://visualstudio.microsoft.com/)

## Step 1: Clone the Repository

```bash
# Clone the repository
git clone https://github.com/yourusername/AI-Game-Project-1.git
cd AI-Game-Project-1

# Or if you already have the folder:
cd AI-Game-Project-1
git pull origin main
```

## Step 2: Set Up MongoDB

Choose one of the following options:

### Option A: Docker (Recommended for Development)

```bash
# Start MongoDB container
docker run -d \
  -p 27017:27017 \
  --name nightmares-mongo \
  -v mongo-data:/data/db \
  mongo:7.0

# Verify it's running
docker ps | grep nightmares-mongo

# To stop later:
docker stop nightmares-mongo

# To resume later:
docker start nightmares-mongo
```

### Option B: Local MongoDB Installation

```bash
# macOS (with Homebrew)
brew tap mongodb/brew
brew install mongodb-community
brew services start mongodb-community

# Windows (with chocolatey)
choco install mongodb

# Linux (Ubuntu)
sudo apt-get update
sudo apt-get install -y mongodb

# Start MongoDB
mongod --dbpath /path/to/data
```

### Option C: MongoDB Atlas (Cloud)

1. Go to [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
2. Create a free cluster
3. Get connection string
4. Update `Server/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "MongoDB": "mongodb+srv://username:password@cluster.mongodb.net/nightmares-wiki"
   }
   ```

### Verify MongoDB is Running

```bash
# Use MongoDB shell
mongosh  # or: mongo

# In the shell:
db.version()  # Should return version
exit
```

## Step 3: Set Up Backend (.NET)

```bash
cd Server

# Restore NuGet packages
dotnet restore

# Verify it builds
dotnet build

# Output should show "Build succeeded"
```

### Configure Backend (Optional)

Edit `Server/appsettings.json` if needed:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  },
  "MongoDB": {
    "DatabaseName": "nightmares-wiki"
  },
  "AllowedHosts": "*"
}
```

### Run Backend

```bash
# Development with auto-reload
dotnet watch run

# Or standard run:
dotnet run

# You should see:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: http://localhost:5000
```

### Verify Backend API

In another terminal:

```bash
# Test the API
curl http://localhost:5000/api/enemies

# Should return:
# {"data":[],"error":null}
```

### Run Backend Tests

```bash
cd Server.Tests

dotnet test

# Should show:
# Passed!  - Failed: 0, Passed: 4, Skipped: 0
```

## Step 4: Set Up Frontend (React)

```bash
cd Client

# Install dependencies
npm install

# Verify TypeScript configuration
npm run build

# Should output files to dist/
```

### Configure Frontend (Optional)

Edit `Client/.env.development`:

```
VITE_API_URL=http://localhost:5000/api
```

### Run Frontend

```bash
# Development server with hot reload
npm run dev

# You should see:
#   VITE v7.x.x  ready in XXX ms
#   ➜  Local:   http://localhost:5173/
#   ➜  press h + enter to show help
```

Visit `http://localhost:5173` in your browser to see the app!

### Run Frontend Tests

```bash
npm test

# Should pass all tests
```

## Step 5: Create Sample Data

With the backend running, add some enemies to the wiki:

```bash
# Create first enemy
curl -X POST http://localhost:5000/api/enemies \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Goblin",
    "description": "A small green creature",
    "health": 10,
    "attack": 5,
    "defense": 2,
    "experience": 25,
    "drops": [
      {"itemName": "Gold", "dropRate": 0.8, "quantity": 5}
    ]
  }'

# Create second enemy
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

Visit `http://localhost:5173` and you should see the enemies in the wiki!

## Troubleshooting

### MongoDB Connection Error

**Error:** `Error connecting to MongoDB: connect ECONNREFUSED 127.0.0.1:27017`

**Solutions:**
1. Check if MongoDB is running:
   ```bash
   # Docker
   docker ps | grep mongo
   
   # Local
   ps aux | grep mongod
   ```

2. Start MongoDB if not running:
   ```bash
   # Docker
   docker start nightmares-mongo
   
   # Local macOS
   brew services start mongodb-community
   ```

3. Check connection string in `appsettings.json`

### Port Already in Use

**Backend (port 5000):**
```bash
# Kill process using port 5000
# macOS/Linux:
lsof -ti :5000 | xargs kill -9

# Windows PowerShell:
Get-Process -Id (Get-NetTCPConnection -LocalPort 5000).OwningProcess | Stop-Process
```

**Frontend (port 5173):**
```bash
# Use different port
npm run dev -- --port 5174
```

### CORS Errors

**Error:** `Access to XMLHttpRequest has been blocked by CORS policy`

**Solution:** Verify backend is running and CORS is configured:
1. Backend running on `http://localhost:5000`
2. Check `Server/Program.cs` for CORS policy
3. Frontend `.env.development` has correct API URL

### .NET Build Fails

```bash
# Clean and rebuild
cd Server
dotnet clean
dotnet restore
dotnet build
```

### Node Modules Issues

```bash
# Clear cache and reinstall
cd Client
rm -rf node_modules package-lock.json
npm install
```

## Development Workflow

### Backend Development

```bash
# Terminal 1: Start MongoDB
docker start nightmares-mongo

# Terminal 2: Start backend with watch mode
cd Server
dotnet watch run

# Terminal 3: Run tests in watch mode
cd Server.Tests
dotnet watch test
```

### Frontend Development

```bash
# Terminal: Start frontend with HMR
cd Client
npm run dev

# In another terminal: Run tests in watch mode
cd Client
npm run test:watch
```

### Making Changes

**Backend:**
- Edit files in `Server/`
- Changes auto-reload with `dotnet watch run`
- Run tests with `dotnet test`

**Frontend:**
- Edit files in `Client/src/`
- Browser auto-refreshes with HMR
- Run tests with `npm test`

## Debugging

### Backend Debugging

**Using VS Code:**

1. Install extension: C# Dev Kit
2. Open `Server` folder
3. Create `.vscode/launch.json`:
   ```json
   {
     "version": "0.2.0",
     "configurations": [
       {
         "name": ".NET Core Launch",
         "type": "coreclr",
         "request": "launch",
         "preLaunchTask": "build",
         "program": "${workspaceFolder}/bin/Debug/net10.0/Server.dll",
         "args": [],
         "cwd": "${workspaceFolder}",
         "stopAtEntry": false,
         "console": "internalConsole"
       }
     ]
   }
   ```
4. Press F5 to debug

**Using Visual Studio 2022:**
- Open `Server.sln`
- Set breakpoints and press F5

### Frontend Debugging

**Using VS Code:**

1. Press F5 to open browser DevTools
2. Console tab shows errors
3. Network tab shows API calls

**Using Chrome DevTools:**

1. Open `http://localhost:5173`
2. Press F12 or right-click → Inspect
3. Use React Developer Tools extension

## Git Workflow

```bash
# Create feature branch
git checkout -b feature/my-feature

# Make changes, then:
git add .
git commit -m "feat: add my feature"

# Push to remote
git push origin feature/my-feature

# Create pull request on GitHub
```

## Environment Variables

### Backend

Create `Server/.env` (ignored by git):
```
MongoDB:DatabaseName=nightmares-wiki
ConnectionStrings:MongoDB=mongodb://localhost:27017
```

Or edit `appsettings.Development.json` directly.

### Frontend

Create `Client/.env.development`:
```
VITE_API_URL=http://localhost:5000/api
```

## Running on Different Ports

If default ports are taken:

```bash
# Backend on port 5001
cd Server && dotnet run -- --urls http://localhost:5001

# Frontend on port 5174
cd Client && npm run dev -- --port 5174
```

Then update `.env.development`:
```
VITE_API_URL=http://localhost:5001/api
```

## Next Steps

1. Read [API_SPECIFICATION.md](../API_SPECIFICATION.md) to understand the API
2. Read [ARCHITECTURE.md](../ARCHITECTURE.md) to understand design decisions
3. Explore the codebase:
   - Backend: `Server/Controllers/`, `Server/Services/`
   - Frontend: `Client/src/pages/`, `Client/src/services/`
4. Make a small change to verify everything works
5. Check out the [main README](../README.md) for more info

## Getting Help

- **API Issues**: Check [API_SPECIFICATION.md](../API_SPECIFICATION.md)
- **Architecture Questions**: See [ARCHITECTURE.md](../ARCHITECTURE.md)
- **MongoDB Docs**: https://docs.mongodb.com/
- **.NET Docs**: https://docs.microsoft.com/en-us/dotnet/
- **React Docs**: https://react.dev/

Happy coding! 🚀
