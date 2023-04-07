using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.EntityFramework.Entities
{
    public class TokenEntity : Entity
    {
        public string Code { get; set; } = default!;

        public string Type { get; set; } = default!;

        public int Lifetime { get; set; }

        public DateTime ExpirationTime { get; set; }

        public DateTime CreationTime { get; set; }

        public ICollection<ClaimEntity> Claims { get; set; } = Array.Empty<ClaimEntity>();

        public static implicit operator Token?(TokenEntity? entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new Token(
                code: entity.Code,
                type: entity.Type,
                lifetime: entity.Lifetime,
                creationTime: entity.CreationTime,
                claims: entity.Claims.Select(s => new Claim(s.Type, s.Value, s.ValueType, s.Issuer)).ToArray());
        }
    }
}
