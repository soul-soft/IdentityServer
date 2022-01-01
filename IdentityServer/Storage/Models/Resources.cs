using System.Collections;

namespace IdentityServer.Models
{
    public class Resources : IEnumerable<Resource>
    {
        private readonly List<Resource> _resources;
      
        public Resources()
        {
            _resources = new List<Resource>();
        }
      
        public Resources(
            ICollection<ApiResource> apiResources,
            ICollection<IdentityResource> identityResources,
            ICollection<ApiScope> apiScopes)
        {
            var table = new List<Resource>();
            table.AddRange(apiResources);
            table.AddRange(identityResources);
            table.AddRange(apiScopes);
            _resources = table;
        }

        public IEnumerator<Resource> GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_resources).GetEnumerator();
        }

        public IEnumerable<ApiResource> ApiResources
        {
            get
            {
                return _resources.Where(a => a is ApiResource).Cast<ApiResource>();
            }
        }

        public IEnumerable<IdentityResource> IdentityResources
        {
            get
            {
                return _resources.Where(a => a is IdentityResource).Cast<IdentityResource>();
            }
        }

        public IEnumerable<ApiScope> ApiScopes
        {
            get
            {
                return _resources.Where(a => a is ApiScope).Cast<ApiScope>();
            }
        }
    }
}
