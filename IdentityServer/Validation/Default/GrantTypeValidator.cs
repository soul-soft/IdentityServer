namespace IdentityServer.Validation
{
    public class GrantTypeValidator : IGrantTypeValidator
    {
        private readonly IdentityServerOptions _options;

        public GrantTypeValidator(IdentityServerOptions options)
        {
            _options = options;
        }

        public Task ValidateAsync(string requestedGrantType, IEnumerable<string> allowedGrantTypes)
        {
            if (requestedGrantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                throw new InvalidGrantException("Grant type is too long");
            }
            if (!allowedGrantTypes.Contains(requestedGrantType))
            {
                throw new InvalidGrantException(string.Format("The client does not allow '{0}' authorization", requestedGrantType));
            }
            return Task.CompletedTask;
        }
    }
}
