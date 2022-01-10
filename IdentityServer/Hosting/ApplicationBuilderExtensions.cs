using IdentityServer;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIdentityServer(this WebApplication app)
        {
            //app.UseMiddleware<IdentityServerMiddleware>();
            app.MapEndpoints();
            return app;
        }

        internal static void MapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var descriptors = endpoints.ServiceProvider.GetRequiredService<IEnumerable<EndpointDescriptor>>();
            endpoints.DataSources.Add(new IdentityServerEndpointDataSource(descriptors));
        }
    }
}
