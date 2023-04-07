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
                Secrets = new Secret[]
                {
                    new Secret("secret".Sha512())
                },
                AllowedScopes = new[]
                {
                    "api",
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                },
                AllowedRedirectUris = new string[]
                {
                    "http://www.baidu.com",
                    "https://localhost:49962/oidc-sign",
                    "https://localhost:7098/signin-oidc"
                },
                RequireSecret = false
            },
            new Client()
            {
                ClientId = "client2",
                RequireSecret = false,
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
                OfflineAccess=false,
                AllowedGrantTypes = new []
                {
                    GrantTypes.ClientCredentials,
                    GrantTypes.RefreshToken,
                    GrantTypes.Password,
                    "myGrant"
                },
                Secrets = new Secret[]
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
                ClaimTypes = new []
                {
                    JwtClaimTypes.Role
                },
                Secrets = new []
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = new[]
                {
                    "api"
                }
            },
            new ApiResource("emailapi")
            {
                AllowedScopes = new[]
                {
                    "api"
                }
            },
            IdentityResources.OpenId,
            IdentityResources.OfflineAccess,
            IdentityResources.Profile,
        };
    }
}
