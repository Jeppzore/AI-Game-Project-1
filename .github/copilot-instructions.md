# Copilot Instructions for AI-Game-Project-1

## Project Overview

AI-Game-Project-1 is a wiki-style reference application documenting game data (enemies, items, NPCs, etc.) from the private "Nightmares" repository. It consists of a full-stack application with:
- **Backend**: C#/.NET server with MongoDB for persistent storage
- **Frontend**: React/TypeScript client for web-based wiki interface
- **Architecture**: Separated frontend/backend with clear API boundary

## Tech Stack

- **Backend**: C# with .NET (Framework TBD - likely .NET 6+ or .NET Framework)
- **Frontend**: React with TypeScript
- **Database**: MongoDB (local instance for development)
- **Client**: TypeScript with React
- **Server**: C#/.NET with REST API endpoints

## Project Structure (Expected)

```
/
├── Server/          # .NET backend
├── Client/          # React frontend  
├── README.md
└── .github/
```

## Build & Run Commands

**Backend (.NET)**
- Restore dependencies: `dotnet restore`
- Build: `dotnet build`
- Run: `dotnet run` or `dotnet watch run` (for development with auto-reload)
- Run tests: `dotnet test`
- Run specific test: `dotnet test --filter "TestName"`

**Frontend (React/TypeScript)**
- Install dependencies: `npm install`
- Development server: `npm start` or `npm run dev`
- Build for production: `npm run build`
- Run tests: `npm test`
- Run specific test: `npm test -- TestFile.test.ts`
- Lint: `npm run lint` (if configured)

**MongoDB**
- Start local MongoDB: `mongod` (ensure MongoDB is installed locally)
- Connection string for development: `mongodb://localhost:27017/nightmares-wiki` (adjust DB name as needed)

## Key Architecture Patterns

### Backend (.NET)
- **API Structure**: REST endpoints mapping to game data entities (enemies, items, NPCs)
- **Data Models**: C# classes representing MongoDB documents (Enemies, Items, NPCs, etc.)
- **Database Access**: MongoDB driver for C# or Entity Framework with MongoDB provider
- **API Routes**: Likely RESTful patterns like `/api/enemies`, `/api/items`, `/api/npcs` with GET/POST/PUT/DELETE operations

### Frontend (React)
- **Component Structure**: Functional components with hooks (useState, useEffect, useContext)
- **State Management**: TBD - likely Context API or Redux, consider for centralized game data state
- **API Integration**: Fetch or Axios for HTTP calls to backend
- **Typing**: Full TypeScript implementation - all props and state should be properly typed

### Data Flow
1. User interacts with React frontend
2. Frontend makes API calls to .NET backend
3. Backend queries MongoDB for game data
4. Backend returns JSON responses
5. Frontend renders data in wiki-style pages

## Conventions

### C#/.NET
- **Naming**: PascalCase for classes, methods, properties; camelCase for local variables
- **Models**: Entity classes should match MongoDB document structure
- **API Responses**: Consistent JSON response format with data/error fields
- **Error Handling**: Use appropriate HTTP status codes (404 for not found, 400 for bad requests, 500 for server errors)

### React/TypeScript
- **File Organization**: Components in `src/components/`, pages in `src/pages/`, utilities in `src/utils/`
- **Naming**: PascalCase for components and types, camelCase for functions and variables
- **Types**: Avoid `any` - use explicit interfaces or types for all component props
- **Hooks**: Use functional components with hooks exclusively
- **API Calls**: Centralize API client in dedicated service files (e.g., `src/services/api.ts`)

### MongoDB Documents
- **Collection Names**: Plural snake_case (e.g., `enemies`, `items`, `npcs`)
- **Field Names**: camelCase to match C# property conventions
- **IDs**: Use MongoDB ObjectId or string IDs consistently

## Development Workflow

1. **Ensure MongoDB is running locally** before starting development
2. **Backend first**: Set up API endpoints before frontend consumption
3. **Type safety**: Keep TypeScript strict mode enabled in `tsconfig.json`
4. **API Contract**: Define clear request/response schemas for backend APIs
5. **Testing**: Write tests alongside implementation for both backend and frontend

## Important Notes

- This is a reference wiki for the "Nightmares" game world - prioritize data accuracy and completeness
- MongoDB should be configured for local development; consider Docker for consistency
- Frontend and backend should be independently deployable
- Environment variables (DB connection strings, API URLs) should be externalized in `.env` files

## Common Tasks

**Adding a new game entity (e.g., Boss type)**:
1. Create C# model in backend
2. Create MongoDB collection if needed
3. Add API endpoints (GET all, GET by ID, POST, PUT, DELETE)
4. Add TypeScript interface in frontend matching the API schema
5. Create React component to display the entity
6. Add API service method to frontend for communication

**Debugging**:
- **Backend**: Use Visual Studio debugger or attach to `dotnet run` process
- **Frontend**: Use browser DevTools or VS Code debugger with `npm start`
- **Database**: Use MongoDB Compass or mongo shell to inspect collections

## MCP Servers (Optional)

Consider configuring these MCP servers to enhance development experience:

- **MongoDB MCP Server**: Useful for testing queries and inspecting collections directly during development
- **Filesystem MCP Server**: Helps with file operations and project structure exploration
- **Node.js/NPM MCP Server**: Useful for frontend dependency and script management
- **.NET CLI MCP Server**: Helpful for backend build and test operations

To configure MCP servers, see the main README or GitHub Actions workflows for setup instructions.

## Next Steps for Setup

1. Create `/Server` directory with .NET project structure
2. Create `/Client` directory with React TypeScript setup (npx create-react-app or Vite)
3. Configure MongoDB connection string in backend
4. Define initial game data models (Enemy, Item, NPC, etc.)
5. Implement core API endpoints
6. Build wiki frontend pages to display data
