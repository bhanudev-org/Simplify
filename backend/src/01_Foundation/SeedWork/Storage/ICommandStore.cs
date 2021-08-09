using System.Threading;
using Simplify.SeedWork.Domain;

namespace Simplify.SeedWork.Storage
{
    public interface ICommandStore<in TAggregate> where TAggregate : class, IAggregateRoot
    {
        /// <summary>
        ///     Try to add the Aggregate to the store.
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<(bool IsAdded, Guid Id)> CreateAsync(TAggregate aggregate, CancellationToken ct = default);

        /// <summary>
        ///     Try to update the Aggregate in the store.
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TAggregate aggregate, CancellationToken ct = default);

        /// <summary>
        ///     Try to remove the Aggregate from the store.
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid aggregateId, CancellationToken ct = default);
    }
}