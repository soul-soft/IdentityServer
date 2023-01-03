using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
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
