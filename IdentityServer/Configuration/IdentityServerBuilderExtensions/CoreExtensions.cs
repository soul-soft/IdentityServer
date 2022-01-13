using IdentityServer;
using IdentityServer.Endpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class CoreExtensions
    {
        #region CoreServices
        public static IIdentityServerBuilder AddCoreServices(this IIdentityServerBuilder builder)
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
        public static IIdentityServerBuilder AddRequiredPlatformServices(this IIdentityServerBuilder builder)
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
            builder.Services.TryAddTransient<IIdGenerator, IdGenerator>();
            builder.Services.TryAddTransient<IClaimIssueService, ClaimIssueService>();
            builder.Services.TryAddTransient<IProfileService, ProfileService>();
            builder.Services.TryAddTransient<ISecretParser, PostBodySecretParser>();
            builder.Services.TryAddTransient<IObjectStorage, ObjectStorage>();
            builder.Services.TryAddTransient<ISecretsListParser, SecretsListParser>();
            builder.Services.TryAddTransient<ITokenService, TokenService>();
            builder.Services.TryAddTransient<ISecurityTokenService, SecurityTokenService>();
            builder.Services.TryAddTransient<IRefreshTokenService, RefreshTokenService>();
            builder.Services.TryAddTransient<IReferenceTokenService, ReferenceTokenService>();
            return builder;
        }
        #endregion

        #region Validators
        public static IIdentityServerBuilder AddValidators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IScopeValidator, ScopeValidator>();
            builder.Services.TryAddTransient<IResourceValidator, ResourceValidator>();
            builder.Services.TryAddTransient<IGrantTypeValidator, GrantTypeValidator>();
            builder.Services.TryAddTransient<ISecretsListValidator, SecretsValidator>();
            builder.Services.TryAddTransient<ISecretValidator, SharedSecretValidator>();
            builder.Services.TryAddTransient<IClientCredentialsGrantValidator, ClientCredentialsGrantValidator>();
            builder.Services.TryAddTransient<IResourceOwnerPasswordGrantValidator, ResourceOwnerPasswordGrantValidator>();
            builder.Services.TryAddTransient<IExtensionGrantsListValidator, ExtensionGrantsListValidator>();
            return builder;
        }
        #endregion

        #region Endpoints
        public static IIdentityServerBuilder AddDefaultEndpoints(this IIdentityServerBuilder builder)
        {
            builder.AddEndpoint<TokenEndpoint>(Constants.EndpointNames.Token, Constants.EndpointRoutePaths.Token);
            builder.AddEndpoint<TokenEndpoint>(Constants.EndpointNames.Authorize, Constants.EndpointRoutePaths.Authorize);
            builder.AddEndpoint<UserInfoEndpoint>(Constants.EndpointNames.UserInfo, Constants.EndpointRoutePaths.UserInfo);
            builder.AddEndpoint<DiscoveryEndpoint>(Constants.EndpointNames.Discovery, Constants.EndpointRoutePaths.Discovery);
            builder.AddEndpoint<DiscoveryKeyEndpoint>(Constants.EndpointNames.DiscoveryJwks, Constants.EndpointRoutePaths.DiscoveryJwks);
            return builder;
        }
        #endregion

        #region ResponseGenerators
        public static IIdentityServerBuilder AddResponseGenerators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<ITokenResponseGenerator, TokenResponseGenerator>();
            builder.Services.TryAddTransient<IDiscoveryResponseGenerator, DiscoveryResponseGenerator>();
            return builder;
        }
        #endregion
    }
}
