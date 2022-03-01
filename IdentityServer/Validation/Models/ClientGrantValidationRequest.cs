namespace IdentityServer.Validation
{
    public class ClientGrantValidationRequest
    {
        public TokenValidationRequest Request { get; }
      
        public ClientGrantValidationRequest(TokenValidationRequest request)
        {
            Request = request;
        }
    }
}
