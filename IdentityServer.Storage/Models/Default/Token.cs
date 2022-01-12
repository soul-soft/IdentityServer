namespace IdentityServer.Models
{
    public class Token : IToken
    {
        public string Id { get; set; }
        public string Type { get;  }
        public string Issuer { get;  }

        public string ClientId { get;  }

        public int Lifetime { get; set; }

        public string? Nonce { get; set; }

        public string? SubjectId { get; set; }

        public string? SessionId { get; set; }

        public AccessTokenType AccessTokenType { get; set; }

        public string? Description { get; set; }

        public ICollection<string> Scopes { get; set; } = new HashSet<string>();

        public ICollection<string> Audiences { get; set; } = new HashSet<string>();

        public ICollection<string> AllowedSigningAlgorithms { get; set; } = new HashSet<string>();

        public Token(string id,string issuer, string type, string clientId)
        {
            Id = id;
            Issuer = issuer;
            Type = type;
            ClientId = clientId;
        }
    }
}
