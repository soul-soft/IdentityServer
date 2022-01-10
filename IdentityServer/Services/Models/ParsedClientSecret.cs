using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class ParsedClientSecret
    {
        public string Id { get; set; }
        public object? Credential { get; set; }
        public string Type { get; set; }

        public ParsedClientSecret(string id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
