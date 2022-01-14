using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Configuration
{
    internal class PostConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly IServiceProvider _sp;

        public PostConfigureJwtBearerOptions(IServiceProvider sp)
        {
            _sp = sp;
        }

        public void PostConfigure(string name, JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                SaveSigninToken = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerValidator = IssuerValidator,
                IssuerSigningKeyResolver = IssuerSigningKeyResolver
            };
        }

        private IEnumerable<SecurityKey> IssuerSigningKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        {
            using (var scope = _sp.CreateScope())
            {
                var credentials = scope.ServiceProvider.GetRequiredService<ISigningCredentialStore>();
                return credentials.GetSecurityKeys();
            }
        }

        string IssuerValidator(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            using (var scope = _sp.CreateScope())
            {
                var url = scope.ServiceProvider.GetRequiredService<IServerUrl>();
                var serverIssuer = url.GetIdentityServerIssuerUri();
                if (!serverIssuer.Equals(serverIssuer))
                {
                    throw new SecurityTokenInvalidIssuerException("IDX10211: Unable to validate issuer. The 'issuer' parameter is null or whitespace");
                }
                return issuer;
            }
        }
    }
}
