using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;

namespace Simplify.Storage.MongoDb
{
    public class MongoDbContextOptions
    {
        public MongoDbContextOptions()
        {
            SeedingEnabled = false;
            ConnectionString = string.Empty;
            DatabaseName = string.Empty;
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