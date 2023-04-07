using IdentityServer.Models;

namespace IdentityServer.EntityFramework.Entities
{
    public class IdentityResourceEntity : ResourceEntity
    {
        public bool Required { get; set; } = false;

        public static implicit operator IdentityResource?(IdentityResourceEntity? resource)
        {
            if (resource == null)
            {
                return default;
            }
            return new IdentityResource(resource.Name)
            {
                Required = resource.Required,
                Name = resource.Name,
                Enabled = resource.Enabled,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                ClaimTypes = resource.ClaimTypes.Select(s => s.Value).ToArray(),
            };
        }
    }
}
