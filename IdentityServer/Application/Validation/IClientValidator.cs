using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface IClientValidator
    {
        Task<ValidationResult> ValidateAsync(ClientValidationRequest request);
    }
}
