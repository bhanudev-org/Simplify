using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Simplify.SeedWork.Domain;

namespace Simplify.Storage.MongoDb.Serialization
{
    public class EnumerationDetailedSerializer<TEnumeration> : SerializerBase<TEnumeration> where TEnumeration : Enumeration
    {
        public override TEnumeration Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();

            var type = context.Reader.GetCurrentBsonType();
            switch(type)
            {
                case BsonType.Int32:
                    return Enumeration.FromValue<TEnumeration>(context.Reader.ReadInt32());
                case BsonType.String:
                    return Enumeration.FromName<TEnumeration>(context.Reader.ReadString());
                default:
                    throw new NotSupportedException($"Cannot convert from {type} to {typeof(TEnumeration).Name}");
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEnumeration value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("Id");
            context.Writer.WriteInt32(value.Value);
            context.Writer.WriteName("Name");
            context.Writer.WriteString(value.Name);
            context.Writer.WriteEndDocument();
        }
    }
}