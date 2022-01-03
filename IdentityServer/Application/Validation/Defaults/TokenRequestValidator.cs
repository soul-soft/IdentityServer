using System.Collections.Specialized;
using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class TokenRequestValidator
        : ITokenRequestValidator
    {
        private ILogger _logger;
        private readonly IdentityServerOptions _options;
        private readonly IServiceProvider _services;
        private readonly IResourceValidator _resourceValidator;

        public TokenRequestValidator(
            IdentityServerOptions options,
            IServiceProvider services,
            IResourceValidator resourceValidator,
            ILogger<TokenRequestValidator> logger)
        {
            _logger = logger;
            _options = options;
            _services = services;
            _resourceValidator = resourceValidator;
        }

        public async Task<TokenRequestValidationResult> ValidateRequestAsync(NameValueCollection parameters, ClientSecretValidationResult clientSecretValidationResult)
        {
            var result = new TokenRequestValidationResult();
            var grantType = parameters.Get(OidcConstants.TokenRequest.GrantType);
            if (string.IsNullOrWhiteSpace(grantType))
            {
                _logger.LogError("Grant type is missing");
                return Error(OidcConstants.TokenErrors.UnsupportedGrantType);
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                _logger.LogError("Grant type is too long");
                return Error(OidcConstants.TokenErrors.UnsupportedGrantType);
            }
            if (!clientSecretValidationResult.Client.AllowedGrantTypes.Contains(GrantType.ClientCredentials))
            {
                _logger.LogError("Client not authorized for client credentials flow, check the AllowedGrantTypes setting");
                return Error(OidcConstants.TokenErrors.UnauthorizedClient);
            }
            //Validate resource 
            var scopes = parameters.Get(OidcConstants.TokenRequest.Scope) ?? string.Empty;
            var requestScopes = scopes.Split(',')
                .Where(a => !string.IsNullOrWhiteSpace(a));
            var resourceValidationResult = await _resourceValidator.ValidateAsync(new ResourceValidationRequest(clientSecretValidationResult.Client, requestScopes));
            if (resourceValidationResult.IsError)
            {
                _logger.LogError(resourceValidationResult.Description);
                return Error(OidcConstants.TokenErrors.InvalidScope);
            }
            GrantValidationResult grantResult;
            switch (grantType)
            {
                //Validate clientCredentials
                case GrantType.ClientCredentials:
                    var grantContext = new ClientCredentialsGrantRequest(clientSecretValidationResult.Client);
                    var validator = _services.GetRequiredService<IClientCredentialsGrantValidator>();
                    grantResult = await validator.ValidateAsync(grantContext);
                    break;
                //Validate resourceOwnerPassword
                case GrantType.ResourceOwnerPassword:
                    var username = parameters.Get(OidcConstants.TokenRequest.UserName);
                    if (string.IsNullOrWhiteSpace(username))
                    {
                        return Error(OidcConstants.TokenErrors.InvalidGrant);
                    }
                    var password = parameters.Get(OidcConstants.TokenRequest.Password);
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        return Error(OidcConstants.TokenErrors.InvalidGrant);
                    }
                    if (username.Length > _options.InputLengthRestrictions.UserName ||
                        password.Length > _options.InputLengthRestrictions.Password)
                    {
                        return Error(OidcConstants.TokenErrors.InvalidGrant);
                    }
                    var resourceOwnerPasswordGrantRequest = new ResourceOwnerPasswordGrantRequest(
                        clientSecretValidationResult.Client,
                        username,
                        password);
                    var resourceOwnerPasswordGrantValidator = _services.GetRequiredService<IResourceOwnerPasswordGrantValidator>();
                    grantResult = await resourceOwnerPasswordGrantValidator.ValidateAsync(resourceOwnerPasswordGrantRequest);
                    break;
                //Validate extensionGrant
                default:
                    var extensionGrantRequest = new ExtensionGrantRequest(clientSecretValidationResult.Client);
                    var extensionGrantValidator = _services.GetRequiredService<IExtensionGrantValidator>();
                    grantResult = await extensionGrantValidator.ValidateAsync(extensionGrantRequest);
                    break;
            }
            if (grantResult.IsError)
            {
                _logger.LogError(grantResult.Description);
                return Error(OidcConstants.TokenErrors.InvalidRequest);
            }
            return Success(
                clientSecretValidationResult.Client,
                resourceValidationResult.Resources);
        }
        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private TokenRequestValidationResult Error(string format, params object[] args)
        {
            var result = new TokenRequestValidationResult();
            result.Error(string.Format(format, args));
            return result;
        }
        /// <summary>
        /// 成功结果
        /// </summary>
        /// <returns></returns>
        private TokenRequestValidationResult Success(Client client, IEnumerable<Resource> resources)
        {
            var result = new TokenRequestValidationResult();
            result.Success(client, resources);
            return result;
        }
    }
}
