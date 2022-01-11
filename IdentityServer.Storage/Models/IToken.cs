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
        ICollection<string> Scopes { get; }
        ICollection<string> Audiences { get; }
        ICollection<string> AllowedSigningAlgorithms { get; }
        DateTime CreationTime { get; }
    }
}
