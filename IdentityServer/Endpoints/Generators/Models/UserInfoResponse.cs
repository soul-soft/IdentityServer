using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class UserInfoResponse
    {
        private readonly Dictionary<string, object?> _items;

        public UserInfoResponse(Dictionary<string, object?> items)
        {
            _items = items;
        }
      
        internal string Serialize()
        {
            return ObjectSerializer.Serialize(_items);
        }
    }
}
