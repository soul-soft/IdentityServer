using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Client
{
    public class IdentityServer
    {
        private string address = "https://localhost:8080";
       
        private readonly HttpClient _client;

        private static OpenIdConnectConfiguration? _configuration;

        public IdentityServer(HttpClient client)
        {
            _client = client;
        }

        public async Task<OpenIdConnectConfiguration> GettConfigurationAsync()
        {
            if (_configuration!=null)
            {
                return _configuration;
            }
            var response = await _client.GetAsync($"{address}/.well-known/openid-configuration");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            _configuration = OpenIdConnectConfiguration.Create(json);
            return _configuration;
        }


        public async Task EndSessionAsync(string identityToken)
        {
            var configuration = await GettConfigurationAsync();
            var address = configuration.EndSessionEndpoint;
            var parameters = new Dictionary<string, string>();
            parameters.Add(OpenIdConnectParameterNames.IdTokenHint, identityToken);
            var urlContext = new FormUrlEncodedContent(parameters);
            var response = await _client.PostAsync(address, urlContext);
            response.EnsureSuccessStatusCode();
        }
    }
}
