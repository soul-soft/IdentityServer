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
    }
}
