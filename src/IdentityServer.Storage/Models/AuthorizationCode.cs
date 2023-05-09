using System.Security.Claims;

namespace IdentityServer.Models
{
    public class AuthorizationCode
    {
        public string Code { get; set; } = default!;
        public int Lifetime { get; set; }
        public string State { get; set; } = default!;
        public string Scope { get; set; } = default!;
        public string None { get; set; } = default!;
        public string ClientId { get; set; } = default!;
        public string RedirectUri { get; set; } = default!;
        public string ResponseType { get; set; } = default!;
        public string? CodeChallenge { get; } 
        public string? CodeChallengeMethod { get; set; } 
        public string? ResponseMode { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();

        public AuthorizationCode(
            string code,
            string none,
            string state,
            string scope,
            ICollection<Claim> claims,
            string clientId,
            string redirectUri,
            string responseType,
            string? responseMode,
            int lifetime,
            DateTime expirationTime,
            DateTime creationTime,
            string? codeChallenge,
            string? codeChallengeMethod)
        {
            Code = code;
            None = none;
            State = state;
            Scope = scope;
            Claims = claims;
            ClientId = clientId;
            RedirectUri = redirectUri;
            ResponseType = responseType;
            ResponseMode = responseMode;
            Lifetime = lifetime;
            CreationTime = creationTime;
            ExpirationTime = expirationTime;
            CodeChallenge = codeChallenge;
            CodeChallengeMethod = codeChallengeMethod;
        }
    }
}
