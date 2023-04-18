namespace IdentityServer.Models
{
    public abstract class Resource : IResource
    {
        public bool Enabled { get; set; } = true;

        public string Name { get; } = default!;

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public bool ShowInDiscoveryDocument { get; set; } = true;

        public ICollection<string> ClaimTypes { get; set; } = new List<string>();
      
        protected Resource() 
        {

        }
      
        protected Resource(string name)
        {
            Name = name;
        }
    }
}
