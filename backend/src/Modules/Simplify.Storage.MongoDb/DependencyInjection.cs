using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Simplify.SeedWork;

namespace Simplify.Storage.MongoDb
{
    public static class DependencyInjection
    {
        public static ISimplifyBuilder AddMongo(this ISimplifyBuilder builder, Action<MongoDbContextOptions> options)
        {
            builder.Services.Configure(options);
            builder.Services.TryAddSingleton<IMongoDbContext, MongoDbContext>();

            Initializer.Initialize();
            return builder;
        }

        public static ISimplifyBuilder AddMongo(this ISimplifyBuilder builder, string sectionName = "storage:mongodb")
        {
            builder.Services.Configure<MongoDbContextOptions>(builder.Configuration.GetSection(sectionName));
            builder.Services.TryAddSingleton<IMongoDbContext, MongoDbContext>();

            Initializer.Initialize();
            return builder;
        }
    }
}