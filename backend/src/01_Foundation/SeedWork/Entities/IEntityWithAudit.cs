using NodaTime;

namespace Simplify.SeedWork.Entities
{
    public interface IEntityWithAudit
    {
        Guid CreatedById { get; set; }
        Instant CreatedOn { get; set; }
        Guid LastModifiedById { get; set; }
        Instant LastModifiedOn { get; set; }
    }
}