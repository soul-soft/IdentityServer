namespace IdentityServer.Services
{
    internal class RandomGenerator : IRandomGenerator
    {
        public Task<string> GenerateAsync(int length = 32)
        {
            var handle = CryptoUtility.CreateUniqueId(length, CryptoUtility.OutputFormat.Hex);
            return Task.FromResult(handle);
        }
    }
}
