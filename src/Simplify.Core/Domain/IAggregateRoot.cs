namespace Simplify.Core.Domain
{
    public interface IAggregateRoot : IAggregateRoot<long>, IEntity { }

    public interface IAggregateRoot<TEntityId> : IEntity<TEntityId>, IDomainEvents { }
}