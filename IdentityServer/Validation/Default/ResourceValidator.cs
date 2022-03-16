namespace IdentityServer.Validation
{
    public class ResourceValidator : IResourceValidator
    {
        public  Task ValidateAsync(IEnumerable<string> scopes, Resources resources)
        {
            foreach (var scope in scopes)
            {
                if (!resources.Scopes.Contains(scope))
                {
                    throw new ValidationException(OpenIdConnectErrors.InvalidScope);
                }
            }
            return Task.CompletedTask;
        }
    }
}
