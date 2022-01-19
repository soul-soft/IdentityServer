using System.Collections;

namespace IdentityServer.Hosting
{
    public class EndpointDescriptorCollectionProvider : IEnumerable<EndpointDescriptor>
    {
        private readonly IEnumerable<EndpointDescriptor> _endpoints;

        public EndpointDescriptorCollectionProvider(IEnumerable<EndpointDescriptor> endpoints)
        {
            _endpoints = endpoints;
        }
     
        public IEnumerator<EndpointDescriptor> GetEnumerator()
        {
            return _endpoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_endpoints).GetEnumerator();
        }
    }
}
