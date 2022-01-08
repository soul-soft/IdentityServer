using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    public static class OpenIdConnectRoutePaths
    {
        public const string ConnectPathPrefix = "/connect";

        public const string DiscoveryConfiguration = "/.well-known/openid-configuration";

        public const string DiscoveryWebKeys = DiscoveryConfiguration + "/jwks";

        public const string Token = ConnectPathPrefix + "/token";
    }
}
