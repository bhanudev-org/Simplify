namespace Simplify.Storage.MongoDb
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }

        IMongoClient Client { get; }

        bool SeedingEnabled { get; }
    }
}