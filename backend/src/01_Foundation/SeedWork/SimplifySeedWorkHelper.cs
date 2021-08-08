using System.Reflection;
using NodaTime;
using Simplify.SeedWork.Domain;
using Simplify.SeedWork.Entities;

namespace Simplify.SeedWork
{
    public static class SimplifySeedWorkHelper
    {
        public static readonly Assembly Assembly = typeof(SimplifySeedWorkHelper).Assembly;
    }

    public static class SimplifySeedWorkExtensions
    {
        public static void Updated(this IAggregateRoot aggregate)
        {
            if(aggregate is IEntityWithAudit entity)
            {
                entity.LastModifiedOn = SystemClock.Instance.GetCurrentInstant();
            }

        }
    }
}