namespace Simplify.Core.Domain
{
    public interface IEntity<TEntityId>
    {
        TEntityId Id { get; set; }
    }

    public interface IEntity : IEntity<long> { }
}