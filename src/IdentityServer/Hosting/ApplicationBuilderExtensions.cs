using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIdentityServer(this WebApplication app)
        {
            app.UseMiddleware<IdentityServerMiddleware>();
            Logging(app);
            return app;
        }

        private static void Logging(WebApplication app)
        {
            app.Lifetime.ApplicationStarted.Register(() => 
            {
                var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
                var clock = app.Services.GetRequiredService<ISystemClock>();
                var logger = loggerFactory.CreateLogger("IdentityServer.Hosting");
                logger.LogInformation("Identityserver started successfully.");
            });
        }
    }
}
