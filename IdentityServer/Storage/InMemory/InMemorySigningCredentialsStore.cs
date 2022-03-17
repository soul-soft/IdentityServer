using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    internal class InMemorySigningCredentialsStore : ISigningCredentialStore
    {
        private readonly IEnumerable<SigningCredentials> _singingCredentials;

        public InMemorySigningCredentialsStore(IEnumerable<SigningCredentials> descriptor)
        {
            _singingCredentials = descriptor;
        }

        public Task<IEnumerable<SecurityKey>> GetSecurityKeysAsync()
        {
            var securityKeys = _singingCredentials.Select(s => s.Key);
            return Task.FromResult(securityKeys);
        }

        public Task<IEnumerable<JsonWebKey>> GetJsonWebKeysAsync()
        {
            var jwks = _singingCredentials.Select(credentials =>
            {
                JsonWebKey jwk;
                if (credentials.Key is JsonWebKey jsonWebKey)
                {
                    jwk = jsonWebKey;
                }
                else
                {
                    jwk = JsonWebKeyConverter.ConvertFromSecurityKey(credentials.Key);
                    jwk.Alg = credentials.Algorithm;
                }
                return jwk;
            });
            return Task.FromResult(jwks);
        }

        public Task<IEnumerable<SigningCredentials>> GetAllSigningCredentialsAsync()
        {
            return Task.FromResult(_singingCredentials);
        }

        public Task<SigningCredentials> GetSigningCredentialsByAlgorithmsAsync(IEnumerable<string> algorithms)
        {
            if (!algorithms.Any())
            {
                return Task.FromResult(_singingCredentials.First());
            }
            var signingCredentials = _singingCredentials
                .Where(a => algorithms.Contains(a.Algorithm))
                .FirstOrDefault()
                ?? _singingCredentials.First();
            return Task.FromResult(signingCredentials);
        }
    }
}
