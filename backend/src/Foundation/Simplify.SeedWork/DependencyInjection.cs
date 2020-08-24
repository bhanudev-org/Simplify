using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simplify.SeedWork.Commands;

namespace Simplify.SeedWork
{
    public static class DependencyInjection
    {
        public static ISimplifyBuilder AddSimplify(this IServiceCollection services, IConfiguration configuration) => SimplifyBuilder.Create(services, configuration);

        public static ISimplifyBuilder AddCommands(this ISimplifyBuilder builder, params Assembly[] assemblies)
        {
            builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

            builder.Services.AddMediator(config =>
            {
                config.AddConsumers(SimplifySeedWorkHelper.Assembly);
                config.AddConsumers(assemblies);
            });

            return builder;
        }
    }
}