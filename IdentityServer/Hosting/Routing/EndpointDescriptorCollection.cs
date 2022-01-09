using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Hosting
{
    public class EndpointDescriptorCollection : IEnumerable<EndpointDescriptor>
    {
        private readonly IEnumerable<EndpointDescriptor> _endpoints;

        public EndpointDescriptorCollection(IEnumerable<EndpointDescriptor> endpoints)
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

        public EndpointDescriptor Discovery => _endpoints
            .Where(a => a.Name == OpenIdConnectEndpoint.Names.Discovery).First();

        public EndpointDescriptor DiscoveryJwks => _endpoints
           .Where(a => a.Name == OpenIdConnectEndpoint.Names.DiscoveryJwks).First();

        public EndpointDescriptor Token => _endpoints
            .Where(a => a.Name == OpenIdConnectEndpoint.Names.Token).First();

        public EndpointDescriptor UserInfo => _endpoints
           .Where(a => a.Name == OpenIdConnectEndpoint.Names.UserInfo).First();

        public EndpointDescriptor Authorize => _endpoints
         .Where(a => a.Name == OpenIdConnectEndpoint.Names.Authorize).First();
    }
}
