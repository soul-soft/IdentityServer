namespace IdentityServer.Validation
{
    public class ClientCredentialsGrantValidationContext
    {
        public IClient Client { get; }
        public Resources Resources { get; }
        public IEnumerable<string> Scopes { get; }

        public ClientCredentialsGrantValidationContext(IClient client, Resources resources, IEnumerable<string> scopes)
        {
            Client = client;
            Resources = resources;
            Scopes = scopes;
        }
    }
}
