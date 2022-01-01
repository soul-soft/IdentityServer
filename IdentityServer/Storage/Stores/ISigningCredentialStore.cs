using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    public interface ISigningCredentialStore
    {
        /// <summary>
        /// 获取密钥信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SecurityKeyInfo>> GetSecurityKeyInfosAsync();
        /// <summary>
        /// 获取所有签名证书
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SigningCredentials>> GetAllSigningCredentialsAsync();
    }
}
