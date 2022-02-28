using System.Security.Claims;

namespace IdentityServer.Storage.Serialization
{
    public class ClaimLite
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        public ClaimLite(string name, string value, string type)
        {
            Name = name;
            Value = value;
            Type = ConvertType(type);
        }

        private static string ConvertType(string type)
        {
            return type switch
            {
                ClaimValueTypes.Base64Binary => nameof(ClaimValueTypes.Base64Binary),
                ClaimValueTypes.Base64Octet => nameof(ClaimValueTypes.Base64Octet),
                ClaimValueTypes.Boolean => nameof(ClaimValueTypes.Boolean),
                ClaimValueTypes.Date => nameof(ClaimValueTypes.Date),
                ClaimValueTypes.DateTime => nameof(ClaimValueTypes.DateTime),
                ClaimValueTypes.DaytimeDuration => nameof(ClaimValueTypes.DaytimeDuration),
                ClaimValueTypes.DnsName => nameof(ClaimValueTypes.DnsName),
                ClaimValueTypes.Double => nameof(ClaimValueTypes.Double),
                ClaimValueTypes.DsaKeyValue => nameof(ClaimValueTypes.DsaKeyValue),
                ClaimValueTypes.Email => nameof(ClaimValueTypes.Email),
                ClaimValueTypes.Fqbn => nameof(ClaimValueTypes.Fqbn),
                ClaimValueTypes.HexBinary => nameof(ClaimValueTypes.HexBinary),
                ClaimValueTypes.Integer => nameof(ClaimValueTypes.Integer),
                ClaimValueTypes.Integer32 => nameof(ClaimValueTypes.Integer32),
                ClaimValueTypes.Integer64 => nameof(ClaimValueTypes.Integer64),
                ClaimValueTypes.KeyInfo => nameof(ClaimValueTypes.KeyInfo),
                ClaimValueTypes.Rfc822Name => nameof(ClaimValueTypes.Rfc822Name),
                ClaimValueTypes.Rsa => nameof(ClaimValueTypes.Rsa),
                ClaimValueTypes.RsaKeyValue => nameof(ClaimValueTypes.RsaKeyValue),
                ClaimValueTypes.Sid => nameof(ClaimValueTypes.Sid),
                ClaimValueTypes.String => nameof(ClaimValueTypes.String),
                ClaimValueTypes.Time => nameof(ClaimValueTypes.Time),
                ClaimValueTypes.UInteger32 => nameof(ClaimValueTypes.UInteger32),
                ClaimValueTypes.UInteger64 => nameof(ClaimValueTypes.UInteger64),
                ClaimValueTypes.UpnName => nameof(ClaimValueTypes.UpnName),
                ClaimValueTypes.X500Name => nameof(ClaimValueTypes.X500Name),
                ClaimValueTypes.YearMonthDuration => nameof(ClaimValueTypes.YearMonthDuration),
                _ => type,
            };
        }
    }
}
