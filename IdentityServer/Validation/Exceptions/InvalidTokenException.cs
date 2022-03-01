namespace IdentityServer.Validation
{
    public class InvalidTokenException : InvalidException
    {
        public InvalidTokenException(string errorDescription)
            : base(ProtectedErrors.InvalidToken, errorDescription)
        {

        }
    }
}
