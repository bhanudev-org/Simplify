namespace Simplify.Core.Domain
{
    public abstract class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId> where TEntity : class, IAggregateRoot { }
}