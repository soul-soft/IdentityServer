using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    internal class InMemorySigningCredentialStore : ISigningCredentialStore
    {
        private readonly IEnumerable<SigningCredentialsInfo> _descriptor;

        public InMemorySigningCredentialStore(IEnumerable<SigningCredentialsInfo> descriptor)
        {
            _descriptor = descriptor;
        }

        public Task<IEnumerable<JsonWebKey>> GetJsonWebKeysAsync()
        {
            var jwks = _descriptor.Select(descriptor =>
            {
                var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(descriptor.Key);
                jwk.Alg = descriptor.SigningAlgorithm;
                return jwk;
            });
            return Task.FromResult(jwks);
        }

        public Task<IEnumerable<SigningCredentialsInfo>> GetSigningCredentialsDescriptorAsync()
        {
            return Task.FromResult(_descriptor);
        }

        public Task<SigningCredentials> GetSigningCredentialsByAlgorithmsAsync(IEnumerable<string> algorithms)
        {
            if (algorithms.Count() == 0)
            {
                return Task.FromResult(_descriptor.First().SigningCredentials);
            }

            var credential = _descriptor
                    .Where(a => algorithms.Contains(a.SigningAlgorithm))
                    .FirstOrDefault()
                    ?? _descriptor.First();

            return Task.FromResult(credential.SigningCredentials);
        }
    }
}
