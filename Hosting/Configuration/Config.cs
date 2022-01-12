using IdentityServer.Models;
using IdentityServer.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using static IdentityServer.OpenIdConnects;

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
                    GrantTypes.ClientCredentials
                },
                ClientSecrets=new ISecret[]
                {
                    new Secret("secret".Sha512())
                },
                AllowedScopes = new[]
                {
                    "api","rpc",StandardScopes.OfflineAccess
                }
            }
        };

        public static IEnumerable<IApiScope> ApiScopes => new IApiScope[]
        {
            new ApiScope("api")
            {

            }, 
            new ApiScope("rpc")
            {

            }
        };

        public static IEnumerable<IApiResource> ApiResources => new IApiResource[]
        {
            new ApiResource("api1")
            {
                Scopes = new string[]
                {
                    "api"
                }
            }
        };

        public static IEnumerable<IIdentityResource> IdentityResources => new IIdentityResource[]
        {
            new IdentityResource(OpenIdConnectScope.OpenId)
            {
                Required = true,
                UserClaims = new string[]
                {
                    JwtClaimTypes.Subject,
                }
            }
        };
    }
}
