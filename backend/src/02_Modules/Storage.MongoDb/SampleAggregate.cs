using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Simplify.SeedWork.Domain;
using Simplify.SeedWork.Entities;

namespace Simplify.Storage.MongoDb
{
    public class SampleAggregate : AggregateRoot, IEntityWithAudit, IEntityWithSoftDelete, IEntityWithTags, IEntityWithVersion
    {
        [BsonId]
        public new Guid Id { get; set; }

        [BsonIgnoreIfDefault]
        [BsonElement("tid", Order = 2)]
        public Guid TenantId { get; set; }

#if NET6_0_OR_GREATER
        [BsonIgnoreIfDefault]
        [BsonElement("dob")]
        public DateOnly DateOfBirth { get; set; }
#else
        [BsonIgnoreIfDefault]
        [BsonElement("dob")]
        public DateTime DateOfBirth { get; set; }
#endif

        [BsonIgnoreIfDefault]
        [BsonExtraElements]
        [BsonElement("extras")]
        public BsonDocument? Extras { get; set; }

        [BsonIgnoreIfDefault]
        [BsonElement("cby")]
        public Guid CreatedById { get; set; }

        [BsonIgnoreIfDefault]
        [BsonElement("con")]
        public Instant CreatedOn { get; set; }

        [BsonIgnoreIfDefault]
        [BsonElement("mby")]
        public Guid LastModifiedById { get; set; }

        [BsonIgnoreIfDefault]
        [BsonElement("mon")]
        public Instant LastModifiedOn { get; set; }

        [BsonIgnoreIfDefault]
        [BsonElement("del")]
        public bool IsDeleted { get; set; }

        [BsonIgnoreIfDefault]
        [BsonElement("dby")]
        public Guid? DeletedById { get; set; }

        [BsonIgnoreIfDefault]
        [BsonElement("don")]
        public Instant? DeletedOn { get; set; }

        [BsonIgnoreIfDefault]
        [BsonElement("tgs")]
        public HashSet<string> Tags { get; set; } = null!;

        [BsonIgnoreIfDefault]
        [BsonElement("ver")]
        public long Version { get; set; }
    }
}