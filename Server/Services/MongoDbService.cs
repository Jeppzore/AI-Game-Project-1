using MongoDB.Driver;
using Server.Models;

namespace Server.Services;

public interface IMongoDbService
{
    IMongoCollection<Enemy> GetEnemiesCollection();
    IMongoCollection<Creature> GetCreaturesCollection();
    IMongoCollection<Item> GetItemsCollection();
    IMongoCollection<NPC> GetNpcsCollection();
    IMongoCollection<Obstacle> GetObstaclesCollection();
    IMongoDatabase GetDatabase();
    Task EnsureIndexesAsync();
}

public class MongoDbService : IMongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB")
            ?? "mongodb://localhost:27017";
        var databaseName = configuration["MongoDB:DatabaseName"] ?? "nightmares-wiki";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Enemy> GetEnemiesCollection()
    {
        return _database.GetCollection<Enemy>("enemies");
    }

    public IMongoCollection<Creature> GetCreaturesCollection()
    {
        return _database.GetCollection<Creature>("creatures");
    }

    public IMongoCollection<Item> GetItemsCollection()
    {
        return _database.GetCollection<Item>("items");
    }

    public IMongoCollection<NPC> GetNpcsCollection()
    {
        return _database.GetCollection<NPC>("npcs");
    }

    public IMongoCollection<Obstacle> GetObstaclesCollection()
    {
        return _database.GetCollection<Obstacle>("obstacles");
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }

    public async Task EnsureIndexesAsync()
    {
        // Creatures indexes
        var creaturesCollection = GetCreaturesCollection();
        await creaturesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Creature>(
                Builders<Creature>.IndexKeys.Ascending(c => c.Name)));
        await creaturesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Creature>(
                Builders<Creature>.IndexKeys.Ascending(c => c.Type)));
        await creaturesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Creature>(
                Builders<Creature>.IndexKeys.Ascending(c => c.Location)));

        // Items indexes
        var itemsCollection = GetItemsCollection();
        await itemsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Item>(
                Builders<Item>.IndexKeys.Ascending(i => i.Name)));
        await itemsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Item>(
                Builders<Item>.IndexKeys.Ascending(i => i.Type)));
        await itemsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Item>(
                Builders<Item>.IndexKeys.Ascending(i => i.Category)));
        await itemsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Item>(
                Builders<Item>.IndexKeys.Ascending(i => i.Rarity)));

        // NPCs indexes
        var npcsCollection = GetNpcsCollection();
        await npcsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<NPC>(
                Builders<NPC>.IndexKeys.Ascending(n => n.Name)));
        await npcsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<NPC>(
                Builders<NPC>.IndexKeys.Ascending(n => n.Role)));
        await npcsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<NPC>(
                Builders<NPC>.IndexKeys.Ascending(n => n.Location.Area)));

        // Obstacles indexes
        var obstaclesCollection = GetObstaclesCollection();
        await obstaclesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Obstacle>(
                Builders<Obstacle>.IndexKeys.Ascending(o => o.Name)));
        await obstaclesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Obstacle>(
                Builders<Obstacle>.IndexKeys.Ascending(o => o.Type)));
    }
}
