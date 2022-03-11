namespace IdentityServer.Models
{
    public class ApiResource : Resource
    {
        public bool Required { get; set; } = false;

        public ICollection<string> Scopes { get; set; } = new HashSet<string>();
        
        public ICollection<Secret> ApiSecrets { get; set; } = new HashSet<Secret>();
        
        public ApiResource(string name) : base(name)
        {

        }
    }
}
