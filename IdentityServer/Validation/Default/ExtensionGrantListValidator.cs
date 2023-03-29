namespace IdentityServer.Validation
{
    internal class ExtensionGrantListValidator: IExtensionGrantListValidator
    {
        private readonly IEnumerable<IExtensionGrantValidator> _extensions;

        public ExtensionGrantListValidator(IEnumerable<IExtensionGrantValidator> extensions)
        {
            _extensions = extensions;
        }

        public Task<ExtensionGrantValidationResult> ValidateAsync(ExtensionGrantValidationRequest request)
        {
            var validator = _extensions
                .Where(a => a.GrantType == request.GrantType)
                .FirstOrDefault();
            if (validator == null)
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, $"Unsupported grant type '{request.GrantType}'");
            }
            return validator.ValidateAsync(request);
        }

        public IEnumerable<string> GetSupportedGrantTypes()
        {
            return _extensions.Select(a => a.GrantType);
        }
    }
}
