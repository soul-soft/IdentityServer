using System.Security.Claims;

namespace IdentityServer.Models
{
    public interface IToken
    {
        string? Id { get; }
        string Type { get; }
        string? ClientId { get; }
        string? Issuer { get; }
        int Lifetime { get; }
        string? SubjectId { get; }
        string? SessionId { get; }
        string? GrantType { get; }
        string? Nonce { get; }
        string? Description { get; }
        DateTime CreationTime { get; }
        AccessTokenType AccessTokenType { get; }
        ICollection<string> Scopes { get; }
        ICollection<string> Audiences { get; }
        ICollection<IClaimLite> Claims { get; }
        ICollection<string> AllowedSigningAlgorithms { get; }
    }
}
