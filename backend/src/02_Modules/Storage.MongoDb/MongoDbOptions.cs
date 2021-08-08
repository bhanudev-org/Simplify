using System.ComponentModel.DataAnnotations;

namespace Simplify.Storage.MongoDb
{
    public class MongoDbOptions
    {
        public const string MongoDb = "Storage:MongoDB";

        public MongoDbOptions()
        {
            SeedingEnabled = false;
            ConnectionString = "mongodb://localhost:27017/SimplifyDefault";
            DatabaseName = "SimplifyDefault";
            DatabaseSettings = default;
        }

        [Required]
        public bool SeedingEnabled { get; set; }

        [Required]
        public string ConnectionString { get; set; }

        [StringLength(20, ErrorMessage = "Too long database name.")]
        public string DatabaseName { get; set; }

        public MongoDatabaseSettings? DatabaseSettings { get; set; }
    }
}