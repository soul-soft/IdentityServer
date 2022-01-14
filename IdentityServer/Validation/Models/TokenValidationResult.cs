using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class TokenValidationResult : ValidationResult
    {
        public IClient Client { get; }

        public ClaimsPrincipal Subject { get; }

        protected TokenValidationResult(bool isError, string description = "success")
            : base(isError, description)
        {
            Client = null!;
            Subject = null!;
        }

        protected TokenValidationResult(IClient client, ClaimsPrincipal subject)
           : base(false)
        {
            Client = client;
            Subject = subject;
        }

        public static new TokenValidationResult Error(string description, params object[] args)
        {
            return new TokenValidationResult(true, string.Format(description, args));
        }

        public static TokenValidationResult Success(IClient client, ClaimsPrincipal subject)
        {
            return new TokenValidationResult(client, subject);
        }
    }
}
