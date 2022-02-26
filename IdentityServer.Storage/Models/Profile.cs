using System.Security.Claims;

namespace IdentityServer.Models
{
    public class Profile
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Profile(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public static implicit operator Claim(Profile profile)
        {
            var claimValueType = ClaimValueTypes.String;
            var type = profile.Value.GetType();
            if (type == typeof(bool))
            {
                claimValueType = ClaimValueTypes.Boolean;
            }
            else if (type == typeof(short))
            {
                claimValueType = ClaimValueTypes.Integer;
            }
            else if (type == typeof(int))
            {
                claimValueType = ClaimValueTypes.Integer32;
            }
            else if (type == typeof(uint))
            {
                claimValueType = ClaimValueTypes.UInteger32;
            }
            else if (type == typeof(long))
            {
                claimValueType = ClaimValueTypes.UInteger32;
            }
            else if (type == typeof(ulong))
            {
                claimValueType = ClaimValueTypes.UInteger64;
            }
            else if (type == typeof(float) || type == typeof(double))
            {
                claimValueType = ClaimValueTypes.Double;
            }
            else if (type == typeof(TimeOnly))
            {
                claimValueType = ClaimValueTypes.Time;
            }
            else if (type == typeof(DateTime))
            {
                claimValueType = ClaimValueTypes.DateTime;
            }
            string claimValue = profile.Value.ToString() ?? string.Empty;
            return new Claim(profile.Name, claimValue, claimValueType);
        }

        public static implicit operator Profile(Claim claim)
        {
            if (claim.ValueType == ClaimValueTypes.Boolean)
            {
                return new Profile(claim.Type, Convert.ToBoolean(claim.Value));
            }
            if (claim.ValueType == ClaimValueTypes.Integer)
            {
                return new Profile(claim.Type, Convert.ToInt16(claim.Value));
            }
            if (claim.ValueType == ClaimValueTypes.Integer32)
            {
                return new Profile(claim.Type, Convert.ToInt32(claim.Value));
            }
            if (claim.ValueType == ClaimValueTypes.UInteger32)
            {
                return new Profile(claim.Type, Convert.ToUInt32(claim.Value));
            }
            if (claim.ValueType == ClaimValueTypes.Integer64)
            {
                return new Profile(claim.Type, Convert.ToInt64(claim.Value));
            }
            if (claim.ValueType == ClaimValueTypes.UInteger64)
            {
                return new Profile(claim.Type, Convert.ToUInt64(claim.Value));
            }
            if (claim.ValueType == ClaimValueTypes.Double)
            {
                return new Profile(claim.Type, Convert.ToDouble(claim.Value));
            }
            if (claim.ValueType == ClaimValueTypes.Time)
            {
                return new Profile(claim.Type, TimeOnly.FromDateTime(Convert.ToDateTime(claim.Value)));
            }
            if (claim.ValueType == ClaimValueTypes.DateTime)
            {
                return new Profile(claim.Type, Convert.ToDateTime(claim.Value));
            }
            return new Profile(claim.Type, claim.Value);
        }
    }
}
