using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    internal class ServerUrl : IServerUrl
    {
        private readonly HttpContext _context;
        private readonly IdentityServerOptions _options;
        private readonly IEnumerable<EndpointDescriptor> _endpoints;
      
        public ServerUrl(
            IHttpContextAccessor accessor,
            IdentityServerOptions options,
            IEnumerable<EndpointDescriptor> endpoints)
        {
            _options = options;
            _context = accessor.HttpContext?? throw new ArgumentNullException(nameof(accessor));
            _endpoints = endpoints;
        }

        public string GetServerBaseUri()
        {
            var request = _context.Request;
            var url = request.Scheme + "://" + request.Host.ToUriComponent();
            if (url.EndsWith("/"))
                url = url[0..^1];
            return url;
        }
        public string GetServerIssuer()
        {
            var url = _options.Issuer;
            if (string.IsNullOrEmpty(url))
            {
                url = GetServerBaseUri();
            }
            if (_options.LowerCaseIssuerUri)
            {
                url = url.ToLowerInvariant();
            }
            return url;
        }

        public string GetEndpointPath(string name)
        {
            return _endpoints.Where(a => a.Name == name).First().Path;
        }

        public string GetEndpointUri(string name)
        {
            return $"{GetServerBaseUri()}/{GetEndpointPath(name)}";
        }
    }
}
