using System.Security.Claims;

namespace IdentityServer.Authentication
{
    public static class ClaimsExtensions
    {
        public static IEnumerable<Claim> FromDictionary(this Dictionary<string, object> entities)
        {
            var claims = new List<Claim>();
            foreach (var item in entities)
            {
                var type = ClaimValueTypes.String;
                if (item.Value is int)
                {
                    type = ClaimValueTypes.Integer32;
                }
                if (item.Value is DateTime)
                {
                    type = ClaimValueTypes.DateTime;
                }
                var value = item.Value.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    claims.Add(new Claim(item.Key, value, type));
                }
            }
            return claims;
        }
    }
}
