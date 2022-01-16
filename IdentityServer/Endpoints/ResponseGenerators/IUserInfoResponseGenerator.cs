namespace IdentityServer.Endpoints
{
    public interface IUserInfoResponseGenerator
    {
        Task<UserInfoResponse> ProcessAsync(UserInfoRequest request);
    }
}
