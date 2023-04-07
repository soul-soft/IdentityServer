namespace IdentityServer.Models
{
    public class ApiResource : Resource
    {
        public bool Required { get; set; } = false;

        public ICollection<Secret> Secrets { get; set; } = new HashSet<Secret>();
      
        public ICollection<string> AllowedScopes { get;  } = default!;
       
        protected ApiResource()
        {

        }

        public ApiResource(string name) : base(name)
        {
        }
    }
}
