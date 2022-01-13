namespace IdentityServer.Validation
{
    public class SecretsValidator: ISecretsListValidator
    {
        private readonly IEnumerable<ISecretValidator> _secretValidators;

        public SecretsValidator(IEnumerable<ISecretValidator> secretValidators)
        {
            _secretValidators = secretValidators;
        }

        public ISecretValidator GetSecretValidator(string secretType)
        {
            return _secretValidators
                .Where(a => a.SecretType == secretType)
                .First();
        }

        public Task<ValidationResult> ValidateAsync(ClientSecret clientSecret, IEnumerable<ISecret> allowedSecrets)
        {
            var validator = GetSecretValidator(clientSecret.Type);
            return validator.ValidateAsync(clientSecret, allowedSecrets);
        }
    }
}
