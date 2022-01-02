using System.Net.Http.Headers;
using IdentityServer.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace IdentityServer
{
    internal static class HttpContextExtensions
    {
        internal static async Task WriteAsJsonExAsync(this HttpContext context, object obj)
        {
            await context.Response.WriteAsJsonAsync(obj, ObjectSerializer.JsonSerializerOptions);
        }
        
        internal static bool HasApplicationFormContentType(this HttpRequest request)
        {
            if (request.ContentType is null)
                return false;

            if (MediaTypeHeaderValue.TryParse(request.ContentType, out var header))
            {
                if (header?.MediaType == null)
                {
                    return false;
                }
                return header.MediaType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
