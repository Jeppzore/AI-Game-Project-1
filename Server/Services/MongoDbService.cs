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
    IMongoCollection<ObstacleAttempt> GetObstacleAttemptsCollection();
    IMongoCollection<ShopInventory> GetShopInventoryCollection();
    IMongoCollection<PlayerInventory> GetPlayerInventoryCollection();
    IMongoCollection<Transaction> GetTransactionsCollection();
    IMongoCollection<Quest> GetQuestsCollection();
    IMongoCollection<Objective> GetObjectivesCollection();
    IMongoCollection<QuestProgress> GetQuestProgressCollection();
    IMongoCollection<QuestReward> GetQuestRewardsCollection();
    IMongoCollection<GameModels.CraftingRecipe> GetCraftingRecipesCollection();
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

    public IMongoCollection<ObstacleAttempt> GetObstacleAttemptsCollection()
    {
        return _database.GetCollection<ObstacleAttempt>("obstacleAttempts");
    }

    public IMongoCollection<ShopInventory> GetShopInventoryCollection()
    {
        return _database.GetCollection<ShopInventory>("shop_inventory");
    }

    public IMongoCollection<PlayerInventory> GetPlayerInventoryCollection()
    {
        return _database.GetCollection<PlayerInventory>("player_inventory");
    }

    public IMongoCollection<Transaction> GetTransactionsCollection()
    {
        return _database.GetCollection<Transaction>("transactions");
    }

    public IMongoCollection<Quest> GetQuestsCollection()
    {
        return _database.GetCollection<Quest>("quests");
    }

    public IMongoCollection<Objective> GetObjectivesCollection()
    {
        return _database.GetCollection<Objective>("objectives");
    }

    public IMongoCollection<QuestProgress> GetQuestProgressCollection()
    {
        return _database.GetCollection<QuestProgress>("quest_progress");
    }

    public IMongoCollection<QuestReward> GetQuestRewardsCollection()
    {
        return _database.GetCollection<QuestReward>("rewards");
    }

    public IMongoCollection<GameModels.CraftingRecipe> GetCraftingRecipesCollection()
    {
        return _database.GetCollection<GameModels.CraftingRecipe>("crafting_recipes");
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
        await obstaclesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Obstacle>(
                Builders<Obstacle>.IndexKeys.Ascending(o => o.Difficulty)));
        await obstaclesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Obstacle>(
                Builders<Obstacle>.IndexKeys.Ascending(o => o.Location)));

        // Obstacle Attempts indexes
        var attemptsCollection = GetObstacleAttemptsCollection();
        await attemptsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<ObstacleAttempt>(
                Builders<ObstacleAttempt>.IndexKeys.Ascending(a => a.PlayerId)));
        await attemptsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<ObstacleAttempt>(
                Builders<ObstacleAttempt>.IndexKeys.Ascending(a => a.ObstacleId)));
        await attemptsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<ObstacleAttempt>(
                Builders<ObstacleAttempt>.IndexKeys.Descending(a => a.Timestamp)));

        // Shop inventory indexes
        var shopInventoryCollection = GetShopInventoryCollection();
        await shopInventoryCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<ShopInventory>(
                Builders<ShopInventory>.IndexKeys.Ascending(s => s.NpcId)));
        await shopInventoryCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<ShopInventory>(
                Builders<ShopInventory>.IndexKeys.Ascending("items.itemId")));

        // Player inventory indexes
        var playerInventoryCollection = GetPlayerInventoryCollection();
        await playerInventoryCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<PlayerInventory>(
                Builders<PlayerInventory>.IndexKeys.Ascending(p => p.PlayerId)));

        // Transactions indexes
        var transactionsCollection = GetTransactionsCollection();
        await transactionsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Transaction>(
                Builders<Transaction>.IndexKeys.Ascending(t => t.BuyerId).Descending(t => t.Timestamp)));

        // Quests indexes
        var questsCollection = GetQuestsCollection();
        await questsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Quest>(
                Builders<Quest>.IndexKeys.Ascending(q => q.QuestId), 
                new CreateIndexOptions { Unique = true }));
        await questsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Quest>(
                Builders<Quest>.IndexKeys.Ascending(q => q.GiverNpcId)));
        await questsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Quest>(
                Builders<Quest>.IndexKeys.Ascending(q => q.LevelRequirement)));

        // Objectives indexes
        var objectivesCollection = GetObjectivesCollection();
        await objectivesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Objective>(
                Builders<Objective>.IndexKeys.Ascending(o => o.ObjectiveId),
                new CreateIndexOptions { Unique = true }));
        await objectivesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<Objective>(
                Builders<Objective>.IndexKeys.Ascending(o => o.Type)));

        // Quest Progress indexes
        var questProgressCollection = GetQuestProgressCollection();
        await questProgressCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<QuestProgress>(
                Builders<QuestProgress>.IndexKeys.Ascending(qp => qp.PlayerId).Ascending(qp => qp.QuestId),
                new CreateIndexOptions { Unique = true }));
        await questProgressCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<QuestProgress>(
                Builders<QuestProgress>.IndexKeys.Ascending(qp => qp.PlayerId).Ascending(qp => qp.Status)));

        // Rewards indexes
        var rewardsCollection = GetQuestRewardsCollection();
        await rewardsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<QuestReward>(
                Builders<QuestReward>.IndexKeys.Ascending(r => r.QuestId)));
        await rewardsCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<QuestReward>(
                Builders<QuestReward>.IndexKeys.Ascending(r => r.RewardType)));

        // Crafting Recipes indexes
        var craftingRecipesCollection = GetCraftingRecipesCollection();
        await craftingRecipesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<GameModels.CraftingRecipe>(
                Builders<GameModels.CraftingRecipe>.IndexKeys.Ascending(cr => cr.Name)));
        await craftingRecipesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<GameModels.CraftingRecipe>(
                Builders<GameModels.CraftingRecipe>.IndexKeys.Ascending(cr => cr.SkillRequirement)));
        await craftingRecipesCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<GameModels.CraftingRecipe>(
                Builders<GameModels.CraftingRecipe>.IndexKeys.Ascending(cr => cr.LevelRequirement)));
    }
}
