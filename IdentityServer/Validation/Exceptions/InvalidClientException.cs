namespace IdentityServer.Validation
{
    public class InvalidClientException : InvalidException
    {
        public InvalidClientException(string errorDescription)
            : base(ProtectedResourceErrors.InvalidClient, errorDescription)
        {

        }
    }
}
