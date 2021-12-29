namespace IdentityServer.Application.Services
{
    public interface ICorsPolicyService
    {
        Task<bool> IsOriginAllowedAsync(string origin);
    }
}
