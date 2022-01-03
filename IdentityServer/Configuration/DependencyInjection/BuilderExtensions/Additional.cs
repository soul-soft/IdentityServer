using IdentityServer.Configuration.DependencyInjection;
using IdentityServer.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Additional
    {
        #region SigningCredentials
        public static IIdentityServerBuilder AddSigningCredentialStore<TSigningCredentialStore>(this IIdentityServerBuilder builder)
            where TSigningCredentialStore : class, ISigningCredentialStore
        {
            builder.Services.AddTransient<ISigningCredentialStore, TSigningCredentialStore>();
            return builder;
        }      
        public static IIdentityServerBuilder ConfigureSigningCredentials(this IIdentityServerBuilder builder, Action<IdentityServerSigningCredentialsBuilder> configure)
        {
            var arg = new IdentityServerSigningCredentialsBuilder(builder.Services);
            configure(arg);
            arg.Build();
            return builder;
        }
        public static IIdentityServerBuilder AddDefaultSigningCredential(this IIdentityServerBuilder builder)
        {
            var arg = new IdentityServerSigningCredentialsBuilder(builder.Services);
            arg.AddDefaultSigningCredential();
            arg.Build();
            return builder;
        }
        #endregion

        #region Stores
        public static IIdentityServerBuilder AddClientStore<TClientStore>(this IIdentityServerBuilder builder)
            where TClientStore : class, IClientStore
        {
            builder.Services.TryAddTransient<IClientStore, TClientStore>();
            return builder;
        }
        #endregion
    }
}
