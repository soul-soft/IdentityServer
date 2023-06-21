using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace IdentityServer.Hosting.DependencyInjection
{
    public static class InMemoryStoreIdentityServerBuilderExtensions
    {
        #region Fields
        private static readonly List<Client> _clients = new List<Client>();
        private static readonly List<IResource> _resources = new List<IResource>();
        private static readonly List<SigningCredentials> _credentials = new List<SigningCredentials>();
        #endregion

        #region SigningCredentials
        public static IIdentityServerBuilder AddInMemorySigningCredentials(this IIdentityServerBuilder builder, SigningCredentials credential)
        {
            _credentials.Add(credential);
            builder.AddSigningCredentialsStore(sp => 
            {
                return new SigningCredentialsStore(_credentials);
            });
            return builder;
        }

        public static IIdentityServerBuilder AddInMemorySigningCredentials(this IIdentityServerBuilder builder, SecurityKey securityKey, string signingAlgorithm = SecurityAlgorithms.RsaSha256)
        {
            var credential = new SigningCredentials(securityKey, signingAlgorithm);
            builder.AddInMemorySigningCredentials(credential);
            return builder;
        }

        public static IIdentityServerBuilder AddInMemorySigningCredentials(this IIdentityServerBuilder builder, X509Certificate2 certificate, string signingAlgorithm = SecurityAlgorithms.RsaSha256)
        {
            if (!certificate.HasPrivateKey)
            {
                throw new InvalidOperationException("X509 certificate does not have a private key.");
            }
            var securityKey = new X509SecurityKey(certificate);
            securityKey.KeyId += signingAlgorithm;
            var credential = new SigningCredentials(securityKey, signingAlgorithm);
            builder.AddInMemorySigningCredentials(credential);
            return builder;
        }

        public static IIdentityServerBuilder AddInMemoryDeveloperSigningCredentials(this IIdentityServerBuilder builder, bool persistKey = true, string? filename = null, string signingAlgorithm = SecurityAlgorithms.RsaSha256)
        {
            if (filename == null)
            {
                filename = Path.Combine(Directory.GetCurrentDirectory(), "idsvr.jwk");
            }
            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                var jwk = new JsonWebKey(json);
                builder.AddInMemorySigningCredentials(jwk, jwk.Alg);
                return builder;
            }
            else
            {
                var key = CryptoUtility.CreateRsaSecurityKey();
                var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
                jwk.Alg = signingAlgorithm.ToString();
                if (persistKey)
                {
                    File.WriteAllText(filename, JsonSerializer.Serialize(jwk));
                }
                builder.AddInMemorySigningCredentials(jwk, signingAlgorithm);
                return builder;
            }
        }
        #endregion

        #region Clients
        public static IIdentityServerBuilder AddInMemoryClients(this IIdentityServerBuilder builder, IEnumerable<Client> clients)
        {
            _clients.AddRange(clients);
            builder.AddClientStore(sp => 
            {
                return new ClientStore(clients);
            });
            return builder;
        }
        #endregion

        #region Resources
        public static IIdentityServerBuilder AddInMemoryResources(this IIdentityServerBuilder builder, IEnumerable<IResource> resources)
        {
            _resources.AddRange(resources);
            builder.AddResourceStore(sp => 
            {
                return new ResourceStore(new Resources(_resources));
            });
            return builder;
        }
        #endregion
    }
}
