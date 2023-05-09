namespace IdentityServer.Services
{
    public interface ICodeChallengeHashService
    {
        string ComputeHash(string code,string method);
    }
}
