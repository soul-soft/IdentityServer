using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class AuthorizeGeneratorRequest
    {
        public string? State { get; set; }
        public string? None { get; set; }
        public string RedirectUri { get; }
        public string? ResponseType { get; }
        public string? ResponseMode { get; }
        public ClaimsPrincipal Subject { get; }
        public Client Client { get; }
        public Resources Resources { get; }

        public AuthorizeGeneratorRequest(string? none, string? state, string redirectUri, string? responseType, string? responseMode, Client client, Resources resources, ClaimsPrincipal subject)
        {
            None = none;
            State = state;
            RedirectUri = redirectUri;
            Client = client;
            Resources = resources;
            Subject = subject;
            ResponseType = responseType;
            ResponseMode = responseMode;
        }
    }
}
