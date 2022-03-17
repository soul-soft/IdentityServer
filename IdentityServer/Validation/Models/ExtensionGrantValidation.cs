namespace IdentityServer.Validation
{
    public class ExtensionGrantValidation
    {
        public TokenRequestValidation Request { get; }
     
        public ExtensionGrantValidation(TokenRequestValidation request)
        {
            Request = request;
        }
    }
}
