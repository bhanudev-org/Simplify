using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Simplify.SeedWork.Domain;

namespace Simplify.Storage.MongoDb.Serialization
{
    public class EnumerationStringSerializer<TEnumeration> : SerializerBase<TEnumeration> where TEnumeration : Enumeration
    {
        public override TEnumeration Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) =>
            Enumeration.FromName<TEnumeration>(context.Reader.ReadString());

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEnumeration value) =>
            context.Writer.WriteString(value.Name);
    }
}