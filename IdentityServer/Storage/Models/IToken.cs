using System.Security.Claims;

namespace IdentityServer.Models
{
    public interface IToken
    {
        string Issuer { get; }
        string Type { get; }
        string ClientId { get; }
        int? Lifetime { get; }
        string? JwtId { get; }
        string? SubjectId { get; }
        string? SessionId { get; }
        string? Nonce { get; }
        AccessTokenType AccessTokenType { get; }
        string? Description { get; }
        IReadOnlyCollection<string> Scopes { get; }
        IReadOnlyCollection<string> Audiences { get; }
        IReadOnlyCollection<string> AllowedSigningAlgorithms { get; }
        DateTime CreationTime { get; }
    }
}
