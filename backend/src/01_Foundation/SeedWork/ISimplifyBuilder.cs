using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Simplify.SeedWork
{
    public interface ISimplifyBuilder
    {
        IServiceCollection Services { get; }
        IConfiguration Configuration { get; }
    }
}