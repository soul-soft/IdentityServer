namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationRequest
    {
        public GrantValidationRequest Request { get; }
     
        public ExtensionGrantValidationRequest(GrantValidationRequest request)
        {
            Request = request;
        }
    }
}
