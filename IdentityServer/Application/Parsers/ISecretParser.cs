using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    /// <summary>
    /// 密钥解析器
    /// </summary>
    public interface ISecretParser
    {
        Task<ParsedSecret?> ParseAsync(HttpContext context);
        string AuthenticationMethod { get; }
    }
}
