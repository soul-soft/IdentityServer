namespace IdentityServer.Configuration
{
    public class EndpointsOptions
    {
        public bool EnableJwtRequestUri { get; set; } = true;
        public bool EnableTokenEndpoint { get; set; } = true;
        public bool EnableDiscoveryEndpoint { get; set; } = true;
        public bool EnableAuthorizeEndpoint { get; set; } = true;
    }
}
