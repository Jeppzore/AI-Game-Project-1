# Architecture Decisions

## Overview
This document records key architectural decisions made for the AI-Game-Project-1 wiki application.

---

## 1. **Full-Stack Separation (Backend/Frontend)**

**Decision:** Implement separate, independently deployable backend (.NET) and frontend (React) applications.

**Rationale:**
- Allows independent scaling of backend API and frontend UI
- Enables teams to work on frontend and backend in parallel
- Simplifies deployment and CI/CD pipelines
- Clear API contract between frontend and backend

**Implications:**
- CORS must be configured on backend
- Frontend must know API URL (via environment variables)
- API-first design ensures consistency

---

## 2. **Technology Stack Selection**

### Backend: .NET 10 with C#
**Why .NET?**
- Strong type system with C#
- Excellent async/await support for high-concurrency APIs
- Built-in dependency injection
- Entity Framework or MongoDB driver for data access
- Great tooling and IDE support

### Database: MongoDB
**Why MongoDB?**
- Document-based storage fits game entity structure (flexible schema)
- Schemaless nature allows easy addition of new entity types
- JSON-compatible data format simplifies API responses
- Excellent for rapid prototyping and iteration

### Frontend: React with Vite and TypeScript
**Why React?**
- Component-based architecture for wiki pages
- Large ecosystem and community support
- Easy to add features incrementally

**Why Vite?**
- Faster build times than Create React App
- Modern ES module-based development
- Better developer experience with instant HMR

**Why TypeScript?**
- Type safety across the frontend
- Better IDE support and refactoring
- Catches errors at compile time
- Strict mode enforced for high code quality

---

## 3. **API Design: RESTful Architecture**

**Decision:** Use REST principles with consistent response format.

**Standard Response Format:**
```json
{
  "data": { /* resource data */ },
  "error": null  // error message on failure
}
```

**Rationale:**
- Familiar to most developers
- Easy to test with standard HTTP tools (curl, Postman)
- Clear separation of concerns between endpoints
- Stateless design simplifies scaling

**Endpoints Follow Pattern:**
- `GET /api/enemies` - List all
- `GET /api/enemies/{id}` - Get one
- `POST /api/enemies` - Create
- `PUT /api/enemies/{id}` - Update
- `DELETE /api/enemies/{id}` - Delete

---

## 4. **Authentication & Authorization**

**Decision:** No authentication required for MVP (read-only public wiki).

**Rationale:**
- MVP focuses on basic functionality
- Wiki-style content (game reference data) is publicly available
- Easier to get started without auth complexity
- Can be added later as a separate phase

**Future Enhancement:**
- Add JWT-based auth for admin editing
- Implement role-based access control (Admin, Editor, Viewer)

---

## 5. **State Management: React Context API**

**Decision:** Use Context API for frontend state (no Redux initially).

**Rationale:**
- Context API is built-in, no external dependency
- Sufficient for MVP with limited state complexity
- Reduced bundle size
- Easier learning curve for new developers

**Migration Path:**
- If state complexity grows, migrate to Redux or Zustand
- Current architecture supports this migration

---

## 6. **Data Modeling: Flat Document Structure**

**Decision:** Store related data (e.g., loot drops) as embedded documents in MongoDB.

**Example:**
```json
{
  "_id": ObjectId("..."),
  "name": "Dragon",
  "drops": [
    {"itemName": "Dragon Scale", "dropRate": 0.5, "quantity": 1}
  ]
}
```

**Rationale:**
- Reduces number of database queries
- Keeps related data together (DDD principle)
- Easier to serialize to API responses
- Avoids complex joins

**Trade-off:**
- Not normalized like relational databases
- Works well for game data with predictable structure

---

## 7. **Error Handling Strategy**

**Backend:**
- Global exception middleware catches all errors
- Consistent HTTP status codes (400, 404, 500)
- Structured error responses
- Logging of unexpected errors

**Frontend:**
- Try/catch in API service
- User-friendly error messages displayed in UI
- Fallback UI states (loading, error, empty)

---

## 8. **Dependency Injection (Backend)**

**Decision:** Use ASP.NET's built-in Dependency Injection container.

**Benefits:**
- Clean separation of concerns
- Testable code with mock dependencies
- No external library needed
- Standardized approach in .NET

**Example:**
```csharp
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();
```

---

## 9. **Development Environment Setup**

**MongoDB:**
- Runs locally on `localhost:27017`
- Database name: `nightmares-wiki`
- Can be replaced with Docker container for consistency

**Backend:**
- Runs on `http://localhost:5000`
- Environment-based configuration via appsettings.json

**Frontend:**
- Runs on `http://localhost:5173` (Vite default)
- API URL configurable via `.env.development`

---

## 10. **Testing Strategy**

**Backend:**
- xUnit for unit tests
- Moq for mocking dependencies
- Tests focus on business logic and API contracts

**Frontend:**
- Vitest for unit tests
- React Testing Library for component tests
- Tests focus on user interactions and data flow

---

## 11. **Versioning & Deployment**

**API Versioning:**
- Not implemented in MVP (single version)
- If needed, use URL path versioning: `/api/v1/enemies`

**Database Migrations:**
- MongoDB is schemaless, no migrations needed
- Add validation in application layer

---

## 12. **Security Considerations**

**Current Implementation (MVP):**
- CORS configured to allow localhost:3000 and localhost:5173
- No sensitive data transmitted (public wiki)
- HTTP used in development, HTTPS required in production

**Future Enhancements:**
- Add rate limiting
- Input validation and sanitization
- SQL injection protection (N/A for MongoDB, but consider NoSQL injection)
- CSRF protection when authentication is added
- HTTPS enforcement in production

---

## 13. **Scalability Considerations**

**Database:**
- MongoDB collection indexes on frequently queried fields (e.g., enemy name)
- Connection pooling enabled
- Can migrate to MongoDB Atlas for cloud deployment

**Backend:**
- Stateless API design allows horizontal scaling
- Can run multiple instances behind load balancer
- Dependency injection simplifies testing and swapping implementations

**Frontend:**
- Static build artifacts can be served from CDN
- Lazy loading of pages as app grows

---

## 14. **Documentation First**

**Decision:** API specification and architecture decisions documented upfront.

**Benefits:**
- Clear contract between frontend and backend teams
- Easier onboarding for new developers
- Reduces integration issues
- Provides reference for future changes

---

## Summary of Key Tradeoffs

| Decision | Pro | Con |
|----------|-----|-----|
| REST API | Simple, familiar, easy to test | Can be verbose in some cases |
| MongoDB | Flexible schema, JSON-friendly | Not ideal for complex relationships |
| React | Large ecosystem, component reusability | More learning curve than static HTML |
| No Auth (MVP) | Faster to market, simpler code | Can't restrict edits, no user tracking |
| Context API | Lightweight, no dependency | Limited for complex state |

---

## Future Architecture Improvements

1. **Add WebSocket Support** for real-time updates
2. **Implement Caching Layer** (Redis) for frequently accessed enemies
3. **Add GraphQL** as alternative to REST API
4. **Database Sharding** for million+ enemies
5. **Microservices** if system grows (separate services for items, NPCs, etc.)
6. **API Gateway** for routing and auth
7. **Search Engine** (Elasticsearch) for advanced filtering

---

## References

- [REST API Best Practices](https://restfulapi.net/)
- [MongoDB Data Modeling](https://docs.mongodb.com/manual/core/data-modeling-introduction/)
- [React Best Practices](https://react.dev/)
- [ASP.NET Core Architecture](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)
