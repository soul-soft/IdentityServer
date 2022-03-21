using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class IntrospectionGeneratorRequest
    {
        public bool IsAuthentication { get; }

        public Client Client { get; }
        
        public ClaimsPrincipal Subject { get; }
        
        public ApiResource ApiResource { get; }

        public IntrospectionGeneratorRequest(bool isAuthentication, Client client, ClaimsPrincipal subject, ApiResource apiResource)
        {
            IsAuthentication = isAuthentication;
            Client = client;
            Subject = subject;
            ApiResource = apiResource;
        }
    }
}
