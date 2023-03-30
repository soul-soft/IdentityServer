namespace IdentityServer.Services
{
    internal class RandomGenerator : IRandomGenerator
    {
        public Task<string> GenerateAsync(int length = 32)
        {
            var handle = CryptoUtility.CreateUniqueId(length, CryptoUtility.OutputFormat.Base64Url);
            return Task.FromResult(handle);
        }
    }
}
