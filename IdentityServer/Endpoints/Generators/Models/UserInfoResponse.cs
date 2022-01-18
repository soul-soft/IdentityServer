using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class UserInfoResponse
    {
        private readonly Dictionary<string, object?> _items = new Dictionary<string, object?>();

        public void Add(string name, object? value)
        {
            _items.TryAdd(name, value);
        }

        internal string Serialize()
        {
            return ObjectSerializer.Serialize(_items);
        }
    }
}
