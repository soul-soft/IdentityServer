using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Application
{
    internal class DefaultClientSecretValidator
        : IClientSecretValidator
    {
        private readonly ISystemClock _clock;

        public DefaultClientSecretValidator(
            ISystemClock clock)
        {
            _clock = clock;
        }

        public Task ValidateAsync(ClientSecretValidationContext context)
        {
            var client = context.Client;
            //Validate Client
            if (!client.Enabled)
            {
                context.Fail("Invalid client");
                return Task.CompletedTask;
            }
            //Validate Secret
            var parsedSecret = context.ParsedSecret;

            if (parsedSecret.Type == IdentityServerConstants.ParsedSecretTypes.NoSecret)
            {
                context.Success();
                return Task.CompletedTask;
            }
            var flag = client.ClientSecrets
                .Where(a => a.Enabled)
                .Where(a => a.Expiration.HasValue && a.Expiration.Value.HasExpired(_clock.UtcNow.UtcDateTime))
                .Where(a => a.Credential == parsedSecret.Credential)
                .Any();
            if (flag)
            {
                context.Success();
                return Task.CompletedTask;
            }
            context.Fail("Invalid secret");
            return Task.CompletedTask;
        }
    }
}
