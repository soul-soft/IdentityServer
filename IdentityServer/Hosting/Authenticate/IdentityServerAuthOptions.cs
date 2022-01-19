using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Hosting
{
    public class IdentityServerAuthOptions
        : AuthenticationSchemeOptions
    {
        public bool SaveToken { get; set; } = true;
    }
}
