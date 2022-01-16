namespace IdentityServer.Services
{
    public interface IProfileService
    {
        Task GetProfileDataAsync(ProfileDataRequestContext context);
    }
}
