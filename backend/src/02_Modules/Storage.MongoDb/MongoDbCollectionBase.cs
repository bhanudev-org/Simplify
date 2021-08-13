using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Linq;
using Simplify.SeedWork.Domain;
using Simplify.Storage.MongoDb.Builders;

namespace Simplify.Storage.MongoDb
{
    public abstract class MongoDbCollectionBase<TMongoDbEntity> : IMongoDbRepository<TMongoDbEntity> where TMongoDbEntity : IAggregateRoot
    {
        protected static readonly SortDefinitionBuilder<TMongoDbEntity> Sort = Builders<TMongoDbEntity>.Sort;
        protected static readonly UpdateDefinitionBuilder<TMongoDbEntity> Update = Builders<TMongoDbEntity>.Update;
        protected static readonly FieldDefinitionBuilder<TMongoDbEntity> Fields = FieldDefinitionBuilder<TMongoDbEntity>.Instance;
        protected static readonly FilterDefinitionBuilder<TMongoDbEntity> Filter = Builders<TMongoDbEntity>.Filter;
        protected static readonly IndexKeysDefinitionBuilder<TMongoDbEntity> Index = Builders<TMongoDbEntity>.IndexKeys;
        protected static readonly ProjectionDefinitionBuilder<TMongoDbEntity> Projection = Builders<TMongoDbEntity>.Projection;

        protected static readonly UpdateOptions UseUpsert = new UpdateOptions { IsUpsert = true, BypassDocumentValidation = false };
        protected static readonly InsertOneOptions UseInsert = new InsertOneOptions { BypassDocumentValidation = false };
        protected static readonly ReplaceOptions UseReplace = new ReplaceOptions { IsUpsert = true, BypassDocumentValidation = false };
        protected static readonly FindOptions<TMongoDbEntity> UseFind = new FindOptions<TMongoDbEntity>();

        private readonly ILogger<MongoDbCollectionBase<TMongoDbEntity>> _logger;
        private IMongoCollection<TMongoDbEntity>? _collection;

        protected MongoDbCollectionBase(ILogger<MongoDbCollectionBase<TMongoDbEntity>> logger, IMongoDbContext mongoContext, bool initialize = false)
        {
            _logger = logger;
            DbContext = mongoContext;
            if(initialize)
            {
                InitializeAsync().Wait();
            }
        }

        protected IMongoDbContext DbContext { get; }
        protected IMongoCollection<TMongoDbEntity> Collection => _collection ?? throw new InvalidOperationException("Collection is not initialized.");

        IMongoCollection<TMongoDbEntity> IMongoDbRepository<TMongoDbEntity>.Collection => Collection;

        public IMongoQueryable<TMongoDbEntity> Query => Collection.AsQueryable();

        public async Task InitializeAsync(bool SeedData = true, CancellationToken ct = default)
        {
            try
            {
                CreateCollection();
                _logger.LogDebug($"Setup Collection - {CollectionName()}");
                await SetupCollectionAsync(ct);
                _logger.LogDebug("Setup Collection - Completed");
                if(DbContext.SeedingEnabled && SeedData)
                {
                    _logger.LogDebug("Collection - Seed data is enabled");
                    var count = await Query.CountAsync(ct);
                    if(count == 0)
                    {
                        _logger.LogDebug("Collection - Seed data started");
                        await SeedDataAsync();
                        _logger.LogDebug("Collection - Seed data completed");
                    }

                    _logger.LogDebug("Collection - Seed data not executed as collection is not empty");
                }
            }
            catch(Exception ex)
            {
                throw new SystemException($"MongoDb connection failed to connect to database {DbContext.Database.DatabaseNamespace.DatabaseName}", ex);
            }
        }

        public virtual async Task ClearAsync(CancellationToken ct = default)
        {
            await DbContext.Database.DropCollectionAsync(CollectionName(), ct);

            await InitializeAsync(false, ct);
        }

        public async Task<bool> DropCollectionIfExistsAsync(CancellationToken ct = default)
        {
            try
            {
                var collectionName = CollectionName();
                _logger.LogWarning("Dropping Collection - {collectionName}", collectionName);
                await DbContext.Database.DropCollectionAsync(CollectionName(), ct);

                _logger.LogWarning("Create Collection - {collectionName}", collectionName);
                CreateCollection();

                _logger.LogWarning("Setup Collection - {collectionName}", collectionName);
                await SetupCollectionAsync(ct);

                return true;
            }
            catch(MongoException ex)
            {
                _logger.LogCritical(ex, $"Database exception {nameof(DropCollectionIfExistsAsync)}");
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex, nameof(DropCollectionIfExistsAsync));
                return false;
            }
        }

        public virtual async Task<TMongoDbEntity> GetByIdAsync(string id, CancellationToken ct = default)
        {
            var result = await Collection.FindAsync(Filter.Eq("_id", id), cancellationToken: ct);

            return await result.FirstOrDefaultAsync(ct);
        }

        public virtual async Task<long> GetCountAsync(CancellationToken ct = default) => await Collection.CountDocumentsAsync(Filter.Empty, cancellationToken: ct);

        public virtual async Task<IReadOnlyList<TMongoDbEntity>> GetAllAsync(CancellationToken ct = default) => await (await Collection.FindAsync(Filter.Empty, cancellationToken: ct)).ToListAsync(ct);


        protected virtual string CollectionName()
        {
            var name = (typeof(TMongoDbEntity).GetTypeInfo().GetCustomAttributes(typeof(MongoDbCollectionNameAttribute)).FirstOrDefault() as MongoDbCollectionNameAttribute)?.Name;
            if(string.IsNullOrWhiteSpace(name)) name = string.Format(CultureInfo.InvariantCulture, typeof(TMongoDbEntity).Name);
            name = Regex.Replace(name, @"[^0-9a-zA-Z]+", string.Empty);
            if(!name.EndsWith("s")) name += "s";
            return name.Substring(0, 1).ToLowerInvariant() + name.Substring(1);
        }

        protected virtual MongoCollectionSettings CollectionSettings() => new MongoCollectionSettings();

        protected virtual Task SetupCollectionAsync(CancellationToken ct = default)
        {
            Collection.Indexes.CreateOneAsync(new CreateIndexModel<TMongoDbEntity>(Index.Ascending(p => p.Id), new CreateIndexOptions
            {
                Unique = true
            }), cancellationToken: ct);

            return Task.CompletedTask;
        }

        private void CreateCollection()
        {
            _collection = DbContext.Database.GetCollection<TMongoDbEntity>(CollectionName(), CollectionSettings());
        }

        public virtual Task<bool> SeedDataAsync(CancellationToken ct = default) => Task.FromResult(true);

        public virtual async Task<IReadOnlyList<TMongoDbEntity>> AddManyAsync(IEnumerable<TMongoDbEntity> entities, CancellationToken ct = default)
        {
            var data = entities.ToList();
            await Collection.InsertManyAsync(data, cancellationToken: ct);

            return data.AsReadOnly();
        }
    }
}