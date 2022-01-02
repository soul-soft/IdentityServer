using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class DefaultClientSecretValidator
        : IClientSecretValidator
    {
        private readonly ILogger _logger;
       
        private readonly IClientSecretParser _parser;

        public DefaultClientSecretValidator(
            ILogger<DefaultClientSecretValidator> logger,
            IClientSecretParser parser)
        {
            _logger = logger;
            _providers = providers;
        }

        public async Task<ClientSecretValidationResult> ValidateAsync(HttpContext context)
        {
            foreach (var provider in _providers)
            {
                var secret = await provider.ParseAsync(context);
                if (secret != null)
                {
                    _logger.LogDebug("Parser found secret: {type}", provider.GetType().Name);
                }
            }
        }
    }
}
