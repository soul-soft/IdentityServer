using IdentityServer.Serialization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    public class AuthorizeGeneratorResponse
    {
        public string Code { get; private set; }
        public string RedirectUri { get; private set; }
      
        public AuthorizeGeneratorResponse(string code, string redirectUri)
        {
            Code = code;
            RedirectUri = redirectUri;
        }

        public string Serialize()
        {
            var values = new Dictionary<string, object>();
            values.Add(OpenIdConnectParameterNames.Code, Code);
            values.Add(OpenIdConnectParameterNames.RedirectUri, RedirectUri);
            return ObjectSerializer.Serialize(values);
        }
    }
}
