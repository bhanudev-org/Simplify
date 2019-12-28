using System;

namespace Simplify.Core.Domain
{
    public interface IEntityWithIsDeleted
    {
        bool IsDeleted { get; set; }
        string DeletedBy { get; set; }
        DateTime DeletedOn { get; set; }
    }
}