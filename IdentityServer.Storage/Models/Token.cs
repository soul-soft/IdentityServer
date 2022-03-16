using System.Security.Claims;

namespace IdentityServer.Models
{
    public class Token
    {
        public string Type { get; }
        public AccessTokenType AccessTokenType { get; set; }
        public IEnumerable<Claim> Claims { get; }
        public string JwtId
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.JwtId)
                    .Select(s => s.Value).First();
            }
        }
        public string? AuthenticationMethod
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.AuthenticationMethod)
                    .Select(s => s.Value).FirstOrDefault();
            }
        }
        public string? IdentityProvider
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.IdentityProvider)
                    .Select(s => s.Value).FirstOrDefault();
            }
        }
        public string Issuer
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Issuer)
                    .Select(s => s.Value).First();
            }
        }
        public string? SubjectId
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Subject)
                    .Select(s => s.Value).FirstOrDefault();
            }
        }
        public DateTimeOffset Expiration
        {
            get
            {
                var issuedAt = Claims.Where(s => s.Type == JwtClaimTypes.Expiration)
                   .Select(s => s.Value).First();
                return DateTimeOffset.FromUnixTimeSeconds(long.Parse(issuedAt));
            }
        }
        public IEnumerable<string> Audiences
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Audience)
                    .Select(s => s.Value).ToArray();
            }
        }
        public IEnumerable<string> AllowedSigningAlgorithms { get; } 
        public DateTimeOffset IssuedAt
        {
            get
            {
                var issuedAt = Claims.Where(s => s.Type == JwtClaimTypes.IssuedAt)
                    .Select(s => s.Value).First();
                return DateTimeOffset.FromUnixTimeSeconds(long.Parse(issuedAt));
            }
        }

        public Token(string type, AccessTokenType accessTokenType, IEnumerable<Claim> claims, IEnumerable<string> allowedSigningAlgorithms)
        {
            Type = type;
            AccessTokenType = accessTokenType;
            Claims = claims;
            AllowedSigningAlgorithms = allowedSigningAlgorithms;
        }
    }
}
