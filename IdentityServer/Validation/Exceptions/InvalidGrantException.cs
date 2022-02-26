namespace IdentityServer.Validation
{
    public class InvalidGrantException : InvalidException
    {
        public InvalidGrantException(string errorDescription)
            : base(ProtectedResourceErrors.InvalidGrant, errorDescription)
        {

        }
    }
}
