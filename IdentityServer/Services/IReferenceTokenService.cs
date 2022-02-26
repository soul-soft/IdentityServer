namespace IdentityServer.Services
{
    public interface IReferenceTokenService
    {
        Task<ReferenceToken?> GetReferenceTokenAsync(string id);
        Task<string> CreateReferenceTokenAsync(AccessToken token);
    }
}
