using IdentityServer.Models;

namespace IdentityServer.EntityFramework.Entities
{
    public class ApiResourceEntity : ResourceEntity
    {
        public bool Required { get; set; } = false;

        public string Scope { get; set; } = default!;

        public ICollection<SecretEntity> Secrets { get; set; } = Array.Empty<SecretEntity>();

        public static implicit operator ApiResource?(ApiResourceEntity? entity)
        {
            if (entity == null) return null;
            return new ApiResource(entity.Name, entity.Scope)
            {
                Required = entity.Required,
                Enabled = entity.Enabled,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                ClaimTypes = entity.ClaimTypes.Select(s => s.Value).ToArray(),
                Secrets = entity.Secrets.Select(s => new Secret
                {
                    Description = s.Description,
                    Expiration = s.Expiration,
                    Value = s.Value,
                }).ToArray(),
            };
        }
    }
}
