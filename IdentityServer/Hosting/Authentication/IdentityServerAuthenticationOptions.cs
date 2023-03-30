using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Hosting
{
    public class IdentityServerAuthenticationOptions
        : AuthenticationSchemeOptions
    {
        public bool SaveToken { get; set; } = true;
    }
}
