using IdentityServer.Configuration.DependencyInjection;
using IdentityServer.Models;
using IdentityServer.Storage;
using IdentityServer.Storage.InMemory;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Additional
    {
        #region signingCredentials
        public static IIdentityServerBuilder AddSigningCredentials(this IIdentityServerBuilder builder, Action<IdentityServerSigningCredentialsBuilder> configure)
        {
            var arg = new IdentityServerSigningCredentialsBuilder(builder.Services);
            configure(arg);
            arg.Build();
            return builder;
        }
        #endregion

        #region InMemory
        public static IIdentityServerBuilder AddInMemoryClienStore(this IIdentityServerBuilder builder, IEnumerable<Client> clients)
        {
            builder.Services.AddSingleton<IClientStore>(new InMemoryClientStore(clients));
            return builder;
        }
        #endregion
    }
}
