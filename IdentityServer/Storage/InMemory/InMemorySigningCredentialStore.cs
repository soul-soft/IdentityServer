using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    public class InMemorySigningCredentialStore : ISigningCredentialStore
    {
        private readonly IEnumerable<SigningCredentials> _credentials;

        public InMemorySigningCredentialStore(IEnumerable<SigningCredentials> credentials)
        {
            _credentials = credentials;
        }

        public Task<IEnumerable<SecurityKey>> GetSigningKeysAsync()
        {
            var keys = _credentials.Select(s => s.Key);
            return Task.FromResult(keys);
        }

        public Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync()
        {
            return Task.FromResult(_credentials);
        }

        public Task<SigningCredentials> GetSigningCredentialsByAlgorithmsAsync(string? algorithm)
        {
            if (algorithm == null)
            {
                return Task.FromResult(_credentials.First());
            }

            var credential = _credentials
                    .Where(a => a.Algorithm == algorithm)
                    .FirstOrDefault()
                    ?? _credentials.First();

            return Task.FromResult(credential);
        }
    }
}
