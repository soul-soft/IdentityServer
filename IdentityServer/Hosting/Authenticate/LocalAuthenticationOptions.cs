using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Hosting
{
    public class LocalAuthenticationOptions
        : AuthenticationSchemeOptions
    {
        public bool SaveToken { get; set; } = true;
    }
}
