using System.Collections.Generic;

namespace Simplify.Core.Domain
{
    public abstract class AggregateRoot : AggregateRoot<long> { }


    public abstract class AggregateRoot<TEntityId> : Entity<TEntityId>, IAggregateRoot<TEntityId>
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        protected virtual void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        public virtual void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents?.Remove(domainEvent);

        public virtual void ClearDomainEvents() => _domainEvents?.Clear();
    }
}