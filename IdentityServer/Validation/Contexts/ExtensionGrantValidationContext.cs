namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationContext
    {
        public ValidatedRequest Request { get; }
     
        public ExtensionGrantValidationContext(ValidatedRequest request)
        {
            Request = request;
        }
    }
}
