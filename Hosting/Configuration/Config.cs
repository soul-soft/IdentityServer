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
                    "myGrant",GrantTypes.Password
                },
                ClientSecrets = new ISecret[]
                {
                    new Secret("secret".Sha512())
                },
                //AccessTokenType=AccessTokenType.Reference,
                AllowedScopes = new[]
                {
                    "api",
                    "openid",
                    "address"
                }
            }
        };

        /// <summary>
        /// scope资源
        /// </summary>
        public static IEnumerable<IApiScope> ApiScopes => new IApiScope[]
        {
            //用来给api资源进行分组，apiResource和apiScope是多对多的关系
            new ApiScope("api")
            {
                UserClaims = new string[]{ JwtClaimTypes.Role}
            },
        };

        /// <summary>
        /// api资源，注意只有当access_token的类型为reference时，需要改资源，
        /// 因为当jwt时，是自签名验证的，它本身就是一个api资源，他的认证行为是在api本身配置的，无需在identityserver中定义
        /// 而当reference时，api资源需要经过identityserver统一验证
        /// </summary>
        public static IEnumerable<IApiResource> ApiResources => new IApiResource[]
        {
            //name要和client_id相同，还需要配置secret
            new ApiResource("orderapi")
            {
                UserClaims = new string[]{ JwtClaimTypes.Role},
                Scopes=new string[]
                {
                    "api"//指定api所属范围
                }
            },
            new ApiResource("emailapi")
            {
                UserClaims = new string[]{ JwtClaimTypes.Email},
                Scopes=new string[]
                {
                    "api"//指定api所属范围
                }
            },
        };

        /// <summary>
        /// 身份资源
        /// </summary>
        public static IEnumerable<IIdentityResource> IdentityResources => new IIdentityResource[]
        {
           new IdentityResource("openid")
           {
               //表示该身份资源允许签发sub给access_token
               UserClaims = new string[]
               {
                   JwtClaimTypes.Subject
               }
           },
           new IdentityResource("address")
           {
               //表示该身份资源允许签发phone和address给access_token
               UserClaims = new string[]
               {
                   JwtClaimTypes.Phone,
                   JwtClaimTypes.Address
               }
           },
        };
    }
}
