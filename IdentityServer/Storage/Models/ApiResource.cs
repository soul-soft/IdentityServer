namespace IdentityServer.Models
{
    /// <summary>
    /// Api资源
    /// </summary>
    public class ApiResource : Resource
    {
        public ApiResource(string name)
            : base(name)
        {
        }

        public ICollection<string> AllowedAccessTokenSigningAlgorithms { get; set; } = new HashSet<string>();
    }
}
