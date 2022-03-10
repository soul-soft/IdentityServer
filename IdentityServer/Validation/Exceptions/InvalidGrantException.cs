namespace IdentityServer.Validation
{
    public class InvalidGrantException : ValidationException
    {
        public InvalidGrantException(string errorDescription)
            : base(OpenIdConnectErrors.InvalidGrant, errorDescription)
        {

        }
    }
}
