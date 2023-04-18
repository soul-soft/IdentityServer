using System;

namespace IdentityServer.Services
{
    internal class RandomGenerator : IRandomGenerator
    {
        public Task<string> GenerateAsync()
        {
            var bytes = Guid.NewGuid().ToByteArray();
            var code = BitConverter.ToString(bytes).Replace("-", "");
            return Task.FromResult(code);
        }
    }
}
