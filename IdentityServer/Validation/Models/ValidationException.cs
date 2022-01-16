namespace IdentityServer.Validation
{
    public class ValidationException : Exception
    {
        public string Error { get; }

        public string? ErrorDescription { get; }

        public ValidationException(string error)
           : base(error)
        {
            Error = error;
        }

        public ValidationException(string error, string errorDescription)
            : base(errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
