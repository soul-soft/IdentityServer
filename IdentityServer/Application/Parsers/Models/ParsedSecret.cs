using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class ParsedSecret
    {
        public string Id { get; set; }
        public object? Credential { get; set; }
        public string Type { get; set; }

        public ParsedSecret(string id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
