namespace IdentityServer.Models
{
    public class ApiResource : ResourceCollection, IApiResource
    {
        public bool Required { get; set; } = false;

        public IReadOnlyCollection<string> Scopes { get; set; } = new HashSet<string>();

        public ApiResource(string name) : base(name)
        {

        }
    }
}
