using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIdentityServer(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapEndpoints();
            return app;
        }

        internal static void MapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var endpointDataSource = ActivatorUtilities
                .CreateInstance<IdentityServerEndpointBuilder>(endpoints.ServiceProvider);
            endpoints.DataSources.Add(endpointDataSource);
        }
    }
}
