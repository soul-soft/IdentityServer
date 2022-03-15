namespace IdentityServer.Services
{
    public interface IServerUrl
    {
        string GetIdentityServerBaseUrl();
        string GetIdentityServerIssuerUri();
    }
}
