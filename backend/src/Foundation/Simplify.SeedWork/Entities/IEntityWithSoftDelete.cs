using System;
using NodaTime;

namespace Simplify.SeedWork.Entities
{
    public interface IEntityWithSoftDelete
    {
        bool IsDeleted { get; }
        Guid? DeletedById { get; }
        Instant? DeletedOn { get; }
    }
}