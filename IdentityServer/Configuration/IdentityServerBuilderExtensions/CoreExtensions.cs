using IdentityServer;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class CoreExtensions
    {
        #region CoreServices
        public static IIdentityServerBuilder AddLocalAuthentication(this IIdentityServerBuilder builder)
        {
            builder.Services.AddAuthentication()
                .AddLoaclApiAuthentication();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityServerAuthDefaults.PolicyName, policy =>
                {
                    policy.AddAuthenticationSchemes(IdentityServerAuthDefaults.Scheme);
                    policy.RequireAuthenticatedUser();
                });
            });
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
            builder.Services.TryAddTransient<IScopeParser, ScopeParser>();
            builder.Services.TryAddTransient<ISecretParser, PostBodySecretParser>();
            builder.Services.TryAddTransient<SecretParserCollection>();
            builder.Services.TryAddTransient<IBearerTokenUsageParser, BearerTokenUsageParser>();

            builder.Services.TryAddTransient<IServerUrl, ServerUrl>();
            builder.Services.TryAddTransient<IIdGenerator, IdGenerator>();
            builder.Services.TryAddTransient<IProfileService, ProfileService>();
            builder.Services.TryAddTransient<IClaimsService, ClaimsService>();
            builder.Services.TryAddTransient<IPersistentStore, InMemoryPersistentStore>();
            builder.Services.TryAddTransient<ITokenService, TokenService>();
            builder.Services.TryAddTransient<ISecurityTokenService, JwtTokenService>();
            builder.Services.TryAddTransient<IRefreshTokenService, RefreshTokenService>();
            builder.Services.TryAddTransient<IReferenceTokenService, ReferenceTokenService>();
            return builder;
        }
        #endregion

        #region Validators
        public static IIdentityServerBuilder AddValidators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IScopeValidator, ScopeValidator>();
            builder.Services.TryAddTransient<IClaimsValidator, ClaimsValidator>();
            builder.Services.TryAddTransient<IAccessTokenValidator, AccessTokenValidator>();
            builder.Services.TryAddTransient<IResourceValidator, ResourceValidator>();
            builder.Services.TryAddTransient<IGrantTypeValidator, GrantTypeValidator>();            
            builder.Services.TryAddTransient<SecretValidatorCollection>();
            builder.Services.TryAddTransient<ISecretValidator, SharedSecretValidator>();
            builder.Services.TryAddTransient<IRefreshTokenGrantValidator, RefreshTokenGrantValidator>();
            builder.Services.TryAddTransient<IClientCredentialsGrantValidator, ClientCredentialsGrantValidator>();
            builder.Services.TryAddTransient<IPasswordGrantValidator, PasswordGrantValidator>();
            builder.Services.TryAddTransient<ExtensionGrantValidatorCollection>();
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
            builder.Services.AddSingleton<EndpointDescriptorCollectionProvider>();
            return builder;
        }
        #endregion

        #region ResponseGenerators
        public static IIdentityServerBuilder AddResponseGenerators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IUserInfoGenerator, UserInfoGenerator>();
            builder.Services.TryAddTransient<ITokenGenerator, TokenGenerator>();
            builder.Services.TryAddTransient<IDiscoveryGenerator, DiscoveryGenerator>();
            return builder;
        }
        #endregion
    }
}
