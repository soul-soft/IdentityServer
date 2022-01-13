using IdentityServer.Models;
using static IdentityServer.Models.OpenIdConnects;

namespace Hosting.Configuration
{
    public static class Config
    {
        public static IEnumerable<IClient> Clients => new IClient[]
        {
            new Client("client")
            {
                AllowedGrantTypes = new []
                {
                   "myGrant",GrantTypes.RefreshToken
                },
                ClientSecrets=new ISecret[]
                {
                    new Secret("secret".Sha512())
                },
                AllowedScopes = new[]
                {
                    "api",
                    StandardScopes.OpenId,
                    StandardScopes.OfflineAccess
                }
            }
        };

        public static IEnumerable<IApiScope> ApiScopes => new IApiScope[]
        {
            new ApiScope("api"),
            new ApiScope("rpc")
        };

        public static IEnumerable<IIdentityResource> IdentityResources => new IIdentityResource[]
        {
            OpenIdConnects.IdentityResources.OpenId,
            OpenIdConnects.IdentityResources.OfflineAccess,
        };
    }
}
