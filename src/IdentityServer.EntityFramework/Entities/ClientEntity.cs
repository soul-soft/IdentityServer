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
        public ICollection<SecretEntity> Secrets { get; set; } = new HashSet<SecretEntity>();
        public ICollection<StringEntity> AllowedScopes { get; set; } = new HashSet<StringEntity>();
        public ICollection<StringEntity> AllowedGrantTypes { get; set; } = new HashSet<StringEntity>();
        public ICollection<StringEntity> AllowedRedirectUris { get; set; } = new HashSet<StringEntity>();
        public ICollection<StringEntity> AllowedSigningAlgorithms { get; set; } = new HashSet<StringEntity>();
        public ICollection<PropertyEntity> Properties { get; set; } = new HashSet<PropertyEntity>();

        public Client Cast()
        {
            return new Client()
            {
                ClientId = ClientId,
                ClientName = ClientName,
                Description = Description,
                ClientUri = ClientUri,
                Enabled = Enabled,
                AuthorizeCodeLifetime = AuthorizeCodeLifetime,
                RequireSecret = RequireSecret,
                OfflineAccess = OfflineAccess,
                AccessTokenType = AccessTokenType,
                AccessTokenLifetime = AccessTokenLifetime,
                RefreshTokenLifetime = RefreshTokenLifetime,
                IdentityTokenLifetime = IdentityTokenLifetime,
                AllowedGrantTypes = AllowedGrantTypes.Select(s => s.Data).ToArray(),
                AllowedRedirectUris = AllowedRedirectUris.Select(s => s.Data).ToArray(),
                AllowedScopes = AllowedScopes.Select(s => s.Data).ToArray(),
                Properties = Properties.Select(s => new KeyValuePair<string, string>(s.Key, s.Value)).ToArray(),
                AllowedSigningAlgorithms = AllowedSigningAlgorithms.Select(s => s.Data).ToArray(),
                Secrets = Secrets.Select(s => new Secret
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
                Properties = client.Properties.Select(s => new PropertyEntity(s.Key, s.Value)).ToArray(),
                AllowedGrantTypes = client.AllowedGrantTypes.Select(s => new StringEntity(s)).ToArray(),
                AllowedRedirectUris = client.AllowedRedirectUris.Select(s => new StringEntity(s)).ToArray(),
                AllowedScopes = client.AllowedScopes.Select(s => new StringEntity(s)).ToArray(),
                AllowedSigningAlgorithms = client.AllowedSigningAlgorithms.Select(s => new StringEntity(s)).ToArray(),
                Secrets = client.Secrets.Select(s => new SecretEntity(s.Value, s.Expiration, s.Description)).ToArray(),
            };
        }
    }
}
