namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationContext
    {
        public GrantRequest Request { get; }
     
        public ExtensionGrantValidationContext(GrantRequest request)
        {
            Request = request;
        }
    }
}
