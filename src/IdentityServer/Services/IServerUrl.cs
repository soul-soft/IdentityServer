namespace IdentityServer.Services
{
    public interface IServerUrl
    {
        string GetServerIssuer();
        string GetServerBaseUri();
        string GetEndpointUri(string name);
        string GetEndpointPath(string name);
    }
}
