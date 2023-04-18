using IdentityServer.Serialization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    public class AuthorizeGeneratorResponse
    {
        public string Code { get; private set; }
        public string? State { get; private set; }
        public string RedirectUri { get; private set; }

        public AuthorizeGeneratorResponse(string code, string? state, string redirectUri)
        {
            Code = code;
            State = state;
            RedirectUri = redirectUri;
        }

        public string Serialize()
        {
            var values = new Dictionary<string, object>();
            values.Add(OpenIdConnectParameterNames.Code, Code);
            var redirctUri = $"{RedirectUri}?code={Code}&state={State ?? string.Empty}";
            values.Add(OpenIdConnectParameterNames.RedirectUri, redirctUri);
            return ObjectSerializer.Serialize(values);
        }
    }
}
