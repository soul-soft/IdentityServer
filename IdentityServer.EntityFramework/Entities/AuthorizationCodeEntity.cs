using IdentityServer.EntityFramework.Entities;
using System.Security.Claims;

namespace IdentityServer.Models
{
    public class AuthorizationCodeEntity : Entity
    {
        public string Code { get; set; } = default!;
        public int Lifetime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public IEnumerable<ClaimEntity> Claims { get; set; } = new List<ClaimEntity>();

        protected AuthorizationCodeEntity()
        {

        }

        public AuthorizationCodeEntity(string code, int lifetime, DateTime expirationTime, DateTime creationTime, IEnumerable<ClaimEntity> claims)
        {
            Code = code;
            Lifetime = lifetime;
            ExpirationTime = expirationTime;
            CreationTime = creationTime;
            Claims = claims;
        }

        public  AuthorizationCode Cast()
        {
            var claims = Claims
                .Select(s => new Claim(s.Type, s.Value, s.ValueType, s.Issuer))
                .ToArray();
            return new AuthorizationCode(
                code: Code,
                lifetime: Lifetime,
                creationTime: CreationTime,
                claims: claims);
        }

        public static implicit operator AuthorizationCodeEntity(AuthorizationCode entity)
        {
            var claims = entity.Claims
                .Select(s => new ClaimEntity(s.Type, s.Value, s.ValueType, s.Issuer))
                .ToArray();
            return new AuthorizationCodeEntity(
                code: entity.Code,
                lifetime: entity.Lifetime,
                creationTime: entity.CreationTime,
                expirationTime: entity.ExpirationTime,
                claims: claims);
        }
    }
}
