namespace IdentityServer.Services
{
    public interface IProfileService
    {
        Task IsActiveAsync(IsActiveContext context);
        Task GetProfileDataAsync(ProfileDataRequestContext context);
    }
}
