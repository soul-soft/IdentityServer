using IdentityServer.Models;

namespace Idp.IdentityServer
{
    public static class IdpResource
    {
        public static IEnumerable<Client> Clients
        {
            get
            {
                yield return new Client()
                {
                    ClientId = "js",
                    RequireSecret = false,
                    AllowedGrantTypes =
                    {
                        GrantTypes.AuthorizationCode
                    },
                    AllowedScopes =
                    {
                        "api",
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                    },
                    AllowedRedirectUris =
                    {
                        "https://localhost:8081/callback.html"
                    },
                };
            }
        }

        public static IEnumerable<Resource> Resources
        {
            get
            {
                yield return IdentityResources.OpenId;
                yield return IdentityResources.Profile;
                yield return new ApiScope("api");
            }
        }
    }
}
