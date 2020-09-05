using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Simplify.Domain.ArticleAggregate;
using Simplify.Storage.MongoDb;

namespace Simplify.Infrastructure.Storage
{
    public class ArticleRepository : MongoDbCollectionBase<Article>, IArticleRepository
    {
        public ArticleRepository(ILogger<MongoDbCollectionBase<Article>> logger, IMongoDbContext mongoContext) : base(logger, mongoContext) { }
        
        public async Task<IReadOnlyCollection<Article>> GetAsync(CancellationToken ct = default)
        {
            var result = await Collection.FindAsync(Filter.Empty, cancellationToken: ct);

            return await result.ToListAsync(ct);
        }

        public async Task<Article> GetByIdAsync(Guid aggregateId, CancellationToken ct = default)
        {
            var result = await Collection.Find(q => q.Id == aggregateId).FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task<(bool IsAdded, Guid Id)> CreateAsync(Article aggregate, CancellationToken ct = default)
        {
            await Collection.InsertOneAsync(aggregate, UseInsert, ct);

            return (true, aggregate.Id);
        }

        public async Task<bool> UpdateAsync(Article aggregate, CancellationToken ct = default)
        {
            var result = await Collection.ReplaceOneAsync(Filter.Empty, aggregate, UseReplace, ct);

            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteAsync(Guid aggregateId, CancellationToken ct = default)
        {
            var aggregate = await Query.FirstOrDefaultAsync(q => q.Id == aggregateId, ct);
            aggregate.UpdateStatus(ArticleStatus.Draft);

            var result = await UpdateAsync(aggregate, ct);

            return result;
        }

        #region MongoDB Settings

        protected override string CollectionName() => "articles";

        protected override Task SetupCollectionAsync(CancellationToken ct = default)
        {
            Collection.Indexes.CreateOneAsync(new CreateIndexModel<Article>(Index.Text(q => q.Content), new CreateIndexOptions {Name = "idx_txt_context"}), cancellationToken: ct);

            return base.SetupCollectionAsync(ct);
        }

        #endregion
    }
}