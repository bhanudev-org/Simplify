namespace Simplify.Storage.MongoDb
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MongoDbCollectionNameAttribute : Attribute
    {
        public MongoDbCollectionNameAttribute(string name) => Name = name;

        public string Name { get; set; }
    }
}