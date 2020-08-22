using System;
using NodaTime;

namespace Simplify.SeedWork.Entities
{
    public interface IEntityWithAudit
    {
        Guid CreatedById { get; }
        Instant CreatedOn { get; }
        Guid LastModifiedById { get; }
        Instant LastModifiedOn { get; }
    }
}