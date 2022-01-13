namespace IdentityServer.Services
{
    public class ProfileDataRequest
    {
        public ProfileDataCaller Caller { get; set; }

        public IEnumerable<string> UserClaims { get; }

        public ProfileDataRequest(ProfileDataCaller caller, IEnumerable<string> userClaims)
        {
            Caller = caller;
            UserClaims = userClaims;
        }
    }

    public enum ProfileDataCaller
    {
        UserInfoEndpoint,
        ClaimsProviderIdentityToken,
        ClaimsProviderAccessToken
    }
}
