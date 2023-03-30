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
                    GrantTypes.ClientCredentials,
                    GrantTypes.AuthorizationCode
                },
                ClientSecrets = new Secret[]
                {
                    new Secret("secret".Sha512())
                },
                AllowedScopes = new[]
                {
                    "api"
                },
                AllowedRedirectUris = new string[]
                {
                    "http://www.baidu.com/callbck"
                },
                RequireClientSecret = false
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
                OfflineAccess=true,
                AllowedGrantTypes = new []
                {
                    GrantTypes.ClientCredentials,
                    GrantTypes.RefreshToken,
                    GrantTypes.Password,
                    "myGrant"
                },
                ClientSecrets = new Secret[]
                {
                    new Secret("secret".Sha512())
                },
                AllowedScopes = new[]
                {
                    StandardScopes.OpenId,
                    StandardScopes.OfflineAccess,
                    StandardScopes.Profile,
                    "api"
                }
            },
        };

        public static IEnumerable<IResource> Resources => new IResource[]
        {
            //用来给api资源进行分组，apiResource和apiScope是多对多的关系
            new ApiScope("api")
            {

            },
            //name要和client_id相同，还需要配置secret
            new ApiResource("orderapi")
            {
                ClaimTypes = new string[]
                {
                    JwtClaimTypes.Role
                },
                Scopes =
                {
                    "api",
                },
                ApiSecrets =
                {
                    new Secret("secret".Sha256())
                }
            },
            new ApiResource("emailapi")
            {
                Scopes = new string[]
                {
                    "api",
                    "profile",
                    "api"
                }
            },
            IdentityResources.OpenId,
            IdentityResources.OfflineAccess,
            IdentityResources.Profile,
        };
    }
}
