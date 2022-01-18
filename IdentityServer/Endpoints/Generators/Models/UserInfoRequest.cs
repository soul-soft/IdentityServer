using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class UserInfoRequest
    {
        public ClaimsPrincipal Subject { get; }
        public IClient Client { get; }
        public Resources Resources { get; }

        public UserInfoRequest(ClaimsPrincipal subject, IClient client, Resources resources)
        {
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
