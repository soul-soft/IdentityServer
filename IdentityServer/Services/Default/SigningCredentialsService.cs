using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services
{
    internal class SigningCredentialsService : ISigningCredentialsService
    {
        private readonly ISigningCredentialsStore _credentials;

        public SigningCredentialsService(
            ISigningCredentialsStore credentials)
        {
            _credentials = credentials;
        }

        public async Task<IEnumerable<JsonWebKey>> GetJsonWebKeysAsync()
        {
            var credentials = await _credentials.GetSigningCredentialsAsync();
            return credentials.Select(credentials =>
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
        }

        public async Task<IEnumerable<SecurityKey>> GetSecurityKeysAsync()
        {
            var credentials = await _credentials.GetSigningCredentialsAsync();
            return credentials.Select(s => s.Key);
        }

        public Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync()
        {
            return _credentials.GetSigningCredentialsAsync();
        }

        public async Task<SigningCredentials> FindByAlgorithmsAsync(IEnumerable<string> algorithms)
        {
            var credentials = await _credentials.GetSigningCredentialsAsync();
            var signingCredentials = credentials
                .Where(a => algorithms.Contains(a.Algorithm))
                .FirstOrDefault() ?? credentials.First();
            return signingCredentials;
        }
    }
}
