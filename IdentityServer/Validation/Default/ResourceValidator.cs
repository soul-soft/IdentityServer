namespace IdentityServer.Validation
{
    public class ResourceValidator : IResourceValidator
    {
        public ResourceValidator()
        {
        }

        public  Task ValidateAsync(IEnumerable<string> scopes, ResourceCollection resources)
        {
            return Task.CompletedTask;
        }
    }
}
