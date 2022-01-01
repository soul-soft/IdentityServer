using System.Security.Cryptography.X509Certificates;
using IdentityServer.Infrastructure;
using IdentityServer.Storage.InMemory;
using IdentityServer.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Configuration.DependencyInjection
{
    public class IdentityServerSigningCredentialsBuilder
    {
        private readonly IServiceCollection _services;

        private readonly List<SigningCredentials> _credentials = new();

        public IdentityServerSigningCredentialsBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IdentityServerSigningCredentialsBuilder AddCredential(SigningCredentials credentials)
        {
            _credentials.Add(credentials);
            return this;
        }

        public IdentityServerSigningCredentialsBuilder AddCredential(X509Certificate2 certificate, string signingAlgorithm = SecurityAlgorithms.RsaSha256)
        {
            if (certificate == null) throw new ArgumentNullException(nameof(certificate));

            if (!certificate.HasPrivateKey)
            {
                throw new InvalidOperationException("X509 certificate does not have a private key.");
            }

            var key = new X509SecurityKey(certificate);
            key.KeyId += signingAlgorithm;

            var credential = new SigningCredentials(key, signingAlgorithm);
            return AddCredential(credential);
        }

        public IdentityServerSigningCredentialsBuilder AddCredential(SecurityKey key, string signingAlgorithm)
        {
            var credential = new SigningCredentials(key, signingAlgorithm);
            return AddCredential(credential);
        }
       
        public IdentityServerSigningCredentialsBuilder AddCredential(RsaSecurityKey key, IdentityServerConstants.RsaSigningAlgorithm signingAlgorithm)
        {
            var credential = new SigningCredentials(key, CryptoHelper.GetRsaSigningAlgorithmValue(signingAlgorithm));
            return AddCredential(credential);
        }
       
        public IdentityServerSigningCredentialsBuilder AddDeveloperCredential(bool persistKey = true,string? filename = null, IdentityServerConstants.RsaSigningAlgorithm signingAlgorithm = IdentityServerConstants.RsaSigningAlgorithm.RS256)
        {
            if (filename == null)
            {
                filename = Path.Combine(Directory.GetCurrentDirectory(), "tempkey.jwk");
            }

            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                var jwk = new JsonWebKey(json);
                return AddCredential(jwk, jwk.Alg);
            }
            else
            {
                var key = CryptoHelper.CreateRsaSecurityKey();
                var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
                jwk.Alg = signingAlgorithm.ToString();

                if (persistKey)
                {
                    File.WriteAllText(filename, ObjectSerializer.SerializeObject(jwk));
                }

                return AddCredential(key, signingAlgorithm);
            }
        }
      
        internal void Build()
        {
            _services.AddSingleton<ISigningCredentialStore>(new InMemorySigningCredentialsStore(_credentials));
        }
    }
}
