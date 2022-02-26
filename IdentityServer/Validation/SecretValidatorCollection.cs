namespace IdentityServer.Validation
{
    public class SecretValidatorCollection
    {
        private readonly IEnumerable<IClientCredentialsValidator> _secretValidators;

        public SecretValidatorCollection(IEnumerable<IClientCredentialsValidator> secretValidators)
        {
            _secretValidators = secretValidators;
        }

        public IClientCredentialsValidator GetSecretValidator(string credentialsType)
        {
            return _secretValidators
                .Where(a => a.CredentialsType == credentialsType)
                .First();
        }

        public Task ValidateAsync(ClientCredentials ClientCredentials, IEnumerable<Secret> allowedSecrets)
        {
            var validator = GetSecretValidator(ClientCredentials.Type);
            return validator.ValidateAsync(ClientCredentials, allowedSecrets);
        }
    }
}
