namespace IdentityServer.Validation
{
    public class ExtensionGrantRequestValidation
    {
        public TokenRequestValidation Request { get; }
     
        public ExtensionGrantRequestValidation(TokenRequestValidation request)
        {
            Request = request;
        }
    }
}
