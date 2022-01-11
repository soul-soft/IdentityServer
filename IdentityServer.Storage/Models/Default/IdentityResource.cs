namespace IdentityServer.Models
{
    public class IdentityResource : Resource, IIdentityResource
    {
        public bool Required { get; set; } = false;
       
        public IdentityResource(string name) : base(name)
        {

        }
    }
}
