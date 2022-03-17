using System.Security.Claims;
using System.Text.Json.Serialization;

namespace IdentityServer.Models
{
    public class Token
    {
        public string Id { get; set; }
        public string? Type { get; set; }
        public AccessTokenType AccessTokenType { get; set; }        
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
        public IEnumerable<string> AllowedSigningAlgorithms { get; } = new HashSet<string>();
      
        public Token(string id, string? type, AccessTokenType accessTokenType, IEnumerable<Claim> claims, IEnumerable<string> allowedSigningAlgorithms)
        {
            Id = id;
            Type = type;
            AccessTokenType = accessTokenType;
            Claims = claims;
            AllowedSigningAlgorithms = allowedSigningAlgorithms;
        }

        [JsonIgnore]
        public string JwtId
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.JwtId)
                    .Select(s => s.Value).First();
            }
        }
        [JsonIgnore]
        public string? AuthenticationMethod
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.AuthenticationMethod)
                    .Select(s => s.Value).FirstOrDefault();
            }
        }
        [JsonIgnore]
        public string? IdentityProvider
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.IdentityProvider)
                    .Select(s => s.Value).FirstOrDefault();
            }
        }
        [JsonIgnore]
        public string Issuer
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Issuer)
                    .Select(s => s.Value).First();
            }
        }
        [JsonIgnore]
        public string? SubjectId
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Subject)
                    .Select(s => s.Value).FirstOrDefault();
            }
        }
        [JsonIgnore]
        public DateTimeOffset Expiration
        {
            get
            {
                var issuedAt = Claims.Where(s => s.Type == JwtClaimTypes.Expiration)
                   .Select(s => s.Value).First();
                return DateTimeOffset.FromUnixTimeSeconds(long.Parse(issuedAt));
            }
        }
        [JsonIgnore]
        public IEnumerable<string> Audiences
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Audience)
                    .Select(s => s.Value).ToArray();
            }
        }
        [JsonIgnore]
        public DateTimeOffset IssuedAt
        {
            get
            {
                var issuedAt = Claims.Where(s => s.Type == JwtClaimTypes.IssuedAt)
                    .Select(s => s.Value).First();
                return DateTimeOffset.FromUnixTimeSeconds(long.Parse(issuedAt));
            }
        }
    }
}
