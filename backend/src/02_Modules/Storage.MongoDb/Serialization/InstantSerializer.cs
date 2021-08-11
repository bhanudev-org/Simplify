using NodaTime.Text;

namespace Simplify.Storage.MongoDb.Serialization
{
    public class InstantSerializer : SerializerBase<Instant>, IBsonPolymorphicSerializer
    {
        public static void Register()
        {
            try
            {
                BsonSerializer.RegisterSerializer(new InstantSerializer());
            }
            catch(BsonSerializationException)
            {
                return;
            }
        }

        public bool IsDiscriminatorCompatibleWithObjectSerializer => true;

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Instant value) =>
            context.Writer.WriteDateTime(value.ToUnixTimeMilliseconds());

        public override Instant Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var type = context.Reader.GetCurrentBsonType();
            switch(type)
            {
                case BsonType.DateTime:
                    return Instant.FromUnixTimeMilliseconds(context.Reader.ReadDateTime());
                case BsonType.String:
                    return InstantPattern.ExtendedIso.Parse(context.Reader.ReadString()).Value;
                case BsonType.Null:
                    throw new InvalidOperationException("Instant is a value type, but the BsonValue is null.");
                default:
                    throw new NotSupportedException($"Cannot convert a {type} to an Instant.");
            }
        }
    }
}