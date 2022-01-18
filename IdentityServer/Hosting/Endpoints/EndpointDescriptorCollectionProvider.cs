using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Primitives;

namespace IdentityServer.Hosting
{
    public class EndpointDescriptorCollectionProvider : IEnumerable<EndpointDescriptor>
    {
        private CancellationTokenSource? _tokenSource;

        private readonly IEnumerable<EndpointDescriptor> _endpoints;

        public EndpointDescriptorCollectionProvider(IEnumerable<EndpointDescriptor> endpoints)
        {
            _endpoints = endpoints;
        }

        public void RequireCors(string policy, Func<string, bool> selector)
        {
            _tokenSource = new CancellationTokenSource();
            foreach (var item in _endpoints)
            {
                if (selector(item.Name))
                {
                    item.Metadata.Add(new EnableCorsAttribute(policy));
                }
            }
            _tokenSource.Cancel();
        }

        public IChangeToken GetChangeToken()
        {
            _tokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(_tokenSource.Token);
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
