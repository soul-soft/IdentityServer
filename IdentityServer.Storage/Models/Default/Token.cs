namespace IdentityServer.Models
{
    public class Token : IToken
    {
        public string Issuer { get;  }

        public string Type { get;  }

        public string ClientId { get;  }

        public string? JwtId { get; set; }

        public int? Lifetime { get; set; }

        public string? Nonce { get; set; }

        public string? SubjectId { get; set; }

        public string? SessionId { get; set; }

        public AccessTokenType AccessTokenType { get; set; }

        public string? Description { get; set; }

        public ICollection<string> Scopes { get; set; } = new HashSet<string>();

        public ICollection<string> Audiences { get; set; } = new HashSet<string>();

        public DateTime CreationTime { get; set; }

        public ICollection<string> AllowedSigningAlgorithms { get; set; } = new HashSet<string>();

        public Token(string issuer, string type, string clientId)
        {
            Issuer = issuer;
            Type = type;
            ClientId = clientId;
        }
    }
}
