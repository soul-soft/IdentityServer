namespace IdentityServer.Application
{
    internal class ClientValidator : IClientValidator
    {

        public ClientValidator()
        {

        }

        public Task<ValidationResult> ValidateAsync(ClientValidationRequest request)
        {
            if (!request.Client.Enabled)
            {
                return ValidationResult.ErrorAsync("No client with id '{clientId}' found. aborting", request.Client.ClientId);
            }
            return ValidationResult.SuccessAsync();
        }

    }
}
