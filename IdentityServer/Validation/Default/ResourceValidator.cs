namespace IdentityServer.Validation
{
    public class ResourceValidator : IResourceValidator
    {
        public Task ValidateAsync(Resources resources, IEnumerable<string> requestedScopes)
        {
            foreach (var scope in requestedScopes)
            {
                if (!resources.Scopes.Contains(scope))
                {
                    throw new InvalidScopeException(string.Format("Invalid scope:'{0}'", scope));
                }
            }
            return Task.CompletedTask;
        }
    }
}
