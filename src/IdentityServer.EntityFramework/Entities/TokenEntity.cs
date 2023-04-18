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

        public ICollection<ClaimEntity> Claims { get; set; } = new HashSet<ClaimEntity>();


        public Token Cast()
        {
            var claims = Claims
                .Select(s => new Claim(s.Type, s.Value, s.ValueType, s.Issuer))
                .ToArray();
            return new Token(
                code: Code,
                type: Type,
                lifetime: Lifetime,
                creationTime: CreationTime,
                expirationTime: ExpirationTime,
                claims: claims);
        }


        public static implicit operator TokenEntity(Token token)
        {
            return new TokenEntity
            {
                Code = token.Code,
                CreationTime = token.CreationTime,
                ExpirationTime = token.ExpirationTime,
                Lifetime = token.Lifetime,
                Type = token.Type,
                Claims = token.Claims.Select(s => new ClaimEntity(s.Type, s.Value, s.ValueType, s.Issuer)).ToArray(),
            };
        }
    }
}
