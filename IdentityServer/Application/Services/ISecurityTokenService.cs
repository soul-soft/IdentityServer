using IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateTokenAsync(IToken token);
    }
}
