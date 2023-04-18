using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Hosting.DependencyInjection
{
    public class IdentityServerBuilder
        : IIdentityServerBuilder
    {
        public IServiceCollection Services { get; }

        public IdentityServerBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
