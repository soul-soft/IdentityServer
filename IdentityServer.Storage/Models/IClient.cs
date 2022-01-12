namespace IdentityServer.Models
{
    public interface IClient
    {
        string ClientId { get; }
        string? ClientName { get; }
        string? Description { get; }
        string? ClientUri { get; }
        bool Enabled { get; }
        bool IncludeJwtId { get; }
        int AccessTokenLifetime { get; }
        int RefreshTokenLifetime { get; }
        int IdentityTokenLifetime { get; }
        AccessTokenType AccessTokenType { get; }
        IReadOnlyCollection<string> AllowedScopes { get; }
        IReadOnlyCollection<ISecret> ClientSecrets { get; }
        IReadOnlyCollection<string> AllowedGrantTypes { get; }
        IReadOnlyCollection<string> AllowedSigningAlgorithms { get; }
    }
}
