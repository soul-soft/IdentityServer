using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Authentication
{
    public class LocalApiAuthenticationOptions : AuthenticationSchemeOptions
    {
        public bool SaveToken { get; set; } = true;
    }
}
