using IdentityServer;
using IdentityServer.Models;
using static IdentityServer.IdentityServerConstants;

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
                    "myGrant",
                    GrantTypes.RefreshToken
                },
                ClientSecrets=new ISecret[]
                {
                    new Secret("secret".Sha512())
                },
                //AccessTokenType=AccessTokenType.Reference,
                AllowedScopes = new[]
                {
                    "api",
                    StandardScopes.OpenId,
                    StandardScopes.OfflineAccess,
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
            IdentityServerConstants.IdentityResources.OpenId,
            IdentityServerConstants.IdentityResources.OfflineAccess,
        };
    }
}
