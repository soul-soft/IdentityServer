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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using static IdentityServer.Protocols.OpenIdConnectConstants;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;
        private readonly IdentityServerOptions _options;
        private readonly ILogger<TokenEndpoint> _logger;
        private readonly ITokenResponseGenerator _generator;
        private readonly ISecretParserProvider _secretParserProvider;
        private readonly IScopeValidator _scopeValidator;
        private readonly IResourceValidator _resourceValidator;
        private readonly ISecretValidatorProvider _secretValidatorProvider;

        public TokenEndpoint(
            IClientStore clients,
            IResourceStore resources,
            IdentityServerOptions options,
            ILogger<TokenEndpoint> logger,
            ITokenResponseGenerator generator,
            IScopeValidator scopeValidator,
            IResourceValidator resourceValidator,
            ISecretParserProvider secretParserProvider,
            ISecretValidatorProvider secretValidatorProvider)
        {
            _logger = logger;
            _clients = clients;
            _options = options;
            _resources = resources;
            _generator = generator;
            _scopeValidator = scopeValidator;
            _resourceValidator = resourceValidator;
            _secretParserProvider = secretParserProvider;
            _secretValidatorProvider = secretValidatorProvider;
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

            #region Get Client And Secret
            var secret = await GetSecretAsnc(context);
            if (secret == null)
            {
                _logger.LogError("The client with secret cannot be found in the '{0}' authorization method", _options.TokenEndpointAuthMethod);
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient);
            }
            var client = await _clients.FindClientByIdAsync(secret.Id);
            if (client == null)
            {
                _logger.LogError("No client with id '{clientId}' found.", secret.Id);
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient);
            }
            #endregion

            #region Validate Client And Secret
            var validationResult = await ValidateClientSecretAsync(client, secret);
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.UnauthorizedClient);
            }
            #endregion

            #region Validate Scopes
            var form = await context.Request.ReadFormAsync();
            var scopes = GetScopes(client, form);
            validationResult = await ValidateScopeAsync(client, scopes);
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope);
            }
            #endregion

            #region Validate GrantType
            var grantType = form[OpenIdConnectParameterNames.GrantType].FirstOrDefault();
            validationResult = ValidateGrantType(client, grantType);
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope);
            }
            #endregion

            #region Validate Resources
            var resources = await GetResourcesAsync(scopes);
            validationResult = await _resourceValidator.ValidateAsync(resources,scopes);
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope);
            }
            #endregion

            #region Validate Grant
            switch (grantType)
            {
                case GrantTypes.ClientCredentials:
                    validationResult = ValidateClientCredentialsRequest(resources);
                    break;
                //case GrantTypes.Password:
                //    validationResult = ValidateResourceOwnerCredentialRequestAsync(context);
                default:
                    break;
            }
            if (validationResult.IsError)
            {
                LogError(validationResult.Description, client.ClientId);
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope);
            }
            #endregion

            #region Generator Response
            var response = await _generator.ProcessAsync(new TokenCreationRequest(client, resources)
            {
                Scopes = scopes,
            });
            return TokenResult(response);
            #endregion
        }


        private async Task<ClientSecret?> GetSecretAsnc(HttpContext context)
        {
            var secretParser = _secretParserProvider.GetParser();
            var clientSecret = await secretParser.ParseAsync(context);
            return clientSecret;
        }

        private string[] GetScopes(IClient client, IFormCollection form)
        {
            var scope = form[OpenIdConnectParameterNames.Scope].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(scope))
            {
                scope = string.Join(",", client.AllowedScopes);
            }
            return scope.Split(",")
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .ToArray();
        }

        private async Task<Resources> GetResourcesAsync(string[] scopes)
        {
            var resources = await _resources.FindResourcesByScopeAsync(scopes);
            var enabledResources = resources.Where(a => a.Enabled);
            return new Resources(resources);
        }

        private async Task<ValidationResult> ValidateClientSecretAsync(IClient client, ClientSecret clientSecret)
        {
            if (!client.Enabled)
            {
                return ValidationResult.Error("Client not enabled");
            }
            var validator = _secretValidatorProvider.GetSecretValidator(clientSecret.Type);
            return await validator.ValidateAsync(client.ClientSecrets, clientSecret);
        }

        private async Task<ValidationResult> ValidateScopeAsync(IClient client, string[] scopes)
        {
            var scope = string.Join("", scopes);
            if (scope.Length > _options.InputLengthRestrictions.Scope)
            {
                _logger.LogError("Scope parameter exceeds max allowed length");
                return ValidationResult.Error(OpenIdConnectTokenErrors.InvalidScope);
            }
            if (client.AllowedScopes.Count == 0)
            {
                return ValidationResult.Error("No allowed scopes configured for client");
            }
            if (scopes.Count() == 0)
            {
                return ValidationResult.Error("No scopes found in request");
            }
            return await _scopeValidator.Validate(client.AllowedScopes, scopes);
        }

        private ValidationResult ValidateGrantType(IClient client, string? grantType)
        {
            if (string.IsNullOrEmpty(grantType))
            {
                return ValidationResult.Error("Grant Type is missing");
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                return ValidationResult.Error("Grant type is too long");
            }
            if (!client.AllowedGrantTypes.Contains(grantType))
            {
                return ValidationResult.Error("Client not authorized for {0} flow, check the AllowedGrantTypes setting", grantType);
            }
            return ValidationResult.Success();
        }

        private ValidationResult ValidateClientCredentialsRequest(Resources resources)
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
