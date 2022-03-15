using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class IntrospectionResponse
    {
        private readonly Dictionary<string, object> _profiles;

        public IntrospectionResponse(Dictionary<string, object> profiles)
        {
            _profiles = profiles;
        }

        public string Serialize()
        {
            return ObjectSerializer.Serialize(_profiles);
        }
    }
}
