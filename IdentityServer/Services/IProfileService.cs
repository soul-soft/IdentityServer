namespace IdentityServer.Services
{
    public interface IProfileService
    {
        Task<IEnumerable<Profile>> GetProfileDataAsync(ProfileDataRequestContext context);
    }
}
