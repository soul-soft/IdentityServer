namespace IdentityServer.Services
{
    public interface IGrantTypeService
    {
        Task<IEnumerable<string>> GetGrantTypeNames();
    }
}
