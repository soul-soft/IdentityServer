using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class ClientCredentialsGrantValidationContext
    {
        public GrantRequest Request { get; }
      
        public ClientCredentialsGrantValidationContext(GrantRequest request)
        {
            Request = request;
        }
    }
}
