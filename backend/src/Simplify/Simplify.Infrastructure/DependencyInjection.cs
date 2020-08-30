using Microsoft.Extensions.DependencyInjection.Extensions;
using Simplify.Infrastructure.Storage;
using Simplify.SeedWork;

namespace Simplify.Infrastructure
{
    public static class DependencyInjection
    {
        public static ISimplifyBuilder AddSimplifyInfra(this ISimplifyBuilder builder)
        {
            builder.Services.TryAddSingleton<IArticleRepository, ArticleRepository>();

            return builder;
        }
    }
}