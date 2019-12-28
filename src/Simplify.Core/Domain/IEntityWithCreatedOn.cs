using System;

namespace Simplify.Core.Domain
{
    public interface IEntityWithCreatedOn
    {
        DateTime CreatedOn { get; set; }
    }
}