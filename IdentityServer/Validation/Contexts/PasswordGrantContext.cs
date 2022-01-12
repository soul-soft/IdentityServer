namespace IdentityServer.Validation
{
    public class PasswordGrantContext
    {
        public IClient Client { get; }
        public Resources Resources { get; }
        public IEnumerable<string> Scopes { get; }

        public PasswordGrantContext(IClient client, Resources resources, IEnumerable<string> scopes)
        {
            Client = client;
            Resources = resources;
            Scopes = scopes;
        }
    }
}
