namespace IdentityServer.Validation
{
    public class InvalidTokenException : ValidationException
    {
        public InvalidTokenException(string errorDescription)
            : base(OpenIdConnectErrors.InvalidToken, errorDescription)
        {

        }
    }
}
