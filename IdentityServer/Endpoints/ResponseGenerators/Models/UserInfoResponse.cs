using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class UserInfoResponse
    {
        private readonly object _data;

        public UserInfoResponse(object data)
        {
            _data = data;
        }
      
        internal string Serialize()
        {
            return ObjectSerializer.Serialize(_data);
        }
    }
}
