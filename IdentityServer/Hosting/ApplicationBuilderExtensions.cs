using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static EndpointDescriptorCollectionProvider UseIdentityServer(this WebApplication app)
        {
            app.UseMiddleware<IdentityServerMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            return app.MapEndpoints();
        }

        internal static EndpointDescriptorCollectionProvider MapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var pro = endpoints.ServiceProvider.GetRequiredService<EndpointDescriptorCollectionProvider>();
            var endpointDataSource = ActivatorUtilities
                .CreateInstance<IdentityServerEndpointDataSource>(endpoints.ServiceProvider);
            endpoints.DataSources.Add(endpointDataSource);
            return pro;
        }
    }
}
