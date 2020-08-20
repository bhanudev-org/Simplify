using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Simplify.SeedWork;
using Simplify.SeedWork.Commands;

namespace Simplify.Infrastructure
{
    public static class DependencyInjection
    {
        public static ISimplifyBuilder AddCommands(this ISimplifyBuilder builder,params Assembly[] assemblies)
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