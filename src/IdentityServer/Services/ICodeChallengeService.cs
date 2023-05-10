namespace IdentityServer.Services
{
    public interface ICodeChallengeService
    {
        string ComputeHash(string code,string method);
    }
}
