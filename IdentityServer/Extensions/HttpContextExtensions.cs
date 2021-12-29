using IdentityServer.Configuration;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace IdentityServer
{
    internal static class HttpContextExtensions
    {
        public static string? GetCorsOrigin(this HttpContext context)
        {
            var urls = context.RequestServices.GetRequiredService<IServerUrls>();
            return urls.Origin;
        }
        public static string GetIdentityServerOrigin(this HttpContext context)
        {
            var urls = context.RequestServices.GetRequiredService<IServerUrls>();
            return urls.Origin;
        }
        public static void SetNoCache(this HttpContext context)
        {
            var response = context.Response;
            if (!response.Headers.ContainsKey(HeaderNames.CacheControl))
            {
                response.Headers.Add(HeaderNames.CacheControl, "no-store, no-cache, max-age=0");
            }
            else
            {
                response.Headers[HeaderNames.CacheControl] = "no-store, no-cache, max-age=0";
            }

            if (!response.Headers.ContainsKey(HeaderNames.Pragma))
            {
                response.Headers.Add(HeaderNames.Pragma, "no-cache");
            }
        }
        public static void SetCache(this HttpContext context, int maxAge, params string[] varyBy)
        {
            var response = context.Response;
            if (maxAge == 0)
            {
                context.SetNoCache();
            }
            else if (maxAge > 0)
            {
                if (!response.Headers.ContainsKey("Cache-Control"))
                {
                    response.Headers.Add(HeaderNames.CacheControl, $"max-age={maxAge}");
                }

                if (varyBy?.Any() == true)
                {
                    var vary = varyBy.Aggregate((x, y) => x + "," + y);
                    if (response.Headers.ContainsKey(HeaderNames.Vary))
                    {
                        vary = response.Headers[HeaderNames.Vary].ToString() + "," + vary;
                    }
                    response.Headers[HeaderNames.Vary] = vary;
                }
            }
        }
        public static string GetIdentityServerIssuerUri(this HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var options = context.RequestServices.GetRequiredService<IdentityServerOptions>();
            var uri = options.IssuerUri;
            if (string.IsNullOrEmpty(uri))
            {
                uri = context.GetIdentityServerOrigin();
                if (uri.EndsWith("/"))
                    uri = uri.Substring(0, uri.Length - 1);
                if (options.LowerCaseIssuerUri)
                {
                    uri = uri.ToLowerInvariant();
                }
            }

            return uri;
        }
    }
}
