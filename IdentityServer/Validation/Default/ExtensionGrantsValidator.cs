namespace IdentityServer.Validation
{
    internal class ExtensionGrantsValidator: IExtensionGrantsValidator
    {
        private readonly IEnumerable<IExtensionGrantValidator> _extensions;

        public ExtensionGrantsValidator(IEnumerable<IExtensionGrantValidator> extensions)
        {
            _extensions = extensions;
        }

        public IEnumerable<string> GetExtensionGrantTypes()
        {
            foreach (var item in _extensions)
            {
                yield return item.GrantType;
            }
        }

        public IExtensionGrantValidator? ValidateAsync(string grantType)
        {
            foreach (var item in _extensions)
            {
                if (item.GrantType == grantType)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
