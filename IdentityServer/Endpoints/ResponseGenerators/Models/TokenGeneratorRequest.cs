using System.Security.Claims;

namespace IdentityServer.Models
{
    public class TokenGeneratorRequest
    {
        public Client Client { get; }
        public string GrantType { get; }
        public ClaimsPrincipal Subject { get; }
        public Resources Resources { get; }
        public AuthorizationCode? Code { get; }
       
        public TokenGeneratorRequest(string grantType, ClaimsPrincipal subject, Client client, Resources resources, AuthorizationCode? authorizationCode = null)
        {
            GrantType = grantType;
            Subject = subject;
            Client = client;
            Resources = resources;
            Code = authorizationCode;
        }
    }
}
