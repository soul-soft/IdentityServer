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
        public ClaimsPrincipal Subject { get; }
        public IdentityServerOptions Options { get; }

        public AuthorizeGeneratorRequest(string? state, string redirctUri, string responseType, Client client, Resources resources, ClaimsPrincipal subject, IdentityServerOptions options)
        {
            State = state;
            RedirectUri = redirctUri;
            ResponseType = responseType;
            Client = client;
            Options = options;
            Subject = subject;
            Resources = resources;
        }
    }
}
