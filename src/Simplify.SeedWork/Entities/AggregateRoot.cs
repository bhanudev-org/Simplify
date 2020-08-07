using System;
using System.Collections.Generic;
using Simplify.SeedWork.Events;

namespace Simplify.SeedWork.Domain
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        protected AggregateRoot() { }

        protected AggregateRoot(Guid id) : this() => Id = id;

        public Guid Id { get; }

        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
        public IReadOnlyList<IDomainEvent> DomainEvents() => _domainEvents.AsReadOnly();
        public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);


        public override bool Equals(object? obj)
        {
            if(!(obj is AggregateRoot other))
                return false;

            if(ReferenceEquals(this, other))
                return true;

            if(GetType() != other.GetType())
                return false;

            if(Id == default || other.Id == default)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(AggregateRoot a, AggregateRoot b)
        {
            if(a is null && b is null)
                return true;

            if(a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(AggregateRoot a, AggregateRoot b) => !(a == b);
        public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
        public override string ToString() => $"[{GetType().Name} {Id}]";
    }
}