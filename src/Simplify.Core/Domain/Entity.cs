using System;

namespace Simplify.Core.Domain
{
    [Serializable]
    public abstract class Entity : Entity<long>, IEntity { }

    [Serializable]
    public abstract class Entity<TEntityId> : IEntity<TEntityId>
    {
        public virtual TEntityId Id { get; set; }

        public override string ToString() => $"[{GetType().Name} {Id}]";
    }
}