using IdentityServer.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InMemory
    {
        public static IIdentityServerBuilder AddSigningCredentials(this IIdentityServerBuilder builder, IEnumerable<SigningCredentials> credentials)
        {
            builder.Services.AddSingleton<ISigningCredentialStore>(sp => 
            {
                return new InMemorySigningCredentialStore(credentials);
            });

            return builder;
        }
    }
}
