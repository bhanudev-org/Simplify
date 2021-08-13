using MongoDB.Bson.Serialization.Attributes;
using Simplify.SeedWork.Domain;
using Simplify.SeedWork.Events;

namespace Simplify.Feature.Identity
{
    public static class SimplifyIdentityExtensions
    {
        public static class ClaimTypes
        {
            public const string SimplifyDisplayName = "urn:simplify:displayname";
        }

        public static class Constants
        {
            public const string IdentityIssuer = "simplify_identity";
        }
    }

    
}