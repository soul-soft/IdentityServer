using IdentityServer.Models;

namespace IdentityServer.Application
{
    /// <summary>
    /// 密钥凭据验证器
    /// </summary>
    public interface ISecretsListValidator
    {
        Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret);
    }
}
