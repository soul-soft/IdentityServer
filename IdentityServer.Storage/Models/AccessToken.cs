namespace IdentityServer.Models
{
    public class AccessToken
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        public string? Issuer { get; set; }
        public string? ClientId { get; set; }
        public int Lifetime { get; set; }
        public string? Nonce { get; set; }
        public string? SubjectId { get; set; }
        public string? SessionId { get; set; }
        public string? Description { get; set; }
        public string? GrantType { get; set; }
        public DateTime? NotBefore { get; set; }
        public DateTime? Expiration { get; set; }
        public AccessTokenType AccessTokenType { get; set; }
        public IReadOnlyCollection<string> Scopes { get; set; } = new HashSet<string>();
        public IReadOnlyCollection<Profile> Profiles { get; set; } = new List<Profile>();
        public IReadOnlyCollection<string> Audiences { get; set; } = new HashSet<string>();
        public IReadOnlyCollection<string> AllowedSigningAlgorithms { get; set; } = new HashSet<string>();
    }
}
