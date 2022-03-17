using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class TokenResponse
    {
        public string? IdentityToken { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int? TokenLifetime { get; set; }
        public string? Scope { get; set; }

        public string Serialize()
        {
            var token = new
            {
                id_token = IdentityToken,
                access_token = AccessToken,
                expires_in = TokenLifetime,
                refresh_token = RefreshToken,
                scope = Scope,
            };
            return ObjectSerializer.Serialize(token);
        }
    }
}
