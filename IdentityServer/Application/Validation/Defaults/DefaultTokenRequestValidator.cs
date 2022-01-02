using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class DefaultTokenRequestValidator
        : ITokenRequestValidator
    {
        private readonly ILogger _logger;
        private readonly IdentityServerOptions _options;
        private readonly IClientValidator _clientValidator;

        public DefaultTokenRequestValidator(
            ILogger<DefaultTokenRequestValidator> logger,
            IdentityServerOptions options,
            IClientValidator clientValidator)
        {
            _logger = logger;
            _options = options;
            _clientValidator = clientValidator;
        }

        public async Task ValidateRequestAsync(TokenRequestValidationContext context)
        {
            var parameters = await context.HttpContext.Request.ReadFormAsNameValueCollectionAsync();
            //grantType
            var grantType = parameters.Get(OidcConstants.TokenRequest.GrantType);
            if (string.IsNullOrWhiteSpace(grantType))
            {
                context.Fail(OidcConstants.TokenErrors.UnsupportedGrantType);
                return;
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                context.Fail(OidcConstants.TokenErrors.UnsupportedGrantType);
                return;
            }
            
            var clientValidationContext = new ClientValidationContext(context.HttpContext, null, grantType);
            await _clientValidator.ValidateAsync(clientValidationContext);
            if (clientValidationContext.IsError)
            {
                context.Fail(OidcConstants.TokenErrors.UnsupportedGrantType);
            }
        }
    }
}
