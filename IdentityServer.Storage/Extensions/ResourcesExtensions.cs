namespace IdentityServer.Models
{
    public static class ResourcesExtensions
    {
        public static Resources ToResources(this IEnumerable<IResource> resources)
        {
            return new Resources(resources);
        }
    }
}
