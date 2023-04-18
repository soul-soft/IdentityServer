using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Authentication
{
    public class OAuth2IntrospectionOptions : AuthenticationSchemeOptions
    {
        public OAuth2IntrospectionOptions()
        {
            Events = new OAuth2IntrospectionEvents();
        }
        public string Authority { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string MetadataAddress { get; set; } = null!;
        public new OAuth2IntrospectionEvents Events { get; }
        public HttpClient Backchannel { get; set; } = null!;
        public HttpMessageHandler? BackchannelHttpHandler { get; set; }
        public bool RequireHttpsMetadata { get; set; } = true;
        public bool SaveToken { get; set; } = true;
        public string NameClaimType { get; set; } = "name";
        public string RoleClaimType { get; set; } = "role";
        public TimeSpan BackchannelTimeout { get; set; } = TimeSpan.FromMinutes(1);
        public TimeSpan RefreshInterval { get; internal set; } = ConfigurationManager<OpenIdConnectConfiguration>.DefaultRefreshInterval;
        public TimeSpan AutomaticRefreshInterval { get; internal set; } = ConfigurationManager<OpenIdConnectConfiguration>.DefaultAutomaticRefreshInterval;
        public OpenIdConnectConfiguration Configuration { get; set; } = null!;
        public IConfigurationManager<OpenIdConnectConfiguration> ConfigurationManager { get; set; } = null!;
    }
}
