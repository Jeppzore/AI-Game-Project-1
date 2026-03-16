# End-to-End Testing Guide

This document describes how to perform end-to-end testing of the AI-Game-Project-1 wiki application.

## Overview

End-to-end testing validates that the entire system works correctly from user interaction through the backend API to the database.

## Prerequisites

- Backend API running on `http://localhost:5000`
- Frontend running on `http://localhost:5173` or `http://localhost:3000` (Docker)
- MongoDB running on `localhost:27017`
- All previous setup steps completed

## Test Scenarios

### Test 1: Create Enemy via API

**Objective:** Verify backend can create an enemy in MongoDB

**Steps:**

1. Open terminal
2. Create an enemy using curl:
   ```bash
   curl -X POST http://localhost:5000/api/enemies \
     -H "Content-Type: application/json" \
     -d '{
       "name": "Test Goblin",
       "description": "A test goblin for E2E testing",
       "health": 15,
       "attack": 6,
       "defense": 3,
       "experience": 30,
       "drops": [
         {"itemName": "Test Gold", "dropRate": 0.7, "quantity": 10}
       ]
     }'
   ```

3. **Expected Result:**
   - Status: `201 Created`
   - Response contains enemy object with generated `id`
   - `error` field is null

**Verification:**
```bash
# Save the returned ID and use it in next tests
# Example ID: 507f1f77bcf86cd799439011
```

---

### Test 2: Retrieve Enemy from API

**Objective:** Verify backend can retrieve enemy from MongoDB

**Steps:**

1. Use the ID from Test 1:
   ```bash
   curl http://localhost:5000/api/enemies/507f1f77bcf86cd799439011
   ```

2. **Expected Result:**
   - Status: `200 OK`
   - Response contains the exact enemy created in Test 1
   - All fields match (name, health, attack, etc.)

---

### Test 3: List All Enemies

**Objective:** Verify backend returns all enemies

**Steps:**

1. Get all enemies:
   ```bash
   curl http://localhost:5000/api/enemies
   ```

2. **Expected Result:**
   - Status: `200 OK`
   - Response is array with at least 1 enemy (from Test 1)
   - Enemy from Test 1 is in the list

---

### Test 4: Update Enemy via API

**Objective:** Verify backend can update enemy

**Steps:**

1. Update the enemy from Test 1:
   ```bash
   curl -X PUT http://localhost:5000/api/enemies/507f1f77bcf86cd799439011 \
     -H "Content-Type: application/json" \
     -d '{
       "health": 25,
       "name": "Updated Test Goblin"
     }'
   ```

2. **Expected Result:**
   - Status: `200 OK`
   - Response shows updated enemy
   - `health` = 25
   - `name` = "Updated Test Goblin"

---

### Test 5: View Enemy List in Frontend

**Objective:** Verify frontend displays enemies from API

**Steps:**

1. Open browser to `http://localhost:5173` (dev) or `http://localhost:3000` (Docker)
2. Should see "Game Enemies Wiki" heading
3. Should see at least one enemy card with:
   - Name (from Test 1)
   - Description
   - Stats (Health, Attack, Defense, Experience)
   - Loot drops (if any)

**Verification:**
- Page loads without errors
- Enemies display in grid layout
- Card styling is visible
- No console errors (F12 → Console tab)

---

### Test 6: Navigate to Enemy Detail Page

**Objective:** Verify frontend can display single enemy details

**Steps:**

1. On the enemy list page, click "View Details" button
2. Should navigate to `/enemy/{id}`
3. Page should show:
   - Back button
   - Enemy name
   - Description
   - All stats in styled blocks
   - Loot drops in table format (if any)
   - Created/Updated timestamps

**Verification:**
- Page loads successfully
- All data matches API response
- Navigation works (can click back button)

---

### Test 7: Error Handling - Nonexistent Enemy

**Objective:** Verify proper error handling

**Steps:**

1. Try to get nonexistent enemy:
   ```bash
   curl http://localhost:5000/api/enemies/000000000000000000000000
   ```

2. **Expected Result:**
   - Status: `404 Not Found`
   - Response: `{"data":null,"error":"Enemy not found"}`

3. In frontend, try to navigate to nonexistent enemy:
   - Go to `http://localhost:5173/enemy/invalid-id`
   - Should show error message
   - Back button should work

---

### Test 8: Delete Enemy via API

**Objective:** Verify enemy deletion

**Steps:**

1. Create another test enemy (use Test 1 process)
2. Delete it:
   ```bash
   curl -X DELETE http://localhost:5000/api/enemies/{id}
   ```

3. **Expected Result:**
   - Status: `200 OK`
   - Response: `{"data":{"message":"Enemy deleted successfully"},"error":null}`

4. Verify it's gone:
   ```bash
   curl http://localhost:5000/api/enemies/{id}
   ```
   - Should return 404

---

### Test 9: Create Enemy with Complex Drops

**Objective:** Verify handling of multiple loot items

**Steps:**

1. Create enemy with multiple drops:
   ```bash
   curl -X POST http://localhost:5000/api/enemies \
     -H "Content-Type: application/json" \
     -d '{
       "name": "Dragon Boss",
       "description": "A mighty dragon with many treasures",
       "health": 200,
       "attack": 40,
       "defense": 30,
       "experience": 2000,
       "drops": [
         {"itemName": "Dragon Scale", "dropRate": 0.8, "quantity": 3},
         {"itemName": "Dragon Egg", "dropRate": 0.05, "quantity": 1},
         {"itemName": "Gold", "dropRate": 1.0, "quantity": 500},
         {"itemName": "Ancient Artifact", "dropRate": 0.01, "quantity": 1}
       ]
     }'
   ```

