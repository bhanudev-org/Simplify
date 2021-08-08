using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Simplify.SeedWork
{
    public static class DependencyInjection
    {
        public static ISimplifyBuilder AddSimplify(this IServiceCollection services, IConfiguration configuration) => SimplifyBuilder.Create(services, configuration);

        public static ISimplifyBuilder AddCommands(this ISimplifyBuilder builder, params Assembly[] assemblies)
        {
            builder.Services.AddMediatR(assemblies);

            return builder;
        }
    }
}