namespace IdentityServer.Validation
{
    public class ClientGrantValidationRequest
    {
        public GrantValidationRequest Request { get; }
      
        public ClientGrantValidationRequest(GrantValidationRequest request)
        {
            Request = request;
        }
    }
}
