using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Protocols
{
    public static class OpenIdConnectEndpoint
    {
        public class Names
        {
            public const string Authorize = "Authorize";
            public const string Token = "Token";
            public const string Discovery = "Discovery";
            public const string DiscoveryJwks = "Jwks";
            public const string UserInfo = "Userinfo";
        }

        public static class RoutePaths
        {
            public const string ConnectPathPrefix = "/connect";

            public const string DiscoveryJwks = Discovery + "/jwks";

            public const string Authorize = ConnectPathPrefix + "/authorize";

            public const string Discovery = "/.well-known/openid-configuration";

            public const string Token = ConnectPathPrefix + "/token";

            public const string UserInfo = ConnectPathPrefix + "/userinfo";
        }
    }
}
