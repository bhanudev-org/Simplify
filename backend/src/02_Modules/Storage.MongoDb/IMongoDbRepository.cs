using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Simplify.SeedWork.Entities;

namespace Simplify.Storage.MongoDb
{
    public interface IMongoDbRepository<TMongoDbEntity> where TMongoDbEntity : IEntity
    {
        IMongoCollection<TMongoDbEntity> Collection { get; }

        IMongoQueryable<TMongoDbEntity> Query { get; }

        Task ClearAsync(CancellationToken ct = default);

        Task InitializeAsync(CancellationToken ct = default);

        Task<bool> DropCollectionIfExistsAsync(CancellationToken ct = default);

        Task<TMongoDbEntity> GetByIdAsync(string id, CancellationToken ct = default);

        Task<long> GetCountAsync(CancellationToken ct = default);

        Task<IReadOnlyList<TMongoDbEntity>> GetAllAsync(CancellationToken ct = default);
    }
}