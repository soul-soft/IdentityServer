namespace IdentityServer.EntityFramework.Entities
{
    public class ApiScopeEntity : ResourceEntity
    {
        public bool Required { get; set; } = false;

        public bool Emphasize { get; set; } = false;
    }
}
