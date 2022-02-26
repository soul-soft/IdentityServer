namespace IdentityServer.Endpoints
{
    public interface IUserInfoGenerator
    {
        Task<UserInfoResponse> ProcessAsync(UserInfoGeneratorRequest request);
    }
}
