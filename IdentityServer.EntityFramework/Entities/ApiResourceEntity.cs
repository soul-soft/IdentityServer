namespace IdentityServer.EntityFramework.Entities
{ 
    public class ApiResourceEntity : ResourceEntity
    {
        public bool Required { get; set; } = false;

        public string Scope { get; set; } = default!;

        public ICollection<SecretEntity> ApiSecrets { get; set; } = Array.Empty<SecretEntity>();
    }
}
