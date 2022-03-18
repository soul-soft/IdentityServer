using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    /// <summary>
    /// 凭据解析器
    /// </summary>
    public interface ISecretParser
    {
        string AuthenticationMethod { get; }
        Task<ParsedSecret> ParseAsync(HttpContext context);
    }
}
