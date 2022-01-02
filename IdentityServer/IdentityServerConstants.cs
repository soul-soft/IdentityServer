using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
    public static class IdentityServerConstants
    {
        public const string LocalIdentityProvider = "local";
        public const string DefaultCookieAuthenticationScheme = "idsrv";
        public const string SignoutScheme = "idsrv";
        public const string ExternalCookieAuthenticationScheme = "idsrv.external";
        public static IEnumerable<string> SupportedSigningAlgorithms = new List<string>
        {
            SecurityAlgorithms.RsaSha256,
            SecurityAlgorithms.RsaSha384,
            SecurityAlgorithms.RsaSha512,

            SecurityAlgorithms.RsaSsaPssSha256,
            SecurityAlgorithms.RsaSsaPssSha384,
            SecurityAlgorithms.RsaSsaPssSha512,

            SecurityAlgorithms.EcdsaSha256,
            SecurityAlgorithms.EcdsaSha384,
            SecurityAlgorithms.EcdsaSha512
        };

        public static class ClaimValueTypes
        {
            public const string Json = "json";
        }
      
        public enum RsaSigningAlgorithm
        {
            RS256,
            RS384,
            RS512,

            PS256,
            PS384,
            PS512
        }

        public enum ECDsaSigningAlgorithm
        {
            ES256,
            ES384,
            ES512
        }

        public static class ParsedSecretTypes
        {
            public const string NoSecret = "NoSecret";
            public const string SharedSecret = "SharedSecret";
            public const string X509Certificate = "X509Certificate";
            public const string JwtBearer = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";
        }

        public static class SecretTypes
        {
            public const string SharedSecret = "SharedSecret";
            public const string X509CertificateThumbprint = "X509Thumbprint";
            public const string X509CertificateName = "X509Name";
            public const string X509CertificateBase64 = "X509CertificateBase64";
            public const string JsonWebKey = "JWK";
        }
    }
}
