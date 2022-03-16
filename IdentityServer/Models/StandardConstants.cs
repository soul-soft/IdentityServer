namespace IdentityServer.Models
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

    public static class SecretParserTypes
    {
        public const string PostBody = "client_secret_post";
        public const string BasicAuthentication = "client_secret_basic";
        public const string PrivateKeyJwt = "private_key_jwt";
        public const string TlsClientAuth = "tls_client_auth";
        public const string SelfSignedTlsClientAuth = "self_signed_tls_client_auth";
    }

    public static class StandardResources
    {
        public static IdentityResource OpenId => new IdentityResource(StandardScopes.OpenId)
        {
            DisplayName = "Your user identifier",
            Required = true,
            ClaimTypes = new string[]
            {
                JwtClaimTypes.Subject,
            }
        };

        public static IdentityResource Profile => new IdentityResource(StandardScopes.Profile)
        {
            DisplayName = "User profile",
            ClaimTypes = new string[]
            {
                JwtClaimTypes.NickName,
                JwtClaimTypes.Profile,
                JwtClaimTypes.Picture,
                JwtClaimTypes.Gender,
                JwtClaimTypes.BirthDate,
            }
        };

        public static IdentityResource Address => new IdentityResource(StandardScopes.Address)
        {
            DisplayName = "User address",
            ClaimTypes = new string[]
            {
                JwtClaimTypes.Address,
            }
        };

        public static IdentityResource Email => new IdentityResource(StandardScopes.Email)
        {
            DisplayName = "User email",
            ClaimTypes = new string[]
            {
                JwtClaimTypes.Email,
            }
        };

        public static IdentityResource Phone => new IdentityResource(StandardScopes.Phone)
        {
            DisplayName = "User phone",
            ClaimTypes = new string[]
            {
                JwtClaimTypes.Phone,
            }
        };

        public static IdentityResource OfflineAccess => new IdentityResource(StandardScopes.OfflineAccess)
        {
            DisplayName = "Offline access",
        };
    }

    public static class ProfileDataCallers
    {
        public const string IdentityToken = "ClaimsProviderIdentityToken";
        public const string AccessToken = "ClaimsProviderAccessToken";
        public const string UserInfoEndpoint = "UserInfoEndpoint";
    }

    public static class ProfileIsActiveCallers
    {
        public const string AuthorizeEndpoint = "AuthorizeEndpoint";
        public const string IdentityTokenValidation = "IdentityTokenValidation";
        public const string AccessTokenValidation = "AccessTokenValidation";
        public const string ResourceOwnerValidation = "ResourceOwnerValidation";
        public const string ExtensionGrantValidation = "ExtensionGrantValidation";
        public const string RefreshTokenValidation = "RefreshTokenValidation";
        public const string AuthorizationCodeValidation = "AuthorizationCodeValidation";
        public const string UserInfoRequestValidation = "UserInfoRequestValidation";
        public const string DeviceCodeValidation = "DeviceCodeValidation";
    }
}
