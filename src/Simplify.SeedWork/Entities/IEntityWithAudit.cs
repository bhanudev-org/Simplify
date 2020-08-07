using NodaTime;

namespace Simplify.SeedWork.Entities
{
    public interface IEntityWithAudit
    {
        string CreatedBy { get; }
        Instant CreatedOn { get; }
        string LastModifiedBy { get; }
        Instant LastModifiedOn { get; }
    }
}