using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface ITokenRequestValidator
    {
        Task<ValidationResult> ValidateAsync(IClient client, HttpContext context);
    }
}
