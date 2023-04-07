using IdentityServer.Models;

namespace IdentityServer.EntityFramework.Entities
{
    public class ApiResourceEntity : ResourceEntity
    {
        public bool Required { get; set; } = false;

        public ICollection<StringEntity> AllowedScopes { get; set; } = default!;

        public ICollection<SecretEntity> Secrets { get; set; } = Array.Empty<SecretEntity>();

        public ICollection<PropertyEntity> Properties { get; set; } = new List<PropertyEntity>();

        public static implicit operator ApiResource?(ApiResourceEntity? entity)
        {
            if (entity == null) return null;
            return new ApiResource(entity.Name)
            {
                Required = entity.Required,
                Enabled = entity.Enabled,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                ClaimTypes = entity.ClaimTypes.Select(s => s.Value).ToArray(),
                Properties = entity.Properties.Select(s=>new KeyValuePair<string,string>(s.Key,s.Value)).ToArray(),
                Secrets = entity.Secrets.Select(s => new Secret
                {
                    Description = s.Description,
                    Expiration = s.Expiration,
                    Value = s.Value,
                }).ToArray(),
            };
        }

        public static implicit operator ApiResourceEntity(ApiResource resource)
        {
            return new ApiResourceEntity
            {
                Name = resource.Name,
                Required = resource.Required,
                Enabled = resource.Enabled,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                ClaimTypes = resource.ClaimTypes.Select(s => new StringEntity(s)).ToArray(),
                Properties = resource.Properties.Select(s=>new PropertyEntity(s.Key,s.Value)).ToArray(),
                AllowedScopes = resource.AllowedScopes.Select(s => new StringEntity(s)).ToArray(),
                Secrets = resource.Secrets.Select(s => new SecretEntity(s.Value, s.Expiration, s.Description)).ToArray(),
            };
        }
    }
}
