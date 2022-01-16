namespace IdentityServer.Validation
{
    public interface IResourceValidator
    {
        Task ValidateAsync(Resources resources, IEnumerable<string> requestedScopes);
    }
}
