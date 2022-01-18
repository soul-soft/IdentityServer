using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class TokenResponse
    {
        public string? IdentityToken { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int? AccessTokenLifetime { get; set; }
        public string? Scope { get; set; }
        public string Serialize()
        {
            var token = new
            {
                id_token = IdentityToken,
                access_token = AccessToken,
                refresh_token = RefreshToken,
                expires_in = AccessTokenLifetime,
                scope = Scope
            };
            return ObjectSerializer.Serialize(token);
        }
    }
}
