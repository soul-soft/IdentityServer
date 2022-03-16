using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    /// <summary>
    /// 凭据解析器集
    /// </summary>
    public interface ISecretListParser
    {
        Task<ParsedSecret> ParseAsync(HttpContext context);
        IEnumerable<string> GetSecretParserTypes();
    }
}
