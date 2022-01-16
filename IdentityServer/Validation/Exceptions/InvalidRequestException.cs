namespace IdentityServer.Validation
{
    public class InvalidRequestException : InvalidException
    {
        public InvalidRequestException(string errorDescription)
            : base(OpenIdConnectTokenErrors.InvalidRequest, errorDescription)
        {

        }
    }
}
