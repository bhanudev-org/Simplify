using System.Collections.Generic;

namespace Simplify.SeedWork.Entities
{
    public interface IEntityWithTags
    {
        HashSet<string> Tags { get; }
    }
}