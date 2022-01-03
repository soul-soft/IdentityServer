namespace IdentityServer.Models
{
    public class IdentityResource : IIdentityResource
    {
        public string Name { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public string Scope { get; set; }

        public IReadOnlyCollection<string> UserClaims { get; set; } = new HashSet<string>();

        public IdentityResource(string name, string scope)
        {
            Name = name;
            Scope = scope;
        }
    }
}
