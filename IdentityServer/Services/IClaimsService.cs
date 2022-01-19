using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimsService
    {
        Task<ClaimsPrincipal> CreateSubjectAsync(GrantRequest request, GrantValidationResult result);
    }
}
