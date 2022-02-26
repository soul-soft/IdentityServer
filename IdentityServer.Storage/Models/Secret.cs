using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class Secret
    {
        public DateTime? Expiration { get ; set; }

        public string Value { get; set; } = null!;

        public string? Description { get; set; }

        public Secret()
        {

        }

        public Secret(string value)
        {
            Value = value;
        }
    }
}
