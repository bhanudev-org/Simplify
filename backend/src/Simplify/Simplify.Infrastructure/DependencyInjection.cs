using Microsoft.Extensions.DependencyInjection.Extensions;
using Simplify.Domain.ArticleAggregate;
using Simplify.Infrastructure.Storage;
using Simplify.SeedWork;
using Simplify.SeedWork.Storage;

namespace Simplify.Infrastructure
{
    public static class DependencyInjection
    {
        public static ISimplifyBuilder AddInfra(this ISimplifyBuilder builder)
        {
            builder.Services.TryAddSingleton<ICommandStore<Article>,ArticleRepository>();
            builder.Services.TryAddSingleton<IQueryStore<Article>,ArticleRepository>();
            builder.Services.TryAddSingleton<IArticleRepository, ArticleRepository>();

            Initializer.Initialize();

            return builder;
        }
    }
}