using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public class InMemorySigningCredentialStore : ISigningCredentialStore
    {
        private readonly IEnumerable<SigningCredentialsDescriptor> _descriptor;

        public InMemorySigningCredentialStore(IEnumerable<SigningCredentialsDescriptor> descriptor)
        {
            _descriptor = descriptor;
        }

        public Task<IEnumerable<SigningCredentialsDescriptor>> GetSigningCredentialsAsync()
        {
            return Task.FromResult(_descriptor);
        }

        public Task<SigningCredentialsDescriptor> GetSigningCredentialsByAlgorithmsAsync(string? algorithm)
        {
            if (algorithm == null)
            {
                return Task.FromResult(_descriptor.First());
            }

            var credential = _descriptor
                    .Where(a => a.SigningAlgorithm == algorithm)
                    .FirstOrDefault()
                    ?? _descriptor.First();

            return Task.FromResult(credential);
        }
    }
}
