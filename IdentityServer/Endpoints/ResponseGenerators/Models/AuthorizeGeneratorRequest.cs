using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class AuthorizeGeneratorRequest
    {
        public string? None { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
        public string? ClientId { get; set; }
        public string RedirectUri { get; }
        public string ResponseType { get; }
        public string? ResponseMode { get; }
        public ClaimsPrincipal Subject { get; }
        public Client Client { get; }
        public Resources Resources { get; }

        public AuthorizeGeneratorRequest(string? none, string scope, string state,string clientId, string redirectUri, string responseType, string? responseMode, Client client, Resources resources, ClaimsPrincipal subject)
        {
            None = none;
            Scope = scope;
            State = state;
            ClientId = clientId;
            RedirectUri = redirectUri;
            Client = client;
            Resources = resources;
            Subject = subject;
            ResponseType = responseType;
            ResponseMode = responseMode;
        }
    }
}
