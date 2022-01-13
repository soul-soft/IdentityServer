namespace IdentityServer
{
    internal static class Constants
    {
        public const string IdentityServerName = "IdentityServer";

        public static class EndpointNames
        {
            public const string Authorize = "Authorize";
            public const string Token = "Token";
            public const string Discovery = "Discovery";
            public const string DiscoveryJwks = "Jwks";
            public const string UserInfo = "Userinfo";
        }

        public static class EndpointRoutePaths
        {
            public const string ConnectPathPrefix = "/connect";

            public const string DiscoveryJwks = Discovery + "/jwks";

            public const string Authorize = ConnectPathPrefix + "/authorize";

            public const string Discovery = "/.well-known/openid-configuration";

            public const string Token = ConnectPathPrefix + "/token";

            public const string UserInfo = ConnectPathPrefix + "/userinfo";
        }

        public static readonly string[] ClaimsServiceFilterClaimTypes = 
        {
            JwtClaimTypes.AccessTokenHash,
            JwtClaimTypes.Audience,
            JwtClaimTypes.AuthenticationMethod,
            JwtClaimTypes.AuthenticationTime,
            JwtClaimTypes.AuthorizedParty,
            JwtClaimTypes.AuthorizationCodeHash,
            JwtClaimTypes.ClientId,
            JwtClaimTypes.Expiration,
            JwtClaimTypes.IdentityProvider,
            JwtClaimTypes.IssuedAt,
            JwtClaimTypes.Issuer,
            JwtClaimTypes.JwtId,
            JwtClaimTypes.Nonce,
            JwtClaimTypes.NotBefore,
            JwtClaimTypes.ReferenceTokenId,
            JwtClaimTypes.SessionId,
            JwtClaimTypes.IdentityProvider,
            JwtClaimTypes.Subject,
            JwtClaimTypes.Scope,
            JwtClaimTypes.Confirmation
        };
    }
}
