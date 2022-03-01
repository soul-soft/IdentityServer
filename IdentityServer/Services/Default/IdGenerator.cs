namespace IdentityServer.Services
{
    internal class IdGenerator : IIdGenerator
    {
        public Task<string> GenerateAsync(int length = 32)
        {
            return Task.FromResult(CryptoRandom.CreateUniqueId(length, CryptoRandom.OutputFormat.Hex));
        }
    }
}
