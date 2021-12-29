using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    public class ServerUrls : IServerUrls
    {
        private readonly HttpContext _httpContext;
        public ServerUrls(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext
                ?? throw new InvalidProgramException("Program type must be aspnetcore");
        }
      
        public string Origin
        {
            get
            {
                var request = _httpContext.Request;
                return request.Scheme + "://" + request.Host.ToUriComponent();
            }
        }
    }
}
