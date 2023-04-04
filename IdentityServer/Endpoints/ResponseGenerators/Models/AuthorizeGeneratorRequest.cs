using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class AuthorizeGeneratorRequest
    {
        public string? State { get; set; }
        public string RedirectUri { get; }
        public string ResponseType { get; }
        public Client Client { get; }
        public Resources Resources { get; }
        public IdentityServerOptions Options { get; }

        public AuthorizeGeneratorRequest(string? state, string redirctUri, string responseType, Client client, Resources resources, IdentityServerOptions options)
        {
            State = state;
            RedirectUri = redirctUri;
            ResponseType = responseType;
            Client = client;
            Options = options;
            Resources = resources;
        }
    }
}
