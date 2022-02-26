namespace IdentityServer.Models
{
    public static class ResourcesExtensions
    {
        public static ResourceCollection ToResources(this IEnumerable<IResource> resources)
        {
            return new ResourceCollection(resources);
        }
    }
}
