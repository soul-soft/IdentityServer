using IdentityServer.Infrastructure;
using IdentityServer.Models;
using IdentityServer.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Hosting.Configuration
{
    public static class Config
    {
        public static IEnumerable<IClient> Clients => new IClient[]
        {
            new Client("text", HashGenerator.Sha256("secret"))
            {

            }
        };
        
        public static IEnumerable<IApiScope> ApiScopes => new IApiScope[]
        {
            new ApiScope("api")
            {

            }
        };
       
        public static IEnumerable<IApiResource> ApiResources => new IApiResource[]
        {
            new ApiResource("api1")
            {
                Scopes=new string[]
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
                    JwtClaimTypes.Subject
                }
            }
        };
    }
}
