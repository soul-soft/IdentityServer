namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIdentityServer(this WebApplication app)
        {
            app.UseMiddleware<IdentityServerMiddleware>();
            return app;
        }
    }
}
