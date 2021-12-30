using IdentityServer.Models;
using IdentityServer.Storage.Stores;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage.InMemory
{
    public class InMemorySigningCredentialsStore
        : ISigningCredentialStore
    {
        private readonly IEnumerable<SigningCredentials> _credentials;

        public InMemorySigningCredentialsStore(IEnumerable<SigningCredentials> credentials)
        {
            _credentials = credentials;
        }

        public Task<IEnumerable<SecurityKeyInfo>> GetSecurityKeysAsync()
        {
            var result = _credentials.Select(credential =>
            {
                return new SecurityKeyInfo(credential.Key, credential.Algorithm);
            });
            return Task.FromResult(result);
        }

        public Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync()
        {
            return Task.FromResult(_credentials);
        }
    }
}
