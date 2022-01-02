using IdentityServer.Models;

namespace IdentityServer.Application
{
    public interface IClientSecretValidator
    {
        Task ValidateAsync(ClientSecretValidationContext context);
    }
}
