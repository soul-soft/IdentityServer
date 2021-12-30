using IdentityServer.Configuration.DependencyInjection;

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
    }
}
