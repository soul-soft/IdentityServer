using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage.InMemory
{
    public class InMemorySigningCredentialsStore
        : ISigningCredentialStore
    {
        private readonly IEnumerable<SecurityKeyInfo> _securityKeys;
      
        private readonly IEnumerable<SigningCredentials> _credentials;

        public InMemorySigningCredentialsStore(IEnumerable<SigningCredentials> credentials)
        {
            _credentials = credentials;
            _securityKeys = _credentials
                .Select(credential => new SecurityKeyInfo(credential.Key, credential.Algorithm));
        }

        public Task<IEnumerable<SecurityKeyInfo>> GetSecurityKeysAsync()
        {
            return Task.FromResult(_securityKeys);
        }

        public Task<SigningCredentials?> GetSigningCredentialsAsync()
        {
            return Task.FromResult(_credentials.FirstOrDefault());
        }

        public Task<IEnumerable<SigningCredentials>> GetAllSigningCredentialsAsync()
        {
            return Task.FromResult(_credentials);
        }
    }
}
