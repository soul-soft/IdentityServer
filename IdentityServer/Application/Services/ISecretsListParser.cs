using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface ISecretsListParser
    {
        Task<ParsedSecret> TryParseAsync(HttpContext context);
        IEnumerable<string> GetAuthenticationMethods();
    }
}
