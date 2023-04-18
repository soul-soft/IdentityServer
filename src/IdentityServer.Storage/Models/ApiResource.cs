namespace IdentityServer.Models
{
    public class ApiResource : Resource
    {
        public bool Required { get; set; } = false;

        public ICollection<Secret> Secrets { get; set; } = new List<Secret>();
      
        public ICollection<string> AllowedScopes { get; set; } = new List<string>();

        public ICollection<KeyValuePair<string, string>> Properties { get; set; } = new Dictionary<string, string>();

        protected ApiResource()
        {

        }

        public ApiResource(string name) : base(name)
        {
        }
    }
}
