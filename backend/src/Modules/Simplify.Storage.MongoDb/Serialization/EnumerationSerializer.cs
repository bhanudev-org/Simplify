using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Pro.Enum;

namespace Simplify.Storage.MongoDb.Serialization
{
    public class EnumerationSerializer<TEnumeration> : SerializerBase<TEnumeration> where TEnumeration : Enumeration
    {
        public override TEnumeration Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) =>
            Enumeration.FromValue<TEnumeration>(context.Reader.ReadInt32());

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEnumeration value) =>
            context.Writer.WriteInt32(value.Value);
    }
}