using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Simplify.SeedWork;

namespace Simplify.Storage.MongoDb
{
    public static class DependencyInjection
    {
        public static ISimplifyBuilder AddMongoDB(this ISimplifyBuilder builder, Action<MongoDbOptions> options)
        {
            builder.Services.Configure(options);
            builder.Services.TryAddSingleton<IMongoDbContext, MongoDbContext>();

            Initializer.Initialize();
            return builder;
        }

        public static ISimplifyBuilder AddMongoDB(this ISimplifyBuilder builder, string sectionName = "")
        {
            builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(string.IsNullOrWhiteSpace(sectionName) ? MongoDbOptions.MongoDb : sectionName));
            builder.Services.TryAddSingleton<IMongoDbContext, MongoDbContext>();

            Initializer.Initialize();
            return builder;
        }
    }
}