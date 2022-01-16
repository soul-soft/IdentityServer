namespace IdentityServer.Validation
{
    public class ClientCredentialsGrantValidationContext
    {
        public TokenValidatedRequest Request { get; }
       
        public ClientCredentialsGrantValidationContext(TokenValidatedRequest request)
        {
            Request = request;
        }
    }
}
