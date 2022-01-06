namespace IdentityServer.Models
{
    public interface IClient
    {
        string ClientId { get; }
        bool Enabled { get; }
        IReadOnlyCollection<ISecret> ClientSecrets { get; }
        IReadOnlyCollection<string> AllowedIdentityTokenSigningAlgorithms { get; }
        bool IncludeJwtId { get; }
        int? AccessTokenLifetime { get; }
        int? IdentityTokenLifetime { get; }
        int? ReferenceTokenLifetime { get; }
        bool RequireClientSecret { get; }
        AccessTokenType AccessTokenType { get; }
        IReadOnlyCollection<string> AllowedScopes { get; }
        IReadOnlyCollection<string> AllowedGrantTypes { get; }
    }
}
