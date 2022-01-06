using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Application
{
    public class TokenResponse
    {
        public string? AccessToken { get; set; }
        public string? IdentityToken { get; set; }
        public int? ExpiresIn { get; set; }
        public string? RefreshToken { get; set; }
        public string? Scope { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
}
