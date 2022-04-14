namespace IdentityServer.Validation
{
    public class ValidationException : Exception
    {
        public string Error { get; }

        public string? ErrorDescription { get; }

        public ValidationException(string errorDescription)
        {
            Error = OpenIdConnectValidationErrors.InvalidRequest;
            ErrorDescription = errorDescription;
        }

        public ValidationException(string error, string errorDescription)
            : base(errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
