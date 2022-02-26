namespace IdentityServer.Validation
{
    public class InvalidTokenException : InvalidException
    {
        public InvalidTokenException(string errorDescription)
            : base(ProtectedResourceErrors.InvalidToken, errorDescription)
        {

        }
    }
}
