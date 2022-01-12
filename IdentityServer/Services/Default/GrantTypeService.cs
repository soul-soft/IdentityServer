namespace IdentityServer.Services
{
    internal class GrantTypeService : IGrantTypeService
    {
        private readonly IEnumerable<IExtensionGrantValidator> _extensions;

        public GrantTypeService(
            IEnumerable<IExtensionGrantValidator> extensions)
        {
            _extensions = extensions;
        }

        public Task<IEnumerable<string>> GetGrantTypeNames()
        {
            var grantTypes = new List<string>();
            grantTypes.Add(GrantTypes.ClientCredentials);
            grantTypes.Add(GrantTypes.Password);
            grantTypes.Add(GrantTypes.RefreshToken);
            foreach (var item in _extensions)
            {
                grantTypes.Add(item.GrantType);
            }
            return Task.FromResult<IEnumerable<string>>(grantTypes);
        }
    }
}
