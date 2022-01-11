using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class Secret : ISecret
    {
        public DateTime? Expiration { get ; set; }

        public string Value { get ; set; }

        public string? Description { get; set; }

        public Secret(string value)
        {
            Value = value;
        }
    }
}
