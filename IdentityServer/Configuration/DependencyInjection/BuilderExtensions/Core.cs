using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Endpoints;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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
            builder.Services.AddTransient<IServerUrl, ServerUrl>();
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
            builder.AddEndpoint<DiscoveryEndpoint>(OpenIdConnectEndpointNames.Discovery, OpenIdConnectRoutePaths.DiscoveryConfiguration);
            builder.AddEndpoint<DiscoveryKeyEndpoint>(OpenIdConnectEndpointNames.Discovery, OpenIdConnectRoutePaths.Jwks);
            builder.AddEndpoint<TokenEndpoint>(OpenIdConnectEndpointNames.Token, OpenIdConnectRoutePaths.Token);
            return builder;
        }
        #endregion
    }
}
