namespace IdentityServer.Validation
{
    public class InvalidScopeException : InvalidException
    {
        public InvalidScopeException(string errorDescription)
            : base(ProtectedResourceErrors.InvalidScope, errorDescription)
        {

        }
    }
}
