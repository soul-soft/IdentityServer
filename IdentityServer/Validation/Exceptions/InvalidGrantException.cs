namespace IdentityServer.Validation
{
    public class InvalidGrantException : InvalidException
    {
        public InvalidGrantException(string errorDescription)
            : base(ProtectedErrors.InvalidGrant, errorDescription)
        {

        }
    }
}
