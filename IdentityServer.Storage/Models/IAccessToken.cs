namespace IdentityServer.Models
{
    public interface IAccessToken
    {
        string? Id { get; }
        string? Type { get; }
        string? ClientId { get; }
        string? Issuer { get; }
        int Lifetime { get; }
        string? SubjectId { get; }
        string? SessionId { get; }
        string? GrantType { get; }
        string? Nonce { get; }
        string? Description { get; }
        DateTime? NotBefore { get; }
        DateTime? Expiration { get; }
        AccessTokenType AccessTokenType { get; }
        IReadOnlyCollection<string> Scopes { get; }
        IReadOnlyCollection<string> Audiences { get; }
        IReadOnlyCollection<ClaimLite> Claims { get; }
        IReadOnlyCollection<string> AllowedSigningAlgorithms { get; }
    }
}
