using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using EndpointDataSource = IdentityServer.Hosting.EndpointDataSource;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIdentityServer(this WebApplication app)
        {
            //app.UseMiddleware<IdentityServerMiddleware>();
            app.UseServerValidator();
            app.UseAuthentication();
            app.MapEndpoints();
            return app;
        }

        internal static void MapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var endpointDataSource = ActivatorUtilities
                .CreateInstance<EndpointDataSource>(endpoints.ServiceProvider);
            endpoints.DataSources.Add(endpointDataSource);
        }

        internal static void UseServerValidator(this WebApplication app)
        {

        }
    }
}
