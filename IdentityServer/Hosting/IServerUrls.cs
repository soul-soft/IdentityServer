namespace IdentityServer.Hosting
{
    public interface IServerUrls
    {
        string GetIdentityServerOrigin();
        string GetIdentityServerIssuerUri();
    }
}
