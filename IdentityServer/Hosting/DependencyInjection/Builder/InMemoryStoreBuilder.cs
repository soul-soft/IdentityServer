using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public class InMemoryStoreBuilder
    {
        #region fields
        private readonly List<Client> _clients = new List<Client>();
        private readonly List<IResource> _resources = new List<IResource>();
        private readonly List<SigningCredentials> _signingCredentials = new List<SigningCredentials>();
        #endregion

        #region SigningCredentials
        public InMemoryStoreBuilder AddSigningCredentials(SigningCredentials credential)
        {
            _signingCredentials.Add(credential);
            return this;
        }
        public InMemoryStoreBuilder AddSigningCredentials(SecurityKey securityKey, string signingAlgorithm = SecurityAlgorithms.RsaSha256)
        {
            var credential = new SigningCredentials(securityKey, signingAlgorithm);
            AddSigningCredentials(credential);
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
            var credential = new SigningCredentials(securityKey, signingAlgorithm);
            AddSigningCredentials(credential);
            return this;
        }
        public InMemoryStoreBuilder AddDeveloperSigningCredentials(bool persistKey = true,string? filename = null,string signingAlgorithm = SecurityAlgorithms.RsaSha256)
        {
            if (filename == null)
            {
                filename = Path.Combine(Directory.GetCurrentDirectory(), "idsvr.jwk");
            }
            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                var jwk = new JsonWebKey(json);
                AddSigningCredentials(jwk, jwk.Alg);
                return this;
            }
            else
            {
                var key = CryptoRandom.CreateRsaSecurityKey();
                var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
                jwk.Alg = signingAlgorithm.ToString();
                if (persistKey)
                {
                    File.WriteAllText(filename, JsonSerializer.Serialize(jwk));
                }
                AddSigningCredentials(key, signingAlgorithm);
                return this;
            }
        }
        #endregion

        #region Resource
        public InMemoryStoreBuilder AddResource(IResource resources)
        {
            _resources.Add(resources);
            return this;
        }
        public InMemoryStoreBuilder AddResources(IEnumerable<IResource> resources)
        {
            _resources.AddRange(resources);
            return this;
        }
        #endregion

        #region Client
        public InMemoryStoreBuilder AddClient(Client client)
        {
            _clients.Add(client);
            return this;
        }
        public InMemoryStoreBuilder AddClients(IEnumerable<Client> clients)
        {
            _clients.AddRange(clients);
            return this;
        }
        #endregion

        #region build
        internal void Build(IIdentityServerBuilder services)
        {
            services.AddClientStore(sp =>
            {
                return new ClientStore(_clients);
            });
            services.AddResourceStore(sp =>
            {
                return new ResourceStore(new Resources(_resources));
            });
            services.AddSigningCredentialsStore(sp =>
            {
                return new SigningCredentialsStore(_signingCredentials);
            });
            services.AddCacheStore<CacheStore>();
            services.AddTokenStore<TokenStore>();
            services.AddAuthorizeCodeStore<AuthorizeCodeStore>();
        }
        #endregion
    }
}
