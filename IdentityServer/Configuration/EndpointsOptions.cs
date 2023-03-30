namespace IdentityServer.Configuration
{
    public class EndpointsOptions
    {
        public bool EnableAuthorizeEndpoint { get; set; } = true;
        public bool EnableDiscoveryEndpoint { get; set; } = true;
        public bool EnableDiscoveryJwksEndpoint { get; set; } = true;
        public bool EnableTokenEndpoint { get; set; } = true;
        public bool EnableUserInfoEndpoint { get; set; } = true;
        public bool EnableIntrospectionEndpoint { get; set; } = true;
        public string EndpointPathPrefix { get; set; } = "/connect";
      
        public void EnableEndpoint(string name)
        {
            _enableEndpoints.Add(name);
        }
       
        private readonly List<string> _enableEndpoints = new List<string>();

        public bool IsEndpointEnabled(EndpointDescriptor endpoint)
        {
            if (endpoint.Name == OpenIdConnectConstants.EndpointNames.Discovery)
            {
                return EnableDiscoveryEndpoint;
            }
            else if (endpoint.Name == OpenIdConnectConstants.EndpointNames.Authorize)
            {
                return EnableAuthorizeEndpoint;
            }
            else if (endpoint.Name == OpenIdConnectConstants.EndpointNames.DiscoveryJwks)
            {
                return EnableDiscoveryJwksEndpoint;
            }
            else if (endpoint.Name == OpenIdConnectConstants.EndpointNames.Token)
            {
                return EnableTokenEndpoint;
            }
            else if (endpoint.Name == OpenIdConnectConstants.EndpointNames.UserInfo)
            {
                return EnableUserInfoEndpoint;
            }
            else if (endpoint.Name == OpenIdConnectConstants.EndpointNames.Introspection)
            {
                return EnableIntrospectionEndpoint;
            }
            else if (_enableEndpoints.Any(a => a == endpoint.Name))
            {
                return true;
            }
            return false;
        }

        public string GetEndpointFullPath(string path)
        {
            if (path.StartsWith('/'))
            {
                return path;
            }
            return $"{EndpointPathPrefix}/{path}";
        }
    }
}
