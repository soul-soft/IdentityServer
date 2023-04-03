namespace IdentityServer.Hosting
{
    public class EndpointDescriptors
    {
        private readonly IdentityServerOptions _options;
        private readonly IEnumerable<EndpointDescriptor> _endpoints;
       
        public EndpointDescriptors(
            IdentityServerOptions options,
            IEnumerable<EndpointDescriptor> endpoints)
        {
            _options = options;
            _endpoints = endpoints;
        }

        public EndpointDescriptor? GetEndpoint(string name)
        {
            var endpoint = _endpoints.Where(a => a.Name == name)
                .FirstOrDefault();
            if (endpoint==null)
            {
                return null;
            }
            if (!_options.Endpoints.IsEndpointEnabled(endpoint))
            {
                return null;
            }
            return endpoint;
        }
    }
}
