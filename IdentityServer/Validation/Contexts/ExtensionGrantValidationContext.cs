namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationContext
    {
        public ValidatedTokenRequest Request { get; }
     
        public ExtensionGrantValidationContext(ValidatedTokenRequest request)
        {
            Request = request;
        }
    }
}
