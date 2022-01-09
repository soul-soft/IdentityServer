using IdentityServer.Infrastructure;

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
            var data = new Dictionary<string, object?>();
            data.Add("id_token", IdentityToken);
            data.Add("access_token", AccessToken);
            data.Add("refresh_token", RefreshToken);
            data.Add("expires_in", AccessTokenLifetime);
            data.Add("scope", Scope);
            return ObjectSerializer.SerializeObject(data);
        }
    }
}
