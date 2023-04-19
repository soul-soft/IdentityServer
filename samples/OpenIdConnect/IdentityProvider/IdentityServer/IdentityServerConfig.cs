using IdentityServer.Models;

namespace IdentityProvider.IdentityServer
{
    public class IdentityServerConfig
    {
        public static IEnumerable<Client> Clients
        {
            get
            {
                yield return new Client()
                {
                    ClientId = "idp",
                    RequireSecret = false,
                    AllowedGrantTypes =
                    {
                        GrantTypes.Password,
                    },
                    AllowedScopes =
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        "basic"
                    }
                };
                yield return new Client()
                {
                    ClientId = "mvc",
                    Secrets =
                    {
                        new Secret("secret".Sha512())
                    },
                    AllowedGrantTypes = 
                    {
                        GrantTypes.Password,
                        GrantTypes.AuthorizationCode
                    },
                    AllowedScopes = 
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        "basic"
                    }
                };
            }
        }
       
        public static IEnumerable<IResource> Resources 
        {
            get
            {
                yield return IdentityResources.OpenId;
                yield return IdentityResources.Profile;
                yield return new ApiScope("basic");
            }
        }

    }
}
