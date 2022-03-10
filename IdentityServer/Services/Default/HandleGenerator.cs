namespace IdentityServer.Services
{
    internal class HandleGenerator : IHandleGenerator
    {
        public Task<string> GenerateAsync(int length = 32)
        {
            var handle = CryptoRandom.CreateUniqueId(length,CryptoRandom.OutputFormat.Hex);
            return Task.FromResult(handle); 
        }
    }
}
