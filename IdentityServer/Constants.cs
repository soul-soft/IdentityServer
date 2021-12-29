using Microsoft.AspNetCore.Http;

namespace IdentityServer
{
    internal static class Constants
    {
        public static string IdentityServerName { get; set; } = "IdentityServer";

        public static class EndpointNames
        {
            public const string Authorize = "Authorize";
            public const string Token = "Token";
            public const string DeviceAuthorization = "DeviceAuthorization";
            public const string Discovery = "Discovery";
            public const string Introspection = "Introspection";
            public const string Revocation = "Revocation";
            public const string EndSession = "Endsession";
            public const string CheckSession = "Checksession";
            public const string UserInfo = "Userinfo";
        }

        public static class ProtocolRoutePaths
        {
            public const string ConnectPathPrefix = "connect";

            public const string DiscoveryConfiguration = ".well-known/openid-configuration";
          
            public const string DiscoveryWebKeys = DiscoveryConfiguration + "/jwks";
            
            public const string Token = ConnectPathPrefix + "/token";

            public static readonly string[] CorsPaths =
            {
                DiscoveryConfiguration,
                Token,
            };
        }
    }
}
