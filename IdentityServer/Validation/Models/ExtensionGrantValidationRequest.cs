namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationRequest
    {
        public TokenGrantValidationRequest Request { get; }
     
        public ExtensionGrantValidationRequest(TokenGrantValidationRequest request)
        {
            Request = request;
        }
    }
}
