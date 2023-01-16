using IdentityServer.Endpoints;
using IdentityServer.Hosting;
using IdentityServer.Models;
using System.Collections.Specialized;

namespace Hosting.IdentityServer
{
    public class AuthorizeResponseGenerator : IAuthorizeResponseGenerator
    {
        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasFormContentType)
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, "Invalid contextType");
            }
            var from = await context.Request.ReadFormAsync();
            var body = GetNameValueCollection(from);
            var username = body["username"];
            var password = body["password"];
            if (username=="123"&&password=="123")
            {

            }
        }

        public static NameValueCollection GetNameValueCollection(IFormCollection form)
        {
            var nv = new NameValueCollection();
            foreach (var field in form)
            {
                nv.Add(field.Key, field.Value.First());
            }
            return nv;
        }
    }
}
