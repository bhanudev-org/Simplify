using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Simplify.SeedWork;

namespace Simplify.Feature.Identity
{
    public static class DependencyInjection
    {
        public static void AddSimplifyIdentity(this IServiceCollection services)
        {
            Guard.NotNull(services);

            services.AddScoped<IUserStore<User>, UserStore>();
        }

        public static ISimplifyBuilder AddIdentity(this ISimplifyBuilder builder)
        {
            builder.Services.AddSimplifyIdentity();

            return builder;
        }
    }
}