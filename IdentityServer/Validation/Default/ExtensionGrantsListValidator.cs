namespace IdentityServer.Validation
{
    internal class ExtensionGrantsListValidator : IExtensionGrantsListValidator
    {
        private readonly IEnumerable<IExtensionGrantValidator> _extensions;

        public ExtensionGrantsListValidator(IEnumerable<IExtensionGrantValidator> extensions)
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

        public Task<GrantValidationResult> ValidateAsync(ExtensionGrantValidationContext context)
        {
            var validator = GetValidator(context.Request.GrantType);
            if (validator == null)
            {
                return GrantValidationResult.ErrorAsync("The identityserver does not support '{0}' authentication", context.Request.GrantType);
            }
            return validator.ValidateAsync(context);
        }

        private IExtensionGrantValidator? GetValidator(string grantType)
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
