using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Models;

namespace IdentityServer.Application
{
    internal class ClientCredentialsGrantValidator
        : IClientCredentialsGrantValidator
    {
        public Task<GrantValidationResult> ValidateAsync(ClientCredentialsGrantRequest context)
        {
            if (!context.Client.AllowedGrantTypes.Contains(GrantType.ClientCredentials))
            {
                return Invalid(OidcConstants.TokenErrors.UnauthorizedClient);
            }
        }

        private GrantValidationResult Invalid(string message)
        {
            var result = new GrantValidationResult();
            result.Error(message);
            return result;
        }
    }
}
