namespace IdentityServer.Validation
{
    internal class ExtensionGrantListValidator: IExtensionGrantListValidator
    {
        private readonly IEnumerable<IExtensionGrantValidator> _extensions;

        public ExtensionGrantListValidator(IEnumerable<IExtensionGrantValidator> extensions)
        {
            _extensions = extensions;
        }

        public Task ValidateAsync(ExtensionGrantRequestValidation context)
        {
            var validator = _extensions
                .Where(a => a.GrantType == context.Request.GrantType)
                .FirstOrDefault();
            if (validator == null)
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidRequest, $"Unsupported grant type '{context.Request.GrantType}'");
            }
            return validator.ValidateAsync(context);
        }

        public IEnumerable<string> GetGrantTypes()
        {
            return _extensions.Select(a => a.GrantType).ToList();
        }
    }
}
