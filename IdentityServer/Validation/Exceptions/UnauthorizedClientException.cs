namespace IdentityServer.Validation
{
    public class UnauthorizedClientException : InvalidException
    {
        public UnauthorizedClientException(string errorDescription)
            : base(OpenIdConnectTokenErrors.UnauthorizedClient, errorDescription)
        {

        }
    }
}
