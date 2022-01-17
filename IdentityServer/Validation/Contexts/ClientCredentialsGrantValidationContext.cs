using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class ClientCredentialsGrantValidationContext
    {
        public GrantValidationRequest Request { get; }
      
        public ClientCredentialsGrantValidationContext(GrantValidationRequest request)
        {
            Request = request;
        }
    }
}
