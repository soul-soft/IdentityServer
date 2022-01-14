using System.Security.Claims;

namespace IdentityServer.Models
{
    public class IsActiveContext
    {
        public IClient Client { get; }
        public ClaimsPrincipal Subject { get; }
        public ProfileIsActiveCaller Caller { get; set; }
        public bool IsActive { get; set; } = true;
        public IsActiveContext(IClient client, ClaimsPrincipal subject, ProfileIsActiveCaller caller)
        {
            Client = client;
            Subject = subject;
            Caller = caller;
        }
    }

    public enum ProfileIsActiveCaller
    {
        AuthorizeEndpoint,
        IdentityTokenValidation,
        AccessTokenValidation,
        ResourceOwnerValidation,
        ExtensionGrantValidation,
        RefreshTokenValidation,
        AuthorizationCodeValidation,
        UserInfoRequestValidation,
        DeviceCodeValidation,
    }
}
