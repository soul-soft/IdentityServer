namespace IdentityServer.Models
{
    public class Resources
    {
        private readonly IEnumerable<IResource> _resources;
        public Resources(IEnumerable<IResource> resources)
        {
            _resources = resources;
        }
        public IReadOnlyCollection<IApiResource> ApiResources 
        {
            get
            {
                return _resources.Where(a => a is IApiResource)
                    .Cast<IApiResource>()
                    .ToList();
            }
        }
        public IReadOnlyCollection<IApiScope> ApiScopes
        {
            get
            {
                return _resources
                    .Where(a => a is IApiScope)
                    .Cast<IApiScope>()
                    .ToList();
            }
        }
        public IReadOnlyCollection<IIdentityResource> IdentityResources
        {
            get
            {
                return _resources
                    .Where(a => a is IIdentityResource)
                    .Cast<IIdentityResource>()
                    .ToList();
            }
        }
    }
}
