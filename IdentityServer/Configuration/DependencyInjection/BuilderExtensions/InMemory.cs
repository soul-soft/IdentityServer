using IdentityServer.Configuration;
using IdentityServer.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InMemory
    {
        public static IIdentityServerBuilder ConfigureSigningCredentials(this IIdentityServerBuilder builder, Action<SigningCredentialsBuilder> configure)
        {
            var signingCredentialsBuilder = new SigningCredentialsBuilder();
            configure(signingCredentialsBuilder);
            var signingCredentials = signingCredentialsBuilder.Build();
            builder.Services.TryAddSingleton<ISigningCredentialStore>(sp =>
            {
                return new InMemorySigningCredentialStore(signingCredentials);
            });
            return builder;
        }
    }
}
