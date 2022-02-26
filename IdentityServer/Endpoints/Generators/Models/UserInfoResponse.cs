using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class UserInfoResponse
    {
        private readonly IEnumerable<Profile> _profiles;

        public UserInfoResponse(IEnumerable<Profile> profiles)
        {
            _profiles = profiles;
        }
      
        internal string Serialize()
        {
            return ObjectSerializer.Serialize(_profiles);
        }
    }
}
