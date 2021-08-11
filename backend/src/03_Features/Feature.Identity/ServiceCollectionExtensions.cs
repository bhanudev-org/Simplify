using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Simplify.SeedWork;

namespace Simplify.Feature.Identity
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSimplifyIdentityStores(this IServiceCollection services)
        {
            Guard.NotNull(services);

            services.AddSingleton<UserStore>();
            services.AddSingleton<IUserStore<User>, UserStore>();

            services.AddSingleton<RoleStore>();
            services.AddSingleton<IRoleStore<Role>, RoleStore>();
        }
    }
}