namespace IdentityServer.Validation
{
    public class SecretValidatorCollection
    {
        private readonly IEnumerable<ISecretValidator> _secretValidators;

        public SecretValidatorCollection(IEnumerable<ISecretValidator> secretValidators)
        {
            _secretValidators = secretValidators;
        }

        public ISecretValidator GetSecretValidator(string credentialsType)
        {
            return _secretValidators
                .Where(a => a.CredentialsType == credentialsType)
                .First();
        }

        public Task ValidateAsync(ParsedSecret ClientCredentials, IEnumerable<Secret> allowedSecrets)
        {
            var validator = GetSecretValidator(ClientCredentials.Type);
            return validator.ValidateAsync(ClientCredentials, allowedSecrets);
        }
    }
}
