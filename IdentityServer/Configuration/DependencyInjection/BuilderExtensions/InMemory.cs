using IdentityServer.Models;
using IdentityServer.Storage;
using IdentityServer.Storage.InMemory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InMemory
    {
        public static IIdentityServerBuilder AddInMemorySigningCredentialStore(this IIdentityServerBuilder builder, IEnumerable<SigningCredentials> signingCredentials)
        {
            builder.Services.TryAddSingleton<ISigningCredentialStore>(sp => 
            {
                return new InMemorySigningCredentialsStore(signingCredentials);
            });
            return builder;
        }
        public static IIdentityServerBuilder AddInMemoryClienStore(this IIdentityServerBuilder builder, IEnumerable<Client> clients)
        {
            builder.Services.TryAddSingleton<IClientStore>(new InMemoryClientStore(clients));
            return builder;
        }
    }
}