2. Verify in API:
   - All drops present
   - Drop rates and quantities preserved

3. Verify in Frontend:
   - Open detail page
   - All drops show in table
   - Drop rates show as percentages
   - Quantities display correctly

---

### Test 10: Performance - List with Multiple Enemies

**Objective:** Verify performance with larger dataset

**Steps:**

1. Create 10-20 enemies using Test 1 process

2. Get all enemies:
   ```bash
   curl http://localhost:5000/api/enemies
   ```
   - Should complete within reasonable time
   - Response size should be manageable

3. In Frontend:
   - List page should load smoothly
   - All enemies visible
   - Grid layout should be responsive
   - No UI lag when scrolling

---

## Automated Test Run

### Run All Backend Tests

```bash
cd Server.Tests
dotnet test --verbosity normal

# Expected: All 4+ tests pass
# Passed!  - Failed: 0, Passed: 4+, Skipped: 0
```

### Run All Frontend Tests

```bash
cd Client
npm test

# Expected: All tests pass
```

---

## Manual Verification Checklist

### Backend

- [ ] `dotnet build` succeeds with 0 errors
- [ ] `dotnet run` starts without errors
- [ ] GET `/api/enemies` returns 200
- [ ] POST `/api/enemies` with valid data returns 201
- [ ] POST `/api/enemies` with invalid data returns 400
- [ ] GET `/api/enemies/{nonexistent}` returns 404
- [ ] PUT `/api/enemies/{id}` returns 200
- [ ] DELETE `/api/enemies/{id}` returns 200
- [ ] All unit tests pass

### Frontend

- [ ] `npm run build` succeeds with 0 errors
- [ ] `npm run dev` starts without errors
- [ ] Home page loads at `http://localhost:5173`
- [ ] Enemy list displays if enemies exist
- [ ] "No enemies" message shows if list empty
- [ ] Clicking "View Details" navigates to detail page
- [ ] Detail page shows correct enemy data
- [ ] Back button returns to list
- [ ] No console errors (F12 → Console)
- [ ] All tests pass (`npm test`)

### Integration

- [ ] Frontend can communicate with backend
- [ ] Creating enemy in API appears in frontend
- [ ] Deleting enemy in API removes from frontend
- [ ] CORS headers present (check Network tab in DevTools)
- [ ] Database connects successfully
- [ ] Timestamps are valid ISO format

---

## Performance Benchmarks

Target metrics for MVP:

| Metric | Target | How to Measure |
|--------|--------|---|
| API Response Time (GET list) | < 100ms | `time curl ...` |
| API Response Time (GET single) | < 50ms | `time curl ...` |
| Frontend Page Load | < 2s | Browser DevTools → Performance |
| Frontend FCP | < 1s | DevTools → Lighthouse |
| Bundle Size | < 250KB gzip | `npm run build` output |

---

## Docker E2E Testing

If running with Docker Compose:

```bash
# Start all services
docker-compose up -d

# Verify all containers are running
docker-compose ps

# Test backend
curl http://localhost:5000/api/enemies

# Open frontend
# http://localhost:3000

# View logs
docker-compose logs -f api

# Stop all
docker-compose down
```

---

## Troubleshooting E2E Issues

### Frontend Can't Connect to Backend

**Error:** CORS error in console

**Solution:**
1. Verify backend is running: `curl http://localhost:5000/api/enemies`
2. Check `.env.development` has correct API URL
3. Verify CORS policy in `Server/Program.cs`

### Stale Data in Frontend

**Solution:**
1. Hard refresh: `Ctrl+Shift+R` (Windows) or `Cmd+Shift+R` (Mac)
2. Clear browser cache
3. Clear IndexedDB if using (DevTools → Application)

### Database Locked

**Error:** MongoDB connection fails

**Solution:**
1. Check MongoDB is running: `docker ps` or `ps aux | grep mongod`
2. Clear database and restart: `docker-compose down -v`

### Port Conflicts

**Solution:** Kill process using port:
```bash
# macOS/Linux
lsof -ti :5000 | xargs kill -9
lsof -ti :5173 | xargs kill -9

# Windows PowerShell
Get-Process -Id (Get-NetTCPConnection -LocalPort 5000).OwningProcess | Stop-Process
```

---

## Test Report Template

Use this template to document E2E test runs:

```
Date: YYYY-MM-DD
Tester: [Name]
Environment: [local/docker/production]

Test Results:
- Test 1 (Create Enemy): PASS/FAIL
- Test 2 (Retrieve Enemy): PASS/FAIL
- Test 3 (List All): PASS/FAIL
- Test 4 (Update): PASS/FAIL
- Test 5 (Frontend List): PASS/FAIL
- Test 6 (Detail Page): PASS/FAIL
- Test 7 (Error Handling): PASS/FAIL
- Test 8 (Delete): PASS/FAIL
- Test 9 (Complex Drops): PASS/FAIL
- Test 10 (Performance): PASS/FAIL

Issues Found:
- [List any issues]

Notes:
- [Any observations]
```

---

## Next Steps After E2E Testing

If all tests pass:
1. ✅ MVP is ready for basic usage
2. Consider deploying to staging environment
3. Gather user feedback
4. Plan next features (Items, NPCs, etc.)

If tests fail:
1. Check specific test failure
2. Review logs: `docker-compose logs api`
3. Debug locally (see Troubleshooting)
4. Check relevant code and fix

---

## Related Documentation

- [API_SPECIFICATION.md](./API_SPECIFICATION.md) - API endpoint details
- [ARCHITECTURE.md](./ARCHITECTURE.md) - Design decisions
- [DEVELOPER_SETUP.md](./DEVELOPER_SETUP.md) - Local setup guide
- [README.md](./README.md) - Project overview
