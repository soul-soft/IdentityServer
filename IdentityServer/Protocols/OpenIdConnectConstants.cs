using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Protocols
{
    public static class OpenIdConnectConstants
    {
        public class TokenTypes
        {
            public const string AccessToken = "access_token";
            public const string IdentityToken = "id_token";
            public const string RefreshToken = "refresh_token";
        }
       
        public static class ParsedSecretTypes
        {
            public const string NoSecret = "NoSecret";
            public const string SharedSecret = "SharedSecret";
            public const string X509Certificate = "X509Certificate";
            public const string JwtBearer = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";
        }

        public static class EndpointAuthenticationMethods
        {
            public const string PostBody = "client_secret_post";
            public const string BasicAuthentication = "client_secret_basic";
            public const string PrivateKeyJwt = "private_key_jwt";
            public const string TlsClientAuth = "tls_client_auth";
            public const string SelfSignedTlsClientAuth = "self_signed_tls_client_auth";
        }
    }
}
