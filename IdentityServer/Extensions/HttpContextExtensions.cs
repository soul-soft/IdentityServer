using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Extensions
{
    public static class HttpContextExtensions
    {
        internal static bool HasApplicationFormContentType(this HttpRequest request)
        {
            if (MediaTypeHeaderValue.TryParse(request.ContentType, out var header))
            {
                if (header == null || header.MediaType == null)
                {
                    return false;
                }
                return header.MediaType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
