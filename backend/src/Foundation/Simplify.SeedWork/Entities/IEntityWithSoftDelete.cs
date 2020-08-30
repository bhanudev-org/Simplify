using System;
using NodaTime;

namespace Simplify.SeedWork.Entities
{
    public interface IEntityWithSoftDelete
    {
        bool IsDeleted { get; set; }
        Guid? DeletedById { get; set; }
        Instant? DeletedOn { get; set; }
    }
}