using MongoDB.Bson.Serialization;
using Simplify.Domain.ArticleAggregate;
using Simplify.Infrastructure.MongoDb.Serialization;
using Simplify.Storage.MongoDb.Serialization;

namespace Simplify.Infrastructure
{
    public static class Initializer
    {
        public static void Initialize() =>
            BsonClassMap.RegisterClassMap<Article>(map =>
            {
                map.AutoMap();
                map.MapField(p => p.LastModifiedById).SetElementName("mby");
                map.MapField(p => p.LastModifiedOn).SetElementName("mon");
                map.MapField(p => p.CreatedById).SetElementName("cby");
                map.MapField(p => p.CreatedOn).SetElementName("con");
                map.MapField(p => p.Status).SetSerializer(new EnumerationSerializer<ArticleStatus>());
                map.MapField(p => p.Visibility).SetSerializer(new EnumerationSerializer<ArticleVisibility>());
                map.MapField(p => p.Content).SetSerializer(new ArticleContentSerializer());
            });
    }
}