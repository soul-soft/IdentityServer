namespace IdentityServer.Validation
{
    public class UnauthorizedException : Exception
    {
        public string Error { get; }

        public string? ErrorDescription { get; }

        public UnauthorizedException(string error)
        {
            Error = error;
        }

        public UnauthorizedException(string error, string errorDescription)
            : base(errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
