using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public class TokenRequestValidationContext : ValidatorContext
    {
        public HttpContext HttpContext { get; }
        public TokenRequestValidationContext(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }
    }
}
