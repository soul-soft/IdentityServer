namespace IdentityServer.Validation
{
    public class InvalidException : Exception
    {
        public string Error { get; }

        public string? ErrorDescription { get; }

        public InvalidException(string error, string errorDescription)
            : base(errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
