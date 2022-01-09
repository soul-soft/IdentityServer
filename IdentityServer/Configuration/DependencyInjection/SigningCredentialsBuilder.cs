using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Configuration
{
    public class SigningCredentialsBuilder
    {
        private readonly List<SigningCredentialsDescriptor> _descriptors = new List<SigningCredentialsDescriptor>();

        public SigningCredentialsBuilder AddSigningCredentials(SecurityKey securityKey, string signingAlgorithm)
        {
            _descriptors.Add(new SigningCredentialsDescriptor(new SigningCredentials(securityKey, signingAlgorithm), signingAlgorithm));
            return this;
        }

        internal IEnumerable<SigningCredentialsDescriptor> Build()
        {
            return _descriptors;
        }
    }
}
