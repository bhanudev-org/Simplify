using System;
using System.Collections.Generic;
using NodaTime;
using Simplify.SeedWork.Entities;
using Simplify.SeedWork.Events;

namespace Simplify.SeedWork.Domain
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        protected AggregateRoot() { }

        protected AggregateRoot(Guid id) : this() => Id = id;

        public Guid Id { get; private set; }

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

        public void Created(Guid userId)
        {
            if(!(this is IEntityWithAudit entity)) return;
            entity.CreatedOn = SystemClock.Instance.GetCurrentInstant();
            entity.CreatedById = userId;
        }

        public void Updated(Guid userId)
        {
            if(!(this is IEntityWithAudit entity)) return;
            entity.LastModifiedOn = SystemClock.Instance.GetCurrentInstant();
            entity.LastModifiedById = userId;
        }

        public void Deleted(Guid userId)
        {
            if(!(this is IEntityWithSoftDelete entity)) return;
            entity.DeletedById = userId;
            entity.DeletedOn = SystemClock.Instance.GetCurrentInstant();
            entity.IsDeleted = true;
        }

        public void UndoDelete()
        {
            if(!(this is IEntityWithSoftDelete entity)) return;
            entity.DeletedById = default;
            entity.DeletedOn = default;
            entity.IsDeleted = false;
        }
    }
}