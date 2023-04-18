namespace IdentityServer.Models
{
    public class IdentityResource : Resource, IIdentityScope
    {
        public bool Required { get; set; } = false;
        
        public string Scope => Name;

        protected IdentityResource()
        {

        }

        public IdentityResource(string name) : base(name)
        {

        }
    }
}
