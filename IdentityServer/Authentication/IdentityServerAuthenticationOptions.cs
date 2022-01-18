using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Authentication
{
    public class IdentityServerAuthenticationOptions
        : AuthenticationSchemeOptions
    {
        public bool SaveToken { get; set; } = true;
    }
}
