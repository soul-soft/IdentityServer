namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationRequest
    {
        public TokenValidationRequest Request { get; }
     
        public ExtensionGrantValidationRequest(TokenValidationRequest request)
        {
            Request = request;
        }
    }
}
