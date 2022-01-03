using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class Token
    {
        public string Type { get; set; }
        public ICollection<string> AllowedSigningAlgorithms { get; set; } = new HashSet<string>();
        public string Issuer { get; set; }
        public int Lifetime { get; set; }
        public IEnumerable<string> Audiences { get; set; } = new HashSet<string>();
        public IEnumerable<Claim> Claims { get; set; }
        public string? Confirmation { get; set; }
        public DateTime CreationTime { get;  set; }
        public string ClientId { get;  set; }
        public string? Description { get;  set; }
        public AccessTokenType AccessTokenType { get; internal set; }
    }
}
