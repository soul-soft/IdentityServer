namespace IdentityServer
{
    public static class OpenIdConnects
    {
        public static class TokenTypes
        {
            public const string AccessToken = "access_token";
            public const string IdentityToken = "id_token";
            public const string RefreshToken = "refresh_token";
        }
        public static class GrantTypes
        {
            public const string AuthorizationCode = "authorization_code";

            public const string RefreshToken = "refresh_token";

            public const string Password = "password";

            public const string ClientCredentials = "client_credentials";
        }
        public static class ClientSecretTypes
        {
            public const string NoSecret = "NoSecret";
            public const string SharedSecret = "SharedSecret";
            public const string X509Certificate = "X509Certificate";
            public const string JwtBearer = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";
        }
        public static class StandardScopes
        {
            public const string OpenId = "openid";
            public const string Profile = "profile";
            public const string Email = "email";
            public const string Address = "address";
            public const string Phone = "phone";
            public const string OfflineAccess = "offline_access";
        }
        public static class TokenEndpointAuthMethods
        {
            public const string PostBody = "client_secret_post";
            public const string BasicAuthentication = "client_secret_basic";
            public const string PrivateKeyJwt = "private_key_jwt";
            public const string TlsClientAuth = "tls_client_auth";
            public const string SelfSignedTlsClientAuth = "self_signed_tls_client_auth";
        }
    }
}
