namespace IdentityServer.Validation
{
    public class InvalidClientException : ValidationException
    {
        public InvalidClientException(string errorDescription)
            : base(OpenIdConnectErrors.InvalidClient, errorDescription)
        {

        }
    }
}
