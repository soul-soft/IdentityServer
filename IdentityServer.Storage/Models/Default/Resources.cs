using System.Collections;
using static IdentityServer.OpenIdConnects;

namespace IdentityServer.Models
{
    public class Resources : IEnumerable<IResource>
    {
        private readonly IEnumerable<IResource> _resources;

        public Resources(IEnumerable<IResource> resources)
        {
            _resources = resources;
        }

        public Resources(params IEnumerable<IResource>[] resources)
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

        public IReadOnlyCollection<string> UserClaims
        {
            get
            {
                return _resources
                   .SelectMany(s => s.UserClaims)
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

        public IReadOnlyCollection<IApiResource> ApiResources
        {
            get
            {
                return _resources.Where(a => a is IApiResource)
                    .Cast<IApiResource>()
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
