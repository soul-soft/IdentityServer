using Microsoft.Extensions.Primitives;

namespace IdentityServer.EntityFramework.Entities
{
    public abstract class ResourceEntity: Entity
    {
        public bool Enabled { get; set; } = true;

        public string Name { get; set; } = default!;

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public bool ShowInDiscoveryDocument { get; set; } = true;

        public ICollection<StringEntity> ClaimTypes { get; set; } = new HashSet<StringEntity>();
    }
}
