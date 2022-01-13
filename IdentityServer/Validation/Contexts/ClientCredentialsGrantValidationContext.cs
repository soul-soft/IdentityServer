namespace IdentityServer.Validation
{
    public class ClientCredentialsGrantValidationContext
    {
        public ValidatedRequest Request { get; }
       
        public ClientCredentialsGrantValidationContext(ValidatedRequest request)
        {
            Request = request;
        }
    }
}
