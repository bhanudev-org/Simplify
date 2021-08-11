using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Simplify.SeedWork
{
    public interface ISimplifyBuilder
    {
        ISimplifyOptions Options { get; }
        IServiceCollection Services { get; }
        IConfiguration Configuration { get; }
    }

    public sealed class SimplifyBuilder : ISimplifyBuilder
    {
        private SimplifyBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Guard.NotNull(services);
            Services = services;

            Guard.NotNull(configuration);
            Configuration = configuration;

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            var section = Configuration.GetSection(SimplifyOptions.ConfigurationSectionKey);
            services.Configure<SimplifyOptions>(section);
            var options = new SimplifyOptions();
            section.Bind(options);

            options.Id = string.IsNullOrWhiteSpace(options.Id) ? "sy" : options.Id.ToLowerInvariant().Trim();
            options.Slug = string.IsNullOrWhiteSpace(options.Slug) ? "simplify" : options.Slug.Trim();
            options.Name = string.IsNullOrWhiteSpace(options.Name) ? "Simplify" : options.Name.Trim();
            options.BaseUrl = string.IsNullOrWhiteSpace(options.BaseUrl) ? "/" : options.BaseUrl.ToLowerInvariant().Trim();
            options.Version = string.IsNullOrWhiteSpace(options.Version) ? typeof(SimplifyBuilder).Assembly.GetName().Version?.ToString() ?? "0.0.1" : options.Version.Trim();
            options.DataDirectory = string.IsNullOrWhiteSpace(options.DataDirectory) ? "./.private" : options.DataDirectory.Trim();

            Options = options;
        }

        public ISimplifyOptions Options { get; set; }
        public IServiceCollection Services { get; set; }
        public IConfiguration Configuration { get; set; }

        public static ISimplifyBuilder Create(IServiceCollection services, IConfiguration configuration) => new SimplifyBuilder(services, configuration);
    }
}