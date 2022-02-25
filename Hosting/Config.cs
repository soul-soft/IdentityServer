using IdentityServer;
using IdentityServer.Models;
using static IdentityServer.IdentityServerConstants;

namespace Hosting.Configuration
{
    public static class Config
    {
        public static IEnumerable<IClient> Clients => new IClient[]
        {
            new Client()
            {
                ClientId="client1",
                AllowedGrantTypes = new []
                {
                    "myGrant",
                    GrantTypes.ClientCredentials
                },
                ClientSecrets = new ISecret[]
                {
                    new Secret("secret".Sha512())
                },
                AllowedScopes = new[]
                {
                    "api",
                }
            },
            new Client()
            {
                ClientId = "client2",
                RequireClientSecret = false,
                AllowedGrantTypes = new []
                {
                    "myGrant",
                    GrantTypes.ClientCredentials
                },
                AllowedScopes = new[]
                {
                    "api",
                }
            },
            new Client()
            {
                ClientId = "client3",
                AccessTokenType = AccessTokenType.Reference,
                AllowedGrantTypes = new []
                {
                    GrantTypes.ClientCredentials,
                    GrantTypes.RefreshToken,
                    GrantTypes.Password
                },
                ClientSecrets = new ISecret[]
                {
                    new Secret("secret".Sha512())
                },
                AllowedScopes = new[]
                {
                    "api",
                    StandardScopes.OpenId,
                    StandardScopes.OfflineAccess
                }
            },
            new Client()
            {
                ClientId="client5",
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
        /// api资源，注意只有当access_token的类型为reference时，需要改资源，
        /// 因为当jwt时，是自签名验证的，它本身就是一个api资源，他的认证行为是在api本身配置的，无需在identityserver中定义
        /// 而当reference时，api资源需要经过identityserver统一验证
        /// </summary>
        public static IEnumerable<IResource> Resources => new IResource[]
        {
            //用来给api资源进行分组，apiResource和apiScope是多对多的关系
            new ApiScope("api")
            {
                UserClaims = new string[]
                { 
                    JwtClaimTypes.Role,
                }
            },
            //name要和client_id相同，还需要配置secret
            new ApiResource("orderapi")
            {
                UserClaims = new string[]{ JwtClaimTypes.Role},
                Scopes = new string[]
                {
                    "api",//指定api所属范围
                }
            },
            new ApiResource("emailapi")
            {
                UserClaims = new string[]{ JwtClaimTypes.Email},
                Scopes = new string[]
                {
                    "api"//指定api所属范围
                }
            },
            IdentityResources.OpenId,
            IdentityResources.OfflineAccess,
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
