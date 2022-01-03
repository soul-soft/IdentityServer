using IdentityServer;
using IdentityServer.Application;
using IdentityServer.Configuration;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using static IdentityServer.Constants;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Core
    {
        #region CoreServices
        internal static IIdentityServerBuilder AddCoreServices(this IIdentityServerBuilder builder)
        {
            return builder;
        }
        #endregion       

        #region PlatformServices
        /// <summary>
        /// 必要的平台服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal static IIdentityServerBuilder AddRequiredPlatformServices(this IIdentityServerBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.TryAddSingleton<ISystemClock, SystemClock>();
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<IdentityServerOptions>>().Value);
            builder.Services.AddHttpClient();
            return builder;
        }
        #endregion

        #region Endpoints

        public static IIdentityServerBuilder AddEndpoint<T>(this IIdentityServerBuilder builder, string name, PathString path)
          where T : class, IEndpointHandler
        {
            builder.Services.AddTransient<T>();
            builder.Services.AddSingleton(new IdentityServer.Hosting.Endpoint(name, path, typeof(T)));
            return builder;
        }

        internal static IIdentityServerBuilder AddDefaultEndpoints(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IEndpointRouter, EndpointRouter>();

            builder.AddEndpoint<DiscoveryKeyEndpoint>(EndpointNames.Discovery, ProtocolRoutePaths.DiscoveryWebKeys.EnsureLeadingSlash());
            builder.AddEndpoint<DiscoveryEndpoint>(EndpointNames.Discovery, ProtocolRoutePaths.DiscoveryConfiguration.EnsureLeadingSlash());
            builder.AddEndpoint<TokenEndpoint>(EndpointNames.Token, ProtocolRoutePaths.Token.EnsureLeadingSlash());

            return builder;
        }
        #endregion

        #region PluggableServices
        /// <summary>
        /// 可插拔的服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal static IIdentityServerBuilder AddPluggableServices(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<ITokenService, DefaultTokenService>();
            builder.Services.TryAddTransient<ITokenCreationService, DefaultTokenCreationService>();
            builder.Services.TryAddTransient<IServerUrls, ServerUrls>();
            return builder;
        }
        #endregion

        #region ResponseGenerators
        internal static IIdentityServerBuilder AddResponseGenerators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IDiscoveryResponseGenerator, DiscoveryResponseGenerator>();
            builder.Services.TryAddTransient<IDiscoveryKeyResponseGenerator, DiscoveryKeyResponseGenerator>();
            return builder;
        }
        #endregion

        #region CookieAuthentication
        internal static IIdentityServerBuilder AddCookieAuthentication(this IIdentityServerBuilder builder)
        {
            //builder.Services.AddAuthentication(IdentityServerConstants.DefaultCookieAuthenticationScheme)
            //    .AddCookie(IdentityServerConstants.DefaultCookieAuthenticationScheme)
            //    .AddCookie(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            return builder;
        }
        #endregion

        #region DefaultSecretParsers
        internal static IIdentityServerBuilder AddDefaultSecretParsers(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<ISecretParser, PostBodySecretParser>();
            builder.Services.AddTransient<ISecretsListParser, SecretsListParser>();
            return builder;
        }
        #endregion

        #region DefaultValidators
        internal static IIdentityServerBuilder AddDefaultValidators(this IIdentityServerBuilder builder)
        {
            //Secret
            builder.Services.AddTransient<ISecretValidator, HashedSharedSecretValidator>();
            builder.Services.AddTransient<ISecretsListValidator, SecretsListValidator>();
            builder.Services.AddTransient<IClientSecretValidator, ClientSecretValidator>();
            //requst
            builder.Services.AddTransient<ITokenRequestValidator, TokenRequestValidator>();
            return builder;
        }
        #endregion
    }
}
