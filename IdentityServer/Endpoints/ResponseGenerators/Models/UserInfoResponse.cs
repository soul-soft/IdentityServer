using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class UserInfoResponse
    {
        public readonly Dictionary<string,object> Profiles;

        public UserInfoResponse(Dictionary<string, object> profiles)
        {
            Profiles = profiles;
        }
      
        internal string Serialize()
        {
            return ObjectSerializer.Serialize(Profiles);
        }
    }
}
