using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    internal class SigningCredentialsStore : ISigningCredentialsStore
    {
        private readonly IEnumerable<SigningCredentials> _singingCredentials;

        public SigningCredentialsStore(IEnumerable<SigningCredentials> descriptor)
        {
            _singingCredentials = descriptor;
        }

        public Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync()
        {
            return Task.FromResult(_singingCredentials);
        }
    }
}
