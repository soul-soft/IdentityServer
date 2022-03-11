using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation.Default
{
    internal class ApiSecretValidator : IApiSecretValidator
    {
        private readonly SecretParserCollection _clientCredentialsParsers;
      
        public ApiSecretValidator(SecretParserCollection clientCredentialsParsers)
        {
            _clientCredentialsParsers = clientCredentialsParsers;
        }
        
        public async Task ValidateAsync(HttpContext context)
        {
            var credentials = await _clientCredentialsParsers.ParseAsync(context);
        }
    }
}
