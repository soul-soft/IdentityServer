namespace IdentityServer.Services
{
    public interface IIdentityServerUrl
    {
        string GetServerIssuer();
        string GetServerBaseUri();
        string GetEndpointUri(string name);
        string GetEndpointPath(string name);
    }
}
