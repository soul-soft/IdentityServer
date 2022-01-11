using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface ISecretParserProvider
    {
        ISecretParser GetParser();
        IEnumerable<string> GetAuthenticationMethods();
    }
}
