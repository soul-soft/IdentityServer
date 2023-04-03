namespace IdentityServer.Services
{
    public interface IServerUrl
    {
        string GetIdentityServerIssuer();
        string GetIdentityServerBaseUrl();
    }
}
