
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Simplify.SeedWork.Domain;
using Simplify.Storage.MongoDb;

namespace Simplify.IdentityApp.App;

public class DataProtectionXmlEntity : AggregateRoot
{
    public string Xml { get; set; } = string.Empty;

    public string FriendlyName { get; set; } = string.Empty;
}

public class DataProtectionXmlRepository : MongoDbCollectionBase<DataProtectionXmlEntity>, IXmlRepository
{
    public DataProtectionXmlRepository(ILogger<MongoDbCollectionBase<DataProtectionXmlEntity>> logger, IMongoDbContext mongoDbContext) : base(logger, mongoDbContext, true) { }

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        var result = new List<XElement>();

        var documents = GetAllAsync().Result;

        foreach(var document in documents)
            result.Add(XElement.Parse(document.Xml));

        return result;
    }

    public void StoreElement(XElement element, string friendlyName) => Collection.InsertOneAsync(new DataProtectionXmlEntity { Xml = element.ToString(), FriendlyName = friendlyName }).Wait();

    protected override string CollectionName() => "identityXml";
}
