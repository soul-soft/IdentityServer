namespace IdentityServer.Configuration
{
    public class EndpointsOptions
    {
        public bool EnableDiscoveryEndpoint { get; set; } = true;
        public bool EnableDiscoveryJwksEndpoint { get; set; } = true;
        public bool EnableTokenEndpoint { get; set; } = true;
        public bool EnableUserInfoEndpoint { get; set; } = true;
        public bool EnableAuthorizeEndpoint { get; set; } = true;
        public bool EnableIntrospectionEndpoint { get; set; } = true;

        public bool IsEndpointEnabled(Endpoint endpoint)
        {
            if (endpoint.Name == Constants.EndpointNames.Discovery)
            {
                return EnableDiscoveryEndpoint;
            }
            else if (endpoint.Name == Constants.EndpointNames.DiscoveryJwks)
            {
                return EnableDiscoveryJwksEndpoint;
            }
            else if (endpoint.Name == Constants.EndpointNames.Token)
            {
                return EnableTokenEndpoint;
            }
            else if (endpoint.Name == Constants.EndpointNames.UserInfo)
            {
                return EnableUserInfoEndpoint;
            }
            else if (endpoint.Name == Constants.EndpointNames.Introspection)
            {
                return EnableIntrospectionEndpoint;
            }
            return false;
        }
    }
}
