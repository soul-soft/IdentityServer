using IdentityServer.EntityFramework.Entities;

namespace IdentityServer.Models
{
    public class AuthorizationCodeEntity: Entity
    {
        public string Code { get; set; } = default!;
        public int Lifetime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public IEnumerable<ClaimEntity> Claims { get; set; } = new List<ClaimEntity>();
    }
}
