namespace IdentityServer.Services
{
    internal class IdGenerator : IIdGenerator
    {
        public string GeneratorId()
        {
            var array = Guid.NewGuid().ToByteArray();
            return BitConverter.ToString(array);
        }
    }
}
