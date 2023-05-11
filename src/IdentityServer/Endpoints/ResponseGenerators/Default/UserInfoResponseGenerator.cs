using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class UserInfoResponseGenerator : IUserInfoResponseGenerator
    {
        private readonly IClaimService _claimService;

        public UserInfoResponseGenerator(IClaimService claimService)
        {
            _claimService = claimService;
        }

        public async Task<UserInfoGeneratorResponse> GenerateAsync(ClaimsPrincipal subject, Client client, Resources resources)
        {
            var profileDataRequest = new ProfileClaimsRequest(subject, client, resources);
            var principal = await _claimService.GetProfileClaimsAsync(profileDataRequest);
            return new UserInfoGeneratorResponse(principal.Claims.ToClaimsDictionary());
        }
    }
}
