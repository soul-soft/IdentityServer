using IdentityServer.Configuration;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    internal class ServerUrl : IServerUrl
    {
        private readonly HttpContext _context;
        private readonly IdentityServerOptions _options;
      
        public ServerUrl(
            IHttpContextAccessor accessor,
            IdentityServerOptions options)
        {
            _options = options;
            _context = accessor.HttpContext
                ?? throw new ArgumentNullException(nameof(accessor));
        }
      
        public string GetIdentityServerOrigin()
        {
            var request = _context.Request;
            var url = request.Scheme + "://" + request.Host.ToUriComponent();
            if (url.EndsWith("/"))
                url = url[0..^1];
            return url;
        }
        
        public string GetIdentityServerIssuer()
        {
            var url = _options.IssuerUri;
            if (string.IsNullOrEmpty(url))
            {
                url = GetIdentityServerOrigin();
            }
            if (_options.LowerCaseIssuerUri)
            {
                url = url.ToLowerInvariant();
            }
            return url;
        }
    }
}
