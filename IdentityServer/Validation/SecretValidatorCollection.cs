namespace IdentityServer.Validation
{
    public class SecretValidatorCollection
    {
        private readonly IEnumerable<ISecretValidator> _secretValidators;

        public SecretValidatorCollection(IEnumerable<ISecretValidator> secretValidators)
        {
            _secretValidators = secretValidators;
        }

        public ISecretValidator GetSecretValidator(string secretType)
        {
            return _secretValidators
                .Where(a => a.SecretType == secretType)
                .First();
        }

        public Task ValidateAsync(ClientSecret clientSecret, IEnumerable<ISecret> allowedSecrets)
        {
            var validator = GetSecretValidator(clientSecret.Type);
            return validator.ValidateAsync(clientSecret, allowedSecrets);
        }
    }
}
