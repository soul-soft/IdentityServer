namespace IdentityServer.Models
{
    public class ApiResource : Resource
    {
        public bool Required { get; set; } = false;

        public string Scope { get; set; }
        
        public ICollection<Secret> ApiSecrets { get; set; } = new HashSet<Secret>();
        
        public ApiResource(string name,string scope) : base(name)
        {
            Scope = scope;
        }
    }
}
