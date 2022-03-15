using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class TokenValidationResult
    {
        public bool IsError { get;  }
        public string Error { get; } = string.Empty;
        public string? ErrorDescription { get;  }
        public IEnumerable<Claim> Claims { get; } = new List<Claim>();

        private TokenValidationResult(string error, string? errorDescription)
        {
            IsError = true;
            Error = error;
            ErrorDescription = errorDescription;
        }

        private TokenValidationResult(IEnumerable<Claim> claims)
        {
            Claims = claims;
        }

        public static TokenValidationResult Fail(string error, string? errorDescription)
        {
            return new TokenValidationResult(error, errorDescription);
        }

        public static TokenValidationResult Success(IEnumerable<Claim> claims)
        {
            return new TokenValidationResult(claims);
        }
    }
}
