using IdentityServer.Serialization;

namespace IdentityServer.Endpoints
{
    public class IntrospectionGeneratorResponse
    {
        private readonly Dictionary<string, object> _profiles;

        public IntrospectionGeneratorResponse(Dictionary<string, object> profiles)
        {
            _profiles = profiles;
        }

        public string Serialize()
        {
            return ObjectSerializer.Serialize(_profiles);
        }
    }
}
