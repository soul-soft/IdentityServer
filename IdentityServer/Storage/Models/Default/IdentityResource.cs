namespace IdentityServer.Models
{
    public class IdentityResource : ResourceCollection, IIdentityResource
    {
        public bool Required { get; set; } = false;
       
        public IdentityResource(string name) : base(name)
        {

        }
    }
}
