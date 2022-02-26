namespace IdentityServer.Services
{
    internal class ProfileService : IProfileService
    {
        public Task<IEnumerable<Profile>> GetProfileDataAsync(ProfileDataRequestContext request)
        {
            IEnumerable<Profile> result = Array.Empty<Profile>();
            return Task.FromResult(result);
        }
    }
}
