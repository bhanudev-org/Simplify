using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Simplify.SeedWork.Domain;
using Simplify.SeedWork.Events;

namespace Simplify.Feature.Identity
{
    public class Role : IdentityRole<Guid>, IAggregateRoot
    {
        #region Events

        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
        public IReadOnlyList<IDomainEvent> DomainEvents() => _domainEvents.AsReadOnly();
        public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

        #endregion
    }
}
