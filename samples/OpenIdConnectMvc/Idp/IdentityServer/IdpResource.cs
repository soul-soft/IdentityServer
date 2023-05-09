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
                    ClientId = "mvc",
                    Secrets =
                    {
                        new Secret("mvc".Sha256())
                    },
                    AllowedGrantTypes =
                    {
                        GrantTypes.AuthorizationCode
                    },
                    AllowedScopes = 
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile
                    },
                    AllowedRedirectUris = 
                    {
                        "https://localhost:8081/signin-oidc"
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
            } 
        }
    }
}
