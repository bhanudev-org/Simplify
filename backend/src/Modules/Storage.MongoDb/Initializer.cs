using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using NodaTime;
using Simplify.Storage.MongoDb.Serialization;

namespace Simplify.Storage.MongoDb
{
    public static class Initializer
    {
        public static void Initialize()
        {
            MongoDefaults.AssignIdOnInsert = true;

            BsonSerializer.RegisterIdGenerator(typeof(Guid), GuidGenerator.Instance);
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
            BsonSerializer.RegisterSerializer(typeof(Instant), new InstantSerializer());

            ConventionRegistry.Register("CamelCase", new ConventionPack {new CamelCaseElementNameConvention()}, t => true);
            ConventionRegistry.Register("IgnoreIfDefault", new ConventionPack {new IgnoreIfDefaultConvention(true)}, t => true);
            ConventionRegistry.Register("IgnoreExtraElements", new ConventionPack {new IgnoreExtraElementsConvention(true)}, t => true);
        }
    }
}