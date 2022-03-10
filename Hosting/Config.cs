using IdentityServer.Models;

namespace Hosting.Configuration
{
    public static class Config
    {
        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client()
            {
                ClientId="client1",
                AllowedGrantTypes = new []
                {
                    "myGrant",
                    GrantTypes.ClientCredentials
                },
                ClientSecrets = new Secret[]
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
                //AccessTokenType = AccessTokenType.Reference,
                AllowedGrantTypes = new []
                {
                    GrantTypes.ClientCredentials,
                    GrantTypes.RefreshToken,
                    GrantTypes.Password
                },
                ClientSecrets = new Secret[]
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
                ClientSecrets = new Secret[]
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

        public static IEnumerable<IResource> Resources => new IResource[]
        {
            //用来给api资源进行分组，apiResource和apiScope是多对多的关系
            new ApiScope("api")
            {
                ClaimTypes = new string[]
                { 
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Email
                }
            },
            //name要和client_id相同，还需要配置secret
            new ApiResource("orderapi")
            {
                ClaimTypes = new string[]
                { 
                    JwtClaimTypes.Role
                },
                Scopes = new string[]
                {
                    "api",//属于api组
                }
            },
            new ApiResource("emailapi")
            {
                Scopes = new string[]
                {
                    "api"//属于api组
                }
            },
            StandardResources.OpenId,
            StandardResources.OfflineAccess,
            new IdentityResource("address")
            {
               //表示该身份资源允许签发phone和address给access_token
               ClaimTypes = new string[]
               {
                   JwtClaimTypes.Phone,
                   JwtClaimTypes.Address
               }
            },
        };

    }
}
