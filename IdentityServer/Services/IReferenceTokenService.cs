namespace IdentityServer.Services
{
    public interface IReferenceTokenService
    {
        Task<string> CreateReferenceTokenAsync(IToken token);
    }
}
