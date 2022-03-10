namespace IdentityServer.Validation
{
    public class ExpiredTokenException : ValidationException
    {
        public ExpiredTokenException(string errorDescription)
            : base(OpenIdConnectErrors.ExpiredToken, errorDescription)
        {

        }
    }
}
