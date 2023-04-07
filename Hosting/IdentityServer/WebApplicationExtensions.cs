using Hosting.Configuration;
using IdentityServer.EntityFramework;
using IdentityServer.Models;

namespace Hosting.IdentityServer
{
    public static class WebApplicationExtensions
    {
        public static void PersistIdentityServer(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IdentityServerDbContext>();
                var client = Config.Clients.First();
                if (context.Clients.Any(a => a.ClientId == client.ClientId))
                {
                    return;
                }
                foreach (var item in Config.Clients)
                {
                    context.Clients.Add(item);
                }
                foreach (var item in Config.Resources)
                {
                    if (item is ApiScope apiScope)
                    {
                        context.ApiScopes.Add(apiScope);
                    }
                    if (item is ApiResource apiResource)
                    {
                        context.ApiResources.Add(apiResource);
                    }
                    if (item is IdentityResource identityResource)
                    {
                        context.IdentityResources.Add(identityResource);
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
