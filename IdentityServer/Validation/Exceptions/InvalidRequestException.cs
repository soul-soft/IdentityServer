namespace IdentityServer.Validation
{
    public class InvalidRequestException : ValidationException
    {
        public InvalidRequestException(string errorDescription)
            : base(OpenIdConnectErrors.InvalidRequest, errorDescription)
        {

        }
    }
}
