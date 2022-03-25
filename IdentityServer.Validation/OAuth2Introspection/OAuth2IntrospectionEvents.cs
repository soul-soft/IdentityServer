using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class OAuth2IntrospectionEvents
    {
        public Func<MessageReceivedContext, Task> OnMessageReceived = context => Task.CompletedTask;
        public Func<TokenValidatedContext, Task> OnTokenValidated = context => Task.CompletedTask;
       
        public virtual Task MessageReceivedAsync(MessageReceivedContext context) => OnMessageReceived(context);
        public virtual Task TokenValidatedAsync(TokenValidatedContext context) => OnTokenValidated(context);
    }
}
