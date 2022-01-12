﻿namespace IdentityServer.Validation
{
    public interface ISecretValidator
    {
        string SecretType { get; }
        Task<ValidationResult> ValidateAsync(ClientSecret clientSecret, IEnumerable<ISecret> allowedSecrets);
    }
}
