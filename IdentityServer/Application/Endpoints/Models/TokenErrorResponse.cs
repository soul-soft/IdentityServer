namespace IdentityServer.Application
{
    public class TokenErrorResponse
    {
        public string Error { get; set; }
        public string? ErrorDescription { get; set; }

        public TokenErrorResponse(string error, string? errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
