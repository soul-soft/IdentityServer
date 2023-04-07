namespace IdentityServer.Models
{
    public class ApiScope : Resource, IScope
    {
        public string Scope => Name;
       
        public bool Required { get; set; } = false;
        
        protected ApiScope()
        {

        }

        public ApiScope(string name) : base(name)
        {

        }
    }
}
