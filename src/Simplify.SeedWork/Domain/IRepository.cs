namespace Simplify.SeedWork.Domain
{
    public interface IRepository<TEntity, TEntityId> where TEntity : class, IAggregateRoot { }
}