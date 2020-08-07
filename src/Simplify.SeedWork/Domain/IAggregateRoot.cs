using System;
using System.Collections.Generic;
using Simplify.SeedWork.Entities;
using Simplify.SeedWork.Events;

namespace Simplify.SeedWork.Domain
{
    public interface IAggregateRoot : IEntity
    {
        Guid Id { get; }

        IReadOnlyList<IDomainEvent> DomainEvents();

        void ClearDomainEvents();

        void AddDomainEvent(IDomainEvent domainEvent);

        void RemoveDomainEvent(IDomainEvent domainEvent);
    }
}