using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    public static class OpenIdConnectEndpointNames
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
}
