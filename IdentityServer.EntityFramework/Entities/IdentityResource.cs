using IdentityServer.Models;

namespace IdentityServer.EntityFramework.Entities
{
    public class IdentityResourceEntity : ResourceEntity
    {
        public bool Required { get; set; } = false;

        public string Scope => Name;

        public static implicit operator IdentityResource?(IdentityResourceEntity? entity)
        {
            if (entity == null) return null;

            return new IdentityResource(entity.Name)
            {
                Required = entity.Required,
                Enabled = entity.Enabled,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                ClaimTypes = entity.ClaimTypes.Select(s => s.Value).ToArray(),
            };
        }

        public static implicit operator IdentityResourceEntity(IdentityResource resource)
        {

            return new IdentityResourceEntity
            {
                Required = resource.Required,
                Enabled = resource.Enabled,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                ClaimTypes = resource.ClaimTypes.Select(s => new StringEntity(s)).ToArray(),
                Name = resource.Name,
            };
        }
    }
}
