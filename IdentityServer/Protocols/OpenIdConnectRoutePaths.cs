using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityModel
{
    public static class OpenIdConnectRoutePaths
    {
        public const string ConnectPathPrefix = "/connect";

        public const string Jwks = DiscoveryConfiguration + "/jwks";

        public const string Authorize = ConnectPathPrefix + "/authorize";

        public const string DiscoveryConfiguration = "/.well-known/openid-configuration";

        public const string Token = ConnectPathPrefix + "/token";

        public const string UserInfo = ConnectPathPrefix + "/userinfo";
    }
}
