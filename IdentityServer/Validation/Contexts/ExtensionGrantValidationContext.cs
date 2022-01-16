namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationContext
    {
        public TokenValidatedRequest Request { get; }
     
        public ExtensionGrantValidationContext(TokenValidatedRequest request)
        {
            Request = request;
        }
    }
}
