using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Simplify.Core;

namespace Simplify.Infrastructure.MongoDb.Serialization
{
    internal class ArticleContentSerializer : SerializerBase<ArticleContent>
    {
        public override ArticleContent Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) =>
            ArticleContent.Create(context.Reader.ReadString()).Value;

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ArticleContent value) =>
            context.Writer.WriteString(value.Value);
    }
}