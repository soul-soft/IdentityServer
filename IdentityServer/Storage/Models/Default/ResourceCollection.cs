namespace IdentityServer.Models
{
    public abstract class ResourceCollection : IResource
    {
        public bool Enabled { get; set; } = true;

        public string Name { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public IReadOnlyCollection<string> UserClaims { get; set; } = new HashSet<string>();

        protected ResourceCollection(string name)
        {
            Name = name;
        }
    }
}
