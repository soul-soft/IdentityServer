using IdentityServer;
using IdentityServer.Configuration;
using IdentityServer.Hosting.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIdentityServer(this IApplicationBuilder app)
        {
            app.ConfigureCors();
            app.UseMiddleware<IdentityServerMiddleware>();
            return app;
        }

        private static void ConfigureCors(this IApplicationBuilder app)
        {
            app.UseCors(Constants.IdentityServerName);
        }
    }
}
