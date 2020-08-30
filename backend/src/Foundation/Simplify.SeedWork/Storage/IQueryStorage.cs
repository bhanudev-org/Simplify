using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Simplify.SeedWork.Domain;

namespace Simplify.SeedWork.Storage
{
    public interface IQueryStorage<TAggregate> where TAggregate : class, IAggregateRoot
    {
        Task<IReadOnlyCollection<TAggregate>> GetAsync(CancellationToken ct = default);

        /// <summary>
        ///     Retrieve the Aggregate for a given ID.
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TAggregate> GetByIdAsync(Guid aggregateId, CancellationToken ct = default);
    }
}