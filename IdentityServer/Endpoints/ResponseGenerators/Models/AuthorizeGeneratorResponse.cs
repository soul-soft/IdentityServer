using IdentityServer.Serialization;

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
    }
}
