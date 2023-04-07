namespace IdentityServer.Models
{
    public class ApiResource : Resource
    {
        public bool Required { get; set; } = false;

        public string Scope { get;  } = default!;

        public ICollection<Secret> Secrets { get; set; } = new HashSet<Secret>();
       
        protected ApiResource()
        {

        }

        public ApiResource(string name, string scope) : base(name)
        {
            Scope = scope;
        }
    }
}
