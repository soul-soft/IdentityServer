namespace IdentityServer.Validation
{
    public class ClientGrantValidationRequest
    {
        public TokenGrantValidationRequest Request { get; }
      
        public ClientGrantValidationRequest(TokenGrantValidationRequest request)
        {
            Request = request;
        }
    }
}
