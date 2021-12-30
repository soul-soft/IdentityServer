using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Models
{
    public class SecurityKeyInfo
    {
        /// <summary>
        /// The key
        /// </summary>
        public SecurityKey Key { get; set; }

        /// <summary>
        /// The signing algorithm
        /// </summary>
        public string SigningAlgorithm { get; set; }

        public SecurityKeyInfo(SecurityKey key, string signingAlgorithm)
        {
            Key = key;
            SigningAlgorithm = signingAlgorithm;
        }
    }
}
