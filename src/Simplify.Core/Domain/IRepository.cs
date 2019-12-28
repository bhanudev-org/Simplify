namespace Simplify.Core.Domain
{
    public interface IRepository<TEntity, TEntityId> where TEntity : class, IAggregateRoot { }
}