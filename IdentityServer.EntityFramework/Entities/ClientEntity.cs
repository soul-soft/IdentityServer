using IdentityServer.Models;

namespace IdentityServer.EntityFramework.Entities
{
    public class ClientEntity : Entity
    {
        public string ClientId { get; set; } = null!;
        public string? ClientName { get; set; }
        public string? Description { get; set; }
        public string? ClientUri { get; set; }
        public bool Enabled { get; set; } = true;
        public int AuthorizeCodeLifetime { get; set; } = 180;
        public int AccessTokenLifetime { get; set; } = 3600;
        public int RefreshTokenLifetime { get; set; } = 3600 * 24 * 30;
        public int IdentityTokenLifetime { get; set; } = 300;
        public bool RequireSecret { get; set; } = true;
        public bool OfflineAccess { get; set; } = false;
        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;
        public ICollection<SecretEntity> Secrets { get; set; } = Array.Empty<SecretEntity>();
        public ICollection<StringEntity> AllowedScopes { get; set; } = Array.Empty<StringEntity>();
        public ICollection<StringEntity> AllowedGrantTypes { get; set; } = Array.Empty<StringEntity>();
        public ICollection<StringEntity> AllowedRedirectUris { get; set; } = Array.Empty<StringEntity>();
        public ICollection<StringEntity> AllowedSigningAlgorithms { get; set; } = Array.Empty<StringEntity>();

        public static implicit operator Client?(ClientEntity? entity)
        {
            if (entity == null) return null;

            return new Client()
            {
                ClientId = entity.ClientId,
                ClientName = entity.ClientName,
                Description = entity.Description,
                ClientUri = entity.ClientUri,
                Enabled = entity.Enabled,
                AuthorizeCodeLifetime = entity.AuthorizeCodeLifetime,
                RequireSecret = entity.RequireSecret,
                OfflineAccess = entity.OfflineAccess,
                AccessTokenType = entity.AccessTokenType,
                AccessTokenLifetime = entity.AccessTokenLifetime,
                RefreshTokenLifetime = entity.RefreshTokenLifetime,
                IdentityTokenLifetime = entity.IdentityTokenLifetime,
                AllowedGrantTypes = entity.AllowedGrantTypes.Select(s => s.Value).ToArray(),
                AllowedRedirectUris = entity.AllowedRedirectUris.Select(s => s.Value).ToArray(),
                AllowedScopes = entity.AllowedScopes.Select(s => s.Value).ToArray(),
                AllowedSigningAlgorithms = entity.AllowedSigningAlgorithms.Select(s => s.Value).ToArray(),
                Secrets = entity.Secrets.Select(s => new Secret
                {
                    Value = s.Value,
                    Expiration = s.Expiration,
                    Description = s.Description,
                }).ToArray(),
            };
        }

        public static implicit operator ClientEntity(Client client)
        {
            return new ClientEntity()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                Description = client.Description,
                ClientUri = client.ClientUri,
                Enabled = client.Enabled,
                AuthorizeCodeLifetime = client.AuthorizeCodeLifetime,
                RequireSecret = client.RequireSecret,
                OfflineAccess = client.OfflineAccess,
                AccessTokenType = client.AccessTokenType,
                AccessTokenLifetime = client.AccessTokenLifetime,
                RefreshTokenLifetime = client.RefreshTokenLifetime,
                IdentityTokenLifetime = client.IdentityTokenLifetime,
                AllowedGrantTypes = client.AllowedGrantTypes.Select(s => new StringEntity(s)).ToArray(),
                AllowedRedirectUris = client.AllowedRedirectUris.Select(s => new StringEntity(s)).ToArray(),
                AllowedScopes = client.AllowedScopes.Select(s => new StringEntity(s)).ToArray(),
                AllowedSigningAlgorithms = client.AllowedSigningAlgorithms.Select(s => new StringEntity(s)).ToArray(),
                Secrets = client.Secrets.Select(s => new SecretEntity(s.Value, s.Expiration, s.Description)).ToArray(),
            };
        }
    }
}
