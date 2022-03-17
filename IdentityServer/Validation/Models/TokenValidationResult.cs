using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class TokenValidationResult
    {
        public Client Client { get; } = null!;
        public IEnumerable<Claim> Claims { get; } = new List<Claim>();
        public bool IsError { get; }
        public string Error { get; } = string.Empty;
        public string? ErrorDescription { get; }

        private TokenValidationResult(string error, string? errorDescription)
        {
            IsError = true;
            Error = error;
            ErrorDescription = errorDescription;
        }

        private TokenValidationResult(Client client, IEnumerable<Claim> claims)
        {
            Client = client;
            Claims = claims;
        }

        public static TokenValidationResult Fail(string error, string? errorDescription)
        {
            return new TokenValidationResult(error, errorDescription);
        }

        public static TokenValidationResult Success(Client client, IEnumerable<Claim> claims)
        {
            return new TokenValidationResult(client, claims);
        }
    }
}
