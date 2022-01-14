using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ProfileDataRequestContext
    {
        public IClient Client { get; }
        public ClaimsPrincipal Subject { get; }
        public ProfileDataCaller Caller { get; }
        public IReadOnlyCollection<string> RequestedClaimTypes { get; }

        public List<Claim> IssuedClaims { get; } = new List<Claim>();

        public ProfileDataRequestContext(
            IClient client,
            ClaimsPrincipal subject,
            ProfileDataCaller caller,
            IEnumerable<string> requestedClaimTypes)
        {
            Client = client;
            Subject = subject;
            Caller = caller;
            RequestedClaimTypes = requestedClaimTypes.ToArray();
        }
    }

    public enum ProfileDataCaller
    {
        UserInfoEndpoint,
        ClaimsProviderIdentityToken,
        ClaimsProviderAccessToken
    }
}
