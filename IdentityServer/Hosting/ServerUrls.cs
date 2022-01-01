using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Configuration;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    public class ServerUrls : IServerUrls
    {
        private readonly HttpContext _context;

        private readonly IdentityServerOptions _options;

        public ServerUrls(
            IdentityServerOptions options,
            IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _context = httpContextAccessor.HttpContext
                ?? throw new InvalidProgramException("Program type must be aspnetcore");
        }

        public string GetIdentityServerOrigin()
        {
            var request = _context.Request;
            return request.Scheme + "://" + request.Host.ToUriComponent();
        }

        public string GetIdentityServerIssuerUri()
        {
            var uri = _options.IssuerUri;
            if (string.IsNullOrEmpty(uri))
            {
                uri = GetIdentityServerOrigin();
                if (uri.EndsWith("/"))
                    uri = uri[0..^1];
                if (_options.LowerCaseIssuerUri)
                {
                    uri = uri.ToLowerInvariant();
                }
            }
            return uri;
        }
    }
}
