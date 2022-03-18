using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class UserInfoGeneratorResponse
    {
        public readonly Dictionary<string,object> Profiles;

        public UserInfoGeneratorResponse(Dictionary<string, object> profiles)
        {
            Profiles = profiles;
        }
      
        internal string Serialize()
        {
            return ObjectSerializer.Serialize(Profiles);
        }
    }
}
