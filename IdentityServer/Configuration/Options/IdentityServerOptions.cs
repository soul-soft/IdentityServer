using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityServer.Configuration
{
    public class IdentityServerOptions
    {
        public DiscoveryOptions Discovery { get; set; } = new DiscoveryOptions();
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();
        public string? IssuerUri { get; set; }
        public bool LowerCaseIssuerUri { get; set; } = true;

        public IdentityServerOptions()
        {
            JsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
    }
}
