using Simplify.SeedWork.Entities;

namespace Simplify.SeedWork.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsDeleted(this IEntityWithSoftDelete entity) => entity?.IsDeleted == true;
    }
}