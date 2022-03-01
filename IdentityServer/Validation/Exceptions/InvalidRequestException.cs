namespace IdentityServer.Validation
{
    public class InvalidRequestException : InvalidException
    {
        public InvalidRequestException(string errorDescription)
            : base(ProtectedErrors.InvalidRequest, errorDescription)
        {

        }
    }
}
