using IdentityServer.Endpoints;
using IdentityServer.Models;

namespace IdentityServer.ResponseGenerators
{
    public interface ITokenResponseGenerator
    {
        Task<TokenResponse> ProcessAsync(TokenCreationRequest request);
    }
}
