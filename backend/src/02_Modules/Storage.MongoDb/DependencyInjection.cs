using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Simplify.Storage.MongoDb
{
    public static class DependencyInjection
    {
        [Obsolete]
        public static ISimplifyBuilder AddMongoDb(this ISimplifyBuilder builder, Action<MongoDbOptions> options)
        {
            Initializer.Initialize();
            builder.Services.Configure(options);
            builder.Services.TryAddSingleton<IMongoDbContext, MongoDbContext>();            
            return builder;
        }

        public static ISimplifyBuilder AddMongoDb(this ISimplifyBuilder builder, string sectionName = MongoDbOptions.ConfigurationSectionKey)
        {
            Initializer.Initialize();
            builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(sectionName));
            builder.Services.TryAddSingleton<IMongoDbContext, MongoDbContext>();
            return builder;
        }
    }
}