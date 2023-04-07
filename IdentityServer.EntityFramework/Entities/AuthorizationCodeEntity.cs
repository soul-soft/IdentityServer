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

        public static implicit operator AuthorizationCode?(AuthorizationCodeEntity? entity)
        {
            if (entity == null) return null;
            return new AuthorizationCode(
                code: entity.Code,
                lifetime: entity.Lifetime,
                creationTime: entity.CreationTime,
                claims: entity.Claims.Select(s => new Claim(s.Type, s.Value, s.ValueType, s.Issuer)).ToArray());
        }
    }
}
