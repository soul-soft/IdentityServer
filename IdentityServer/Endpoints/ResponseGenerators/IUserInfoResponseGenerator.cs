namespace IdentityServer.Endpoints
{
    public interface IUserInfoResponseGenerator
    {
        Task<UserInfoResponse> ProcessAsync(UserInfoGeneratorRequest request);
    }
}
