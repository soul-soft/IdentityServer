namespace IdentityServer.Models
{
    public abstract class Resource
    {
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public IReadOnlyCollection<string> Scopes { get; } = new HashSet<string>();
        public IReadOnlyCollection<string> UserClaims { get; set; } = new HashSet<string>();
        public Resource(string name)
        {
            Name = name;
        }
    }
}
