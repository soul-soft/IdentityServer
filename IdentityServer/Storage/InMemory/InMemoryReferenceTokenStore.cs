using System.Collections.Concurrent;
using IdentityServer.Infrastructure;
using IdentityServer.Models;

namespace IdentityServer.Storage
{
    internal class InMemoryReferenceTokenStore : IReferenceTokenStore
    {
        private readonly ConcurrentDictionary<string, IToken> _tokens = new ConcurrentDictionary<string, IToken>();

        public Task<string> SaveAsync(IToken token)
        {
            int replay = 0;
            while (true)
            {
                replay++;
                var handle = CryptoRandom.CreateUniqueId(32, CryptoRandom.OutputFormat.Hex);
                if (_tokens.TryAdd(handle, token))
                {
                    return Task.FromResult(handle);
                }
                if (replay >= 10)
                {
                    throw new InvalidOperationException("The tenth attempt to create a reference token still failed. Please clean up the invalid reference token");
                }
            }
        }
    }
}
