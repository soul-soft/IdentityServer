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
                logger.LogInformation("  ___    _            _   _ _           ____                           \r\n |_ _|__| | ___ _ __ | |_(_) |_ _   _  / ___|  ___ _ ____   _____ _ __ \r\n  | |/ _` |/ _ \\ '_ \\| __| | __| | | | \\___ \\ / _ \\ '__\\ \\ / / _ \\ '__|\r\n  | | (_| |  __/ | | | |_| | |_| |_| |  ___) |  __/ |   \\ V /  __/ |   \r\n |___\\__,_|\\___|_| |_|\\__|_|\\__|\\__, | |____/ \\___|_|    \\_/ \\___|_|   \r\n                                |___/                                  ");
            });
        }
    }
}
