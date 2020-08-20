using NodaTime;

namespace Simplify.SeedWork.Entities
{
    public interface IEntityWithSoftDelete
    {
        bool IsDeleted { get; }
        string? DeletedBy { get; }
        Instant? DeletedOn { get; }
    }
}