using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class UserInfoProfileRequest
    {
        public ClaimsPrincipal Subject { get; }
        public IClient Client { get; }
        public Resources Resources { get; }

        public UserInfoProfileRequest(ClaimsPrincipal subject, IClient client, Resources resources)
        {
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
