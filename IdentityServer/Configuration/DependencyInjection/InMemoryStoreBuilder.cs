using IdentityServer.Models;
using IdentityServer.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Configuration
{
    public class InMemoryStoreBuilder
    {
        #region fields
        private readonly List<IClient> Clients = new List<IClient>();
        private readonly List<IResource> Resources = new List<IResource>();
        private readonly List<SigningCredentialsInfo> SigningCredentials = new List<SigningCredentialsInfo>();
        #endregion

        #region SigningCredentials
        public InMemoryStoreBuilder AddSigningCredentials(SecurityKey securityKey, string signingAlgorithm)
        {
            SigningCredentials.Add(new SigningCredentialsInfo(new SigningCredentials(securityKey, signingAlgorithm), signingAlgorithm));
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
            services.AddClientStore(sp =>
            {
                return new InMemoryClientStore(Clients);
            });
            services.AddResourceStore(sp =>
            {
                return new InMemoryResourceStore(new Resources(Resources));
            });
            services.AddSigningCredentialStore(sp =>
            {
                return new InMemorySigningCredentialStore(SigningCredentials);
            });
        }
        #endregion
    }
}
