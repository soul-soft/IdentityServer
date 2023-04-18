using IdentityServer.Validation;

namespace IdentityProvider.IdentityServer.Validators
{
    public class ResourceOwnerCredentialRequestValidator : IResourceOwnerCredentialRequestValidator
    {
        private readonly ILogger _logger;

        public ResourceOwnerCredentialRequestValidator(ILogger<ResourceOwnerCredentialRequestValidator> logger)
        {
            _logger = logger;
        }

        public Task<GrantValidationResult> ValidateAsync(ResourceOwnerCredentialValidationRequest request)
        {
            if (request.Username != "admin")
            {
                _logger.LogWarning("账号错误");
                throw new ValidationException("账号错误");
            }
            if (request.Password != "123")
            {
                _logger.LogWarning("密码错误");
                throw new ValidationException("密码错误");
            }
            return Task.FromResult(new GrantValidationResult("10"));
        }
    }
}
