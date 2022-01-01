namespace IdentityServer.Models
{
    public class Client
    {
        public string ClientId { get; set; } = null!;
        public string? ClientSecret { get; set; }
        public ICollection<string> AllowedIdentityTokenSigningAlgorithms { get; set; } = new HashSet<string>();
        public bool IncludeJwtId { get;  set; } = true;
        public int AccessTokenLifetime { get;  set; } = 3600;
        public int IdentityTokenLifetime { get; set; } = 3600;
        public int? ReferenceTokenLifetime { get; set; } = 3600;
        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;
    }
}
