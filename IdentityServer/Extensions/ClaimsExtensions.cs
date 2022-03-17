using System.Security.Claims;
using System.Text.Json;

namespace IdentityServer.Extensions
{
    public static class ClaimsExtensions
    {
        public static Dictionary<string, object> ToClaimsDictionary(this IEnumerable<Claim> claims)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var grouping in claims.GroupBy(c => c.Type))
            {
                var list = new List<object>();
                foreach (var item in grouping)
                {
                    list.Add(ParseValue(item));
                }
                if (list.Count == 1)
                {
                    dictionary.Add(grouping.Key, list.First());
                }
                else
                {
                    dictionary.Add(grouping.Key, list);
                }
            }
            return dictionary;
        }

        private static object ParseValue(Claim claim)
        {
            if (claim.ValueType == ClaimValueTypes.Integer || claim.ValueType == ClaimValueTypes.Integer32)
            {
                if (int.TryParse(claim.Value, out int value))
                {
                    return value;
                }
            }
            if (claim.ValueType == ClaimValueTypes.UInteger32)
            {
                if (uint.TryParse(claim.Value, out uint value))
                {
                    return value;
                }
            }
            if (claim.ValueType == ClaimValueTypes.Integer64)
            {
                if (long.TryParse(claim.Value, out long value))
                {
                    return value;
                }
            }
            if (claim.ValueType == ClaimValueTypes.UInteger64)
            {
                if (ulong.TryParse(claim.Value, out ulong value))
                {
                    return value;
                }
            }
            if (claim.ValueType == ClaimValueTypes.Double)
            {
                if (double.TryParse(claim.Value, out double value))
                {
                    return value;
                }
            }
            if (claim.ValueType == ClaimValueTypes.Boolean)
            {
                if (bool.TryParse(claim.Value, out bool value))
                {
                    return value;
                }
            }
            if (claim.ValueType == "json")
            {
                return JsonSerializer.Deserialize<JsonElement>(claim.Value);
            }
            return claim.Value;
        }
    }
}
