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
                };
            }
        }
    }
}
