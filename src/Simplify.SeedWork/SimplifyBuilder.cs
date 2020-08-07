using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Simplify.SeedWork
{
    public sealed class SimplifyBuilder : ISimplifyBuilder
    {
        private SimplifyBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Guard.NotNull(services);
            Services = services;

            Guard.NotNull(configuration);
            Configuration = configuration;

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        }

        public IServiceCollection Services { get; set; }
        public IConfiguration Configuration { get; set; }

        public static ISimplifyBuilder Create(IServiceCollection services, IConfiguration configuration) => new SimplifyBuilder(services, configuration);
    }
}