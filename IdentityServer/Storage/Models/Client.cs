namespace IdentityServer.Models
{
    public class Client
    {
        public string ClientId { get; } = null!;
        public bool Enabled { get; } = true;
        public ICollection<Secret> ClientSecrets { get; } = new HashSet<Secret>();
        public ICollection<string> AllowedIdentityTokenSigningAlgorithms { get; set; } = new HashSet<string>();
        public bool IncludeJwtId { get; } = true;
        public int AccessTokenLifetime { get; } = 3600;
        public int IdentityTokenLifetime { get; } = 3600;
        public int? ReferenceTokenLifetime { get; } = 3600;
        public bool RequireClientSecret { get;  } = true;
        public AccessTokenType AccessTokenType { get; } = AccessTokenType.Jwt;
        public IReadOnlyCollection<string> AllowedScopes { get; } = new HashSet<string>();
        public IReadOnlyCollection<string> AllowedGrantTypes { get; } = new HashSet<string>();
    }
}
