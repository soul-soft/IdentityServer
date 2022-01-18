using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class Client : IClient
    {
        public string ClientId { get; }
        public string? ClientName { get; set; }
        public string? Description { get; set; }
        public string? ClientUri { get; set; }
        public bool Enabled { get; set; } = true;
        public int AccessTokenLifetime { get; set; } = 3600;
        public int RefreshTokenLifetime { get; set; } = 3600 * 24 * 30;
        public int IdentityTokenLifetime { get; set; } = 300;
        public bool RequireClientSecret { get; set; } = true;
        public bool IncludeJwtId { get; set; } = true;
        public IReadOnlyCollection<ISecret> ClientSecrets { get; set; } = new HashSet<ISecret>();
        public IReadOnlyCollection<string> AllowedGrantTypes { get; set; } = new HashSet<string>();
        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;
        public IReadOnlyCollection<string> AllowedSigningAlgorithms { get; set; } = new HashSet<string>();
        public IReadOnlyCollection<string> AllowedScopes { get; set; } = new HashSet<string>();

        public Client(string clientId)
        {
            ClientId = clientId;
        }
    }
}
