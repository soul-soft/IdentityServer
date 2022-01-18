namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationContext
    {
        public GrantValidationRequest Request { get; }
     
        public ExtensionGrantValidationContext(GrantValidationRequest request)
        {
            Request = request;
        }
    }
}
