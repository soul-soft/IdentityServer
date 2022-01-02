using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    /// <summary>
    /// 密钥集解析器组
    /// </summary>
    public interface ISecretsListParser
    {
        Task<ParsedSecret?> ParseAsync(HttpContext context);
        IEnumerable<string> GetAuthenticationMethods();
    }
}
