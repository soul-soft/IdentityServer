namespace IdentityServer.Protocols
{
    public class JwtClaimTypes
    {
        public const string JwtId = "jti";
        public const string Issuer = "iss";
        public const string IssuedAt = "iat";
        public const string SessionId = "sid";
        public const string Audience = "aud";
        public const string Subject = "sub";
        public const string Nonce = "nonce";
        public const string Scope = "scope";
        public const string ClientId = "client_id";
        public const string IdentityProvider = "idp";
        public const string AccessTokenHash = "at_hash";
        public const string Expiration = "exp";
        public const string NotBefore = "nbf";
        public const string AuthenticationMethod = "amr";
        public const string Confirmation = "cnf";
        public const string AuthorizationCodeHash = "c_hash";
        public const string AuthorizedParty = "azp";
        public const string AuthenticationTime = "auth_time";
        public const string ReferenceTokenId = "reference_token_id";
    }
}
