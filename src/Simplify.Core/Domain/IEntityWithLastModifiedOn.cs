using System;

namespace Simplify.Core.Domain
{
    public interface IEntityWithLastModifiedOn
    {
        DateTime LastModifiedOn { get; set; }
    }
}