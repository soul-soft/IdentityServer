namespace IdentityServer.Models
{
    internal static class Constants
    {
        public const string IdentityServerProvider = "IdentityServer";

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

        public static class AuthenticationSchemes
        {
            public const string AuthorizationHeaderBearer = "Bearer";
            public const string FormPostBearer = "access_token";
            public const string QueryStringBearer = "access_token";
            public const string AuthorizationHeaderPop = "PoP";
            public const string FormPostPop = "pop_access_token";
            public const string QueryStringPop = "pop_access_token";
        }
    }
}
