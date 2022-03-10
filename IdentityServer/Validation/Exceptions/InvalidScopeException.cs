namespace IdentityServer.Validation
{
    public class InvalidScopeException : ValidationException
    {
        public InvalidScopeException(string errorDescription)
            : base(OpenIdConnectErrors.InvalidScope, errorDescription)
        {

        }
    }
}
