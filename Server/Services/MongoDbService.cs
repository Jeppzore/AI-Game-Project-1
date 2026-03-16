using MongoDB.Driver;
using Server.Models;

namespace Server.Services;

public interface IMongoDbService
{
    IMongoCollection<Enemy> GetEnemiesCollection();
    IMongoDatabase GetDatabase();
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

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }
}
