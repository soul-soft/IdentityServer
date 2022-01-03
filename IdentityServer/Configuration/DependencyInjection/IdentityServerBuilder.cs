using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Configuration
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
