namespace IdentityServer.Validation
{
    public class InvalidGrantException : InvalidException
    {
        public InvalidGrantException(string errorDescription)
            : base(OpenIdConnectErrors.InvalidGrant, errorDescription)
        {

        }
    }
}
