namespace IdentityServer.Services
{
    public interface IServerUrl
    {
        string GetOriginUrl();
        string GetBasePath();
    }
}
