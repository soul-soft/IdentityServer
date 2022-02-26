namespace IdentityServer.Models
{
    public abstract class Resource : IResource
    {
        public bool Enabled { get; set; } = true;

        public string Name { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public bool ShowInDiscoveryDocument { get; set; } = true;

        public IReadOnlyCollection<string> ClaimTypes { get; set; } = new HashSet<string>();

        protected Resource(string name)
        {
            Name = name;
        }
    }
}
