namespace IdentityServer.Models
{
    public class ApiResource : IApiResource
    {
        public ApiResource(string name, string scope)
        {
            Name = name;
            Scope = scope;
        }

        public IReadOnlyCollection<string> UserClaims { get; set; } = new HashSet<string>();
        
        public IReadOnlyCollection<string> AllowedAccessTokenSigningAlgorithms { get; set; } = new HashSet<string>();

        public string Name { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public string Scope { get; set; }
    }
}
