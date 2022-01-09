using IdentityServer.Configuration;
using IdentityServer.Endpoints;
using IdentityServer.Hosting;
using IdentityServer.Protocols;
using IdentityServer.ResponseGenerators;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

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

        #region PluggableServices
        public static IIdentityServerBuilder AddPluggableServices(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IServerUrl, ServerUrl>();
            builder.Services.TryAddTransient<ISecurityTokenService, SecurityTokenService>();
            return builder;
        }
        #endregion

        #region Endpoints

        public static IIdentityServerBuilder AddEndpoint<T>(this IIdentityServerBuilder builder, string name, PathString path)
          where T : class, IEndpointHandler
        {
            builder.Services.AddTransient<T>();
            builder.Services.AddSingleton(new EndpointDescriptor(name, path, typeof(T)));
            return builder;
        }

        internal static IIdentityServerBuilder AddDefaultEndpoints(this IIdentityServerBuilder builder)
        {
            builder.Services.AddSingleton<EndpointDescriptorCollection>();
            builder.Services.AddTransient<IEndpointRouter, EndpointRouter>();
            builder.AddEndpoint<TokenEndpoint>(OpenIdConnectEndpoint.Names.Token, OpenIdConnectEndpoint.RoutePaths.Token);
            builder.AddEndpoint<TokenEndpoint>(OpenIdConnectEndpoint.Names.Authorize, OpenIdConnectEndpoint.RoutePaths.Authorize);
            builder.AddEndpoint<UserInfoEndpoint>(OpenIdConnectEndpoint.Names.UserInfo, OpenIdConnectEndpoint.RoutePaths.UserInfo);
            builder.AddEndpoint<DiscoveryEndpoint>(OpenIdConnectEndpoint.Names.Discovery,  OpenIdConnectEndpoint.RoutePaths.Discovery);
            builder.AddEndpoint<DiscoveryKeyEndpoint>(OpenIdConnectEndpoint.Names.DiscoveryJwks, OpenIdConnectEndpoint.RoutePaths.DiscoveryJwks);
            return builder;
        }
        #endregion

        #region ResponseGenerators
        public static IIdentityServerBuilder AddResponseGenerators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IDiscoveryResponseGenerator, DiscoveryResponseGenerator>();
            builder.Services.TryAddTransient<ITokenResponseGenerator, TokenResponseGenerator>();
            return builder;
        }
        #endregion
      
    }
}
