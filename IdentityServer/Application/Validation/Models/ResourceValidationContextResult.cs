using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ResourceValidationResult : ValidationResult
    {
        public IEnumerable<Resource> Resources { get; }
     
        public ResourceValidationResult()
        {
            Resources = new List<Resource>();
        }

        public ResourceValidationResult(IEnumerable<Resource> resources)
        {
            Resources = resources;
        }
    }
}
