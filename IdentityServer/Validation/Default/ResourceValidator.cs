namespace IdentityServer.Validation
{
    public class ResourceValidator : IResourceValidator
    {
        public Task ValidateAsync(Resources resources, IEnumerable<string> requestedScopes)
        {
            return Task.CompletedTask;
        }
    }
}
