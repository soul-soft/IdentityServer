using IdentityServer.Models;

namespace IdentityServer.EntityFramework.Entities
{
    public class ApiScopeEntity : ResourceEntity
    {
        public bool Required { get; set; } = false;

        public static implicit operator ApiScope?(ApiScopeEntity entity)
        {
            if (entity == null) return null;
            return new ApiScope(entity.Name) 
            {
                Required = entity.Required,
                Name = entity.Name,
                Enabled = entity.Enabled,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                ClaimTypes = entity.ClaimTypes.Select(s => s.Value).ToArray(),
            };
        }
    }
}
