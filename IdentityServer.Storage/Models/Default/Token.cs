namespace IdentityServer.Models
{
    public class Token : IToken
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string? Issuer { get; set; } 

        public string? ClientId { get; set; }

        public int Lifetime { get; set; }

        public string? Nonce { get; set; }

        public string? SubjectId { get; set; }

        public string? SessionId { get; set; }

        public AccessTokenType AccessTokenType { get; set; }

        public string? Description { get; set; }

        public string? GrantType { get; set; } 

        public DateTime CreationTime { get; set; }
        
        public ICollection<string> Scopes { get; set; } = new HashSet<string>();
       
        public ICollection<IClaimLite> Claims { get; set; } = new List<IClaimLite>();

        public ICollection<string> Audiences { get; set; } = new HashSet<string>();

        public ICollection<string> AllowedSigningAlgorithms { get; set; } = new HashSet<string>();

        public Token(string id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
