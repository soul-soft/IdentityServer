using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 核心服务
    /// </summary>
    internal static class CoreExtensions
    {
        #region RequiredPlatformEndpoints
        public static IIdentityServerBuilder AddRequiredPlatformEndpoints(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IEndpointRouter, EndpointRouter>();
            builder.AddEndpoint<TokenEndpoint>(Constants.EndpointNames.Token, Constants.EndpointRutePaths.Token);
            builder.AddEndpoint<UserInfoEndpoint>(Constants.EndpointNames.UserInfo, Constants.EndpointRutePaths.UserInfo);
            builder.AddEndpoint<AuthorizeEndpoint>(Constants.EndpointNames.Authorize, Constants.EndpointRutePaths.Authorize);
            builder.AddEndpoint<DiscoveryEndpoint>(Constants.EndpointNames.Discovery, Constants.EndpointRutePaths.Discovery);
            builder.AddEndpoint<IntrospectionEndpoint>(Constants.EndpointNames.Introspection, Constants.EndpointRutePaths.Introspection);
            builder.AddEndpoint<DiscoveryKeyEndpoint>(Constants.EndpointNames.DiscoveryJwks, Constants.EndpointRutePaths.DiscoveryJwks);
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
        /// <summary>
        /// 可插拔的服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddPluggableServices(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IServerUrl, ServerUrl>();
            builder.Services.TryAddTransient<IIdGenerator, IdGenerator>();
            builder.Services.TryAddTransient<IProfileService, ProfileService>();
            builder.Services.TryAddTransient<IClaimService, ClaimService>();
            builder.Services.TryAddTransient<ICache, Cache>();
            builder.Services.TryAddTransient<ITokenService, TokenService>();
            builder.Services.TryAddTransient<IAuthorizeCodeService, AuthorizeCodeService>();
            builder.Services.TryAddTransient<IJwtTokenService, JwtTokenService>();
            return builder;
        }
        #endregion

        #region PluggableValidators
        /// <summary>
        /// 可插拔的验证器
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddPluggableValidators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<ISecretParser, PostBodySecretParser>();
            builder.Services.TryAddTransient<ISecretListParser, SecretListParser>();
            builder.Services.TryAddTransient<ITokenParser, BearerTokenUsageParser>();

            builder.Services.TryAddTransient<ITokenValidator, TokenValidator>();
            builder.Services.TryAddTransient<ISecretValidator, SharedSecretValidator>();
            builder.Services.TryAddTransient<IResourceValidator, ResourceValidator>();
            builder.Services.TryAddTransient<IApiSecretValidator, ApiSecretValidator>();
            builder.Services.TryAddTransient<IClientSecretValidator, ClientSecretValidator>();
            builder.Services.TryAddTransient<ISecretListValidator, SecretListValidator>();
            builder.Services.TryAddTransient<IExtensionGrantListValidator, ExtensionGrantListValidator>();
            builder.Services.TryAddTransient<IRefreshTokenRequestValidator, RefreshTokenRequestValidator>();
            builder.Services.TryAddTransient<IAuthorizeCodeRequestValidator, AuthorizeCodeRequestValidator>();
            builder.Services.TryAddTransient<IClientCredentialsRequestValidator, ClientCredentialsRequestValidator>();
            builder.Services.TryAddTransient<IResourceOwnerCredentialRequestValidator, ResourceOwnerCredentialRequestValidator>();
            return builder;
        }
        #endregion

        #region PluggableResponseGenerators
        public static IIdentityServerBuilder AddPluggableResponseGenerators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IAuthorizeResponseGenerator, AuthorizeResponseGenerator>();
            builder.Services.TryAddTransient<IUserInfoResponseGenerator, UserInfoResponseGenerator>();
            builder.Services.TryAddTransient<ITokenResponseGenerator, TokenResponseGenerator>();
            builder.Services.TryAddTransient<IDiscoveryResponseGenerator, DiscoveryResponseGenerator>();
            builder.Services.TryAddTransient<IIntrospectionResponseGenerator, IntrospectionResponseGenerator>();
            return builder;
        }
        #endregion
    }
}
