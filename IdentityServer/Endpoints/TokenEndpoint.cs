using IdentityServer.Configuration;
using IdentityServer.Extensions;
using IdentityServer.Hosting;
using IdentityServer.Models;
using IdentityServer.Protocols;
using IdentityServer.ResponseGenerators;
using IdentityServer.Services;
using IdentityServer.Storage;
using IdentityServer.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using static IdentityServer.OpenIdConnects;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;
        private readonly IdentityServerOptions _options;
        private readonly ILogger<TokenEndpoint> _logger;
        private readonly ITokenResponseGenerator _generator;
        private readonly ISecretsParser _secretsParser;
        private readonly IScopeValidator _scopeValidator;
        private readonly ISecretsValidator _secretsValidator;
        private readonly IResourceValidator _resourceValidator;
        private readonly IGrantTypeValidator _grantTypeValidator;

        public TokenEndpoint(
            IClientStore clients,
            IResourceStore resources,
            ISecretsParser secretsParser,
            IdentityServerOptions options,
            ILogger<TokenEndpoint> logger,
            ITokenResponseGenerator generator,
            IScopeValidator scopeValidator,
            ISecretsValidator secretsValidator,
            IResourceValidator resourceValidator,
            IGrantTypeValidator grantTypeValidator)
        {
            _logger = logger;
            _clients = clients;
            _options = options;
            _resources = resources;
            _generator = generator;
            _secretsParser = secretsParser;
            _scopeValidator = scopeValidator;
            _secretsValidator = secretsValidator;
            _resourceValidator = resourceValidator;
            _grantTypeValidator = grantTypeValidator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            #region ValidateRequest
            if (!_options.Endpoints.EnableTokenEndpoint)
            {
                return MethodNotAllowed();
            }
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasApplicationFormContentType())
            {
                return BadRequest(OpenIdConnectTokenErrors.UnsupportedContextType);
            }
            #endregion

            #region Get Secret
            var secret = await _secretsParser.ParseAsync(context);
            if (secret == null)
            {
                _logger.LogError("The client with secret cannot be found in the '{0}' authorization method", _options.TokenEndpointAuthMethod);
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient, "No client credentials found");
            }
            #endregion

            #region Get Client
            var client = await _clients.FindClientByIdAsync(secret.Id);
            if (client == null)
            {
                _logger.LogError("No client with id '{clientId}' found.", secret.Id);
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient, "No client found");
            }
            #endregion

            #region Validate Secret
            var validationResult = await _secretsValidator.ValidateAsync(secret, client.ClientSecrets);
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.UnauthorizedClient, validationResult.Description);
            }
            #endregion

            #region Get Scopes
            var form = await context.Request.ReadFormAsync();
            var scope = form[OpenIdConnectParameterNames.Scope].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(scope))
            {
                scope = string.Join(",", client.AllowedScopes);
            }
            var scopes = scope.Split(",").Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
            #endregion

            #region Validate Scopes
            validationResult = await _scopeValidator.Validate(client.AllowedScopes, scopes);
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope, validationResult.Description);
            }
            #endregion

            #region Validate GrantType
            var grantType = form[OpenIdConnectParameterNames.GrantType].FirstOrDefault();
            if (string.IsNullOrEmpty(grantType))
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidGrant, "Grant Type is missing");
            }
            await _grantTypeValidator.ValidateAsync(grantType,client.AllowedGrantTypes);
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.InvalidResource, validationResult.Description);
            }
            #endregion

            #region Get Resources
            var resources = await _resources.FindResourcesByScopeAsync(scopes);
            #endregion

            #region Validate Resources
            validationResult = await _resourceValidator.ValidateAsync(resources, scopes);
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.InvalidResource, validationResult.Description);
            }
            #endregion

            #region Validate Grant
            if (GrantTypes.ClientCredentials.Equals(grantType))
            {
                var grantContext = new ClientCredentialsGrantValidationContext(
                    client,
                    resources,
                    scopes);
                var grantValidator = context.RequestServices
                    .GetRequiredService<IClientCredentialsGrantValidator>();
                validationResult = await grantValidator.ValidateAsync(grantContext);
            }
            else if (GrantTypes.Password.Equals(grantType))
            {
                var grantContext = new PasswordGrantContext(
                    client,
                    resources,
                    scopes);
                var grantValidator = context.RequestServices
                   .GetRequiredService<IPasswordGrantValidator>();
                validationResult = await grantValidator.ValidateAsync(grantContext);
            }
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope);
            }
            #endregion

            #region Generator Response
            var response = await _generator.ProcessAsync(new TokenRequest(client, resources)
            {
                Scopes = scopes,
            });
            return TokenResult(response);
            #endregion
        }


        private ValidationResult ValidateClientCredentialsRequest(IClient client, Resources resources)
        {
            if (resources.IdentityResources.Any())
            {
                return ValidationResult.Error("Client cannot request OpenID scopes in client credentials flow");
            }
            return ValidationResult.Success();
        }

        private void LogError(string description, string clientId)
        {
            _logger.LogError($"[{clientId}]" + description);
        }

        //private async Task<ValidationResult> ValidatePasswordRequestAsync(HttpContext context, IFormCollection form)
        //{
        //    var username = form[OpenIdConnectParameterNames.Username].FirstOrDefault();
        //    var password = form[OpenIdConnectParameterNames.Password].FirstOrDefault();
        //    if (string.IsNullOrWhiteSpace(username))
        //    {
        //        return ValidationResult.Error("Username is missing");
        //    }
        //    if (username.Length > _options.InputLengthRestrictions.UserName ||
        //        password?.Length > _options.InputLengthRestrictions.Password)
        //    {
        //        return ValidationResult.Error("Username or password too long");
        //    }
        //    return ValidationResult.Success();
        //}
    }
}
