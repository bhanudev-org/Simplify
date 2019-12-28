using System;
using System.Reflection;
using Simplify.Core.Domain;

namespace Simplify.Core.Extensions {
    public static class EntityExtensions
    {
        public static bool IsNullOrDeleted(this IEntityWithIsDeleted entity) => entity == null || entity.IsDeleted;

        public static void UndoDelete(this IEntityWithIsDeleted entity)
        {
            entity.IsDeleted = false;
            entity.DeletedBy = default;
            entity.DeletedOn = default;
        }
    }
}