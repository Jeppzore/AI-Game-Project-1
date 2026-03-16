# REST Client Setup Guide

## 🚀 What's Been Done

✅ Created **6 enemy records** in MongoDB via REST API calls  
✅ All data persisted to Docker volume: `mongo_data`  
✅ Generated `test-data.http` file for VS Code REST Client extension  

## 📊 Test Data Created

| Enemy | Health | Attack | Defense | Experience | Drops |
|-------|--------|--------|---------|-------------|-------|
| Goblin | 10 | 5 | 2 | 25 | 2 |
| Orc | 30 | 12 | 5 | 100 | 3 |
| Dragon | 200 | 50 | 20 | 5000 | 3 |
| Skeleton | 15 | 8 | 3 | 40 | 3 |
| Troll | 80 | 25 | 12 | 300 | 3 |
| Dark Elf | 35 | 18 | 8 | 120 | 3 |

## 🛠️ REST Client Extension Setup

### Install Extension
1. Open VS Code
2. Go to Extensions (Ctrl+Shift+X)
3. Search for "REST Client" 
4. Install by Huachao Mao

### Use test-data.http
1. Open `test-data.http` in VS Code
2. Click "Send Request" above any HTTP request
3. Response appears in side panel

## 📝 Available HTTP Requests in test-data.http

### Check API Health
```http
GET http://localhost:5000/api/enemies
```

### Create New Enemy
```http
POST http://localhost:5000/api/enemies
Content-Type: application/json

{
  "name": "Enemy Name",
  "description": "Description",
  "health": 50,
  "attack": 20,
  "defense": 5,
  "experience": 250,
  "drops": [
    {
      "itemName": "Item Name",
      "dropRate": 0.5,
      "quantity": 1
    }
  ]
}
```

### Get All Enemies
```http
GET http://localhost:5000/api/enemies
```

### Get Specific Enemy
```http
GET http://localhost:5000/api/enemies/{id}
```

### Update Enemy
```http
PUT http://localhost:5000/api/enemies/{id}
Content-Type: application/json

{
  "name": "Updated Name",
  "description": "Updated description",
  "health": 60,
  "attack": 25,
  "defense": 8,
  "experience": 300,
  "drops": [...]
}
```

### Delete Enemy
```http
DELETE http://localhost:5000/api/enemies/{id}
```

## 💾 Data Persistence

All created enemies are persisted in Docker volume:
- **Volume name**: `mongo_data`
- **Location**: Docker Desktop > Preferences > Resources > File Sharing
- **Data survives**: Container restarts, Docker stops/starts
- **Lost only when**: `docker-compose down -v` (removes volumes)

### View Database
```bash
# Connect to MongoDB
mongosh mongodb://admin:password@localhost:27017/nightmares-wiki

# In mongo shell:
use nightmares-wiki
db.enemies.find().pretty()
```

## 🌐 Test the Web App

1. **Visit**: http://localhost:3000
2. **See all enemies** in the wiki list
3. **Click on enemy** for detailed stats
4. **Add more enemies** using REST Client

## 🔄 Common Operations

### Add Vampire Enemy
```json
{
  "name": "Vampire",
  "description": "Undead creature that feeds on blood.",
  "health": 50,
  "attack": 30,
  "defense": 10,
  "experience": 400,
  "drops": [
    {"itemName": "Vampire Fang", "dropRate": 0.6, "quantity": 2},
    {"itemName": "Blood Vial", "dropRate": 0.8, "quantity": 1},
    {"itemName": "Gold", "dropRate": 0.9, "quantity": 150}
  ]
}
```

### Get All Enemies as JSON
```bash
curl http://localhost:5000/api/enemies
```

### Get Specific Enemy
```bash
# Replace with actual ID
curl http://localhost:5000/api/enemies/69b8166e497a42f3d14909cf
```

## ⚙️ Configure REST Client (Optional)

Create `.vscode/settings.json`:
```json
{
  "rest-client.defaultHeaders": {
    "Content-Type": "application/json"
  },
  "rest-client.timeoutinmilliseconds": 10000,
  "rest-client.showResponseInDifferentTab": false
}
```

## 🐛 Troubleshooting

**Error: Connection refused**
- Make sure Docker containers are running: `docker ps`
- Verify API on port 5000: `curl http://localhost:5000/api/enemies`

**Enemy not showing in web app**
- Refresh browser (F5 or Ctrl+R)
- Check browser console for errors (F12)
- Verify API returned success response

**MongoDB connection error**
- Check credentials: admin / password
- Verify MongoDB container is running: `docker ps | grep mongo`
- Check logs: `docker logs nightmares-mongo`

## 📚 Next Steps

1. **Modify test data** in `test-data.http` 
2. **Send requests** using REST Client extension
3. **View results** in web app at http://localhost:3000
4. **Explore API** endpoints in `API_SPECIFICATION.md`
5. **Check database** with MongoDB tools or CLI

## 📖 Related Files

- **REST Requests**: `test-data.http`
- **API Docs**: `API_SPECIFICATION.md`
- **Architecture**: `ARCHITECTURE.md`
- **Developer Setup**: `DEVELOPER_SETUP.md`

---

**Happy testing! 🎮**
