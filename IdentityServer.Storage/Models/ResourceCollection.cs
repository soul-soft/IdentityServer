using System.Collections;
using static IdentityServer.OpenIdConnects;

namespace IdentityServer.Models
{
    public class ResourceCollection : IEnumerable<IResource>
    {
        private readonly IEnumerable<IResource> _resources;

        public ResourceCollection(IEnumerable<IResource> resources)
        {
            _resources = resources;
        }

        public ResourceCollection(params IEnumerable<IResource>[] resources)
        {
            _resources = resources.SelectMany(s => s);
        }

        public bool OfflineAccess
        {
            get
            {
                return IdentityResources
                .Any(a => a.Name == StandardScopes.OfflineAccess);
            }
        }

        public IReadOnlyCollection<string> Scopes
        {
            get
            {
                return _resources
                    .Where(a => a is IScope)
                    .Cast<IScope>()
                    .Select(s => s.Scope)
                    .ToList();
            }
        }

        public IReadOnlyCollection<string> ClaimTypes
        {
            get
            {
                return _resources
                   .SelectMany(s => s.ClaimTypes)
                   .Distinct()
                   .ToList();
            }
        }

        public IReadOnlyCollection<ApiScope> ApiScopes
        {
            get
            {
                return _resources
                    .Where(a => a is ApiScope)
                    .Cast<ApiScope>()
                    .ToList();
            }
        }

        public IReadOnlyCollection<ApiResource> ApiResources
        {
            get
            {
                return _resources.Where(a => a is ApiResource)
                    .Cast<ApiResource>()
                    .ToList();
            }
        }

        public IReadOnlyCollection<IdentityResource> IdentityResources
        {
            get
            {
                return _resources
                    .Where(a => a is IdentityResource)
                    .Cast<IdentityResource>()
                    .ToList();
            }
        }

        public IEnumerator<IResource> GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_resources).GetEnumerator();
        }
    }
}
