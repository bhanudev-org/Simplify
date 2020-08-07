using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Simplify.SeedWork
{
    public static class DependencyInjection
    {
        public static ISimplifyBuilder AddSimplify(this IServiceCollection services, IConfiguration configuration) => SimplifyBuilder.Create(services, configuration);
    }
}