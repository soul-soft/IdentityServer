using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Hosting.DependencyInjection
{
    public interface IIdentityServerBuilder
    {
        IServiceCollection Services { get; }
    }
}
