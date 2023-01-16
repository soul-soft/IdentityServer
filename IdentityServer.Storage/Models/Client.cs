namespace IdentityServer.Models
{
    public class Client
    {
        public string ClientId { get; set; } = null!;
        public string? ClientName { get; set; }
        public string? Description { get; set; }
        public string? ClientUri { get; set; }
        public bool Enabled { get; set; } = true;
        public int AuthorizeCodeLifetime { get; set; } = 60;
        public int AccessTokenLifetime { get; set; } = 3600;
        public int RefreshTokenLifetime { get; set; } = 3600 * 24 * 30;
        public int IdentityTokenLifetime { get; set; } = 300;
        public bool RequireClientSecret { get; set; } = true;
        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;
        public ICollection<Secret> ClientSecrets { get; set; } = new HashSet<Secret>();
        public ICollection<string> AllowedGrantTypes { get; set; } = new HashSet<string>();
        public ICollection<string> AllowedSigningAlgorithms { get; set; } = new HashSet<string>();
        public ICollection<string> AllowedScopes { get; set; } = new HashSet<string>();
    }
}
