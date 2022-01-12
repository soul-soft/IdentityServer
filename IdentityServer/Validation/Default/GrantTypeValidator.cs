namespace IdentityServer.Validation
{
    public class GrantTypeValidator : IGrantTypeValidator
    {
        private readonly IdentityServerOptions _options;

        public GrantTypeValidator(IdentityServerOptions options)
        {
            _options = options;
        }

        public Task<ValidationResult> ValidateAsync(string requestedGrantType, IEnumerable<string> allowedGrantTypes)
        {
            if (requestedGrantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                return ValidationResult.ErrorAsync("Grant type is too long");
            }
            if (!allowedGrantTypes.Contains(requestedGrantType))
            {
                return ValidationResult.ErrorAsync("Unsupported grant type:'{0}'", requestedGrantType);
            }
            return ValidationResult.SuccessAsync();
        }
    }
}
