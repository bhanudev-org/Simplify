using System.Collections.Generic;

namespace Simplify.Core.Domain
{
    public interface IEntityWithTags
    {
        HashSet<string> Tags { get; set; }
    }
}