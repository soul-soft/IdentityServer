namespace IdentityServer.Validation
{
    public class ClientCredentialsGrantValidationContext
    {
        public ValidatedTokenRequest Request { get; }
       
        public ClientCredentialsGrantValidationContext(ValidatedTokenRequest request)
        {
            Request = request;
        }
    }
}
