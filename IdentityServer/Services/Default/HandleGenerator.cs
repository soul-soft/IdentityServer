namespace IdentityServer.Services
{
    internal class HandleGenerator : IHandleGenerator
    {
        public Task<string> GenerateAsync(int length = 32)
        {
            var array = Guid.NewGuid().ToByteArray();
            var result = BitConverter.ToString(array).Replace("-", "");
            return Task.FromResult(result); 
        }
    }
}
