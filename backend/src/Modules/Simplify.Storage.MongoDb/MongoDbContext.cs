using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Simplify.SeedWork;

namespace Simplify.Storage.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        public MongoDbContext(IOptions<MongoDbContextOptions> options)
        {
            var optionsValue = options.Value;

            Guard.NotNull(optionsValue);

            Guard.NotNull(optionsValue, "MongoDB Options cannot be NULL.");
            Guard.NotNull(optionsValue.ConnectionString, "MongoDB Connection String in Options cannot be NULL.");

            var databaseName = optionsValue.DatabaseName;

            if(string.IsNullOrWhiteSpace(databaseName))
                databaseName = new MongoUrl(optionsValue.ConnectionString).DatabaseName;

            Guard.NotNull(databaseName, "MongoDB Database in Options cannot be NULL.");

            Client = new MongoClient(optionsValue.ConnectionString);
            Database = Client.GetDatabase(databaseName, optionsValue.DatabaseSettings);
            SeedingEnabled = optionsValue.SeedingEnabled;
        }


        public IMongoDatabase Database { get; set; }
        public IMongoClient Client { get; set; }
        public bool SeedingEnabled { get; set; }
    }
}