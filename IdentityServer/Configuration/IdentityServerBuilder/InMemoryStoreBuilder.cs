using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer.Configuration
{
    public class InMemoryStoreBuilder
    {
        #region fields
        private readonly List<IClient> Clients = new List<IClient>();
        private readonly List<IResource> Resources = new List<IResource>();
        private readonly List<SigningCredentialsDescriptor> SigningCredentials = new List<SigningCredentialsDescriptor>();
        #endregion

        #region SigningCredentials
        public InMemoryStoreBuilder AddSigningCredentials(SecurityKey securityKey, string signingAlgorithm = SecurityAlgorithms.RsaSha256)
        {
            SigningCredentials.Add(new SigningCredentialsDescriptor(new SigningCredentials(securityKey, signingAlgorithm), signingAlgorithm));
            return this;
        }
        public InMemoryStoreBuilder AddSigningCredentials(X509Certificate2 certificate, string signingAlgorithm = SecurityAlgorithms.RsaSha256)
        {
            if (!certificate.HasPrivateKey)
            {
                throw new InvalidOperationException("X509 certificate does not have a private key.");
            }
            var securityKey = new X509SecurityKey(certificate);
            securityKey.KeyId += signingAlgorithm;
            SigningCredentials.Add(new SigningCredentialsDescriptor(new SigningCredentials(securityKey, signingAlgorithm), signingAlgorithm));
            return this;
        }
        #endregion

        #region Resource
        public InMemoryStoreBuilder AddResources(IEnumerable<IResource> scopes)
        {
            Resources.AddRange(scopes);
            return this;
        }
        #endregion

        #region Client
        public InMemoryStoreBuilder AddClients(IEnumerable<IClient> clients)
        {
            Clients.AddRange(clients);
            return this;
        }
        #endregion

        #region build
        internal void Build(IIdentityServerBuilder services)
        {
            if (Clients.Any())
            {
                services.AddClientStore(sp =>
                {
                    return new InMemoryClientStore(Clients);
                });
            }
            if (Resources.Any())
            {
                services.AddResourceStore(sp =>
                {
                    return new InMemoryResourceStore(new Resources(Resources));
                });
            }
            if (SigningCredentials.Any())
            {
                services.AddSigningCredentialStore(sp =>
                {
                    return new InMemorySigningCredentialStore(SigningCredentials);
                });
            }
        }
        #endregion
    }
}
