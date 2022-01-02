using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    /// <summary>
    /// 客户端及密钥验证
    /// </summary>
    public interface IClientSecretValidator
    {
        Task<ClientSecretValidationResult> ValidateAsync(HttpContext context);
    }
}
