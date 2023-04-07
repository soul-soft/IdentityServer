using IdentityServer.EntityFramework.Entities;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;

namespace IdentityServer.EntityFramework
{
    public class IdentityServerDbContext : DbContext
    {
        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options)
            : base(options)
        {

        }

        public DbSet<TokenEntity> Tokens => Set<TokenEntity>();
        public DbSet<ClientEntity> Clients => Set<ClientEntity>();
        public DbSet<ApiScopeEntity> ApiScopes => Set<ApiScopeEntity>();
        public DbSet<ApiResourceEntity> ApiResources => Set<ApiResourceEntity>();
        public DbSet<IdentityResourceEntity> IdentityResources => Set<IdentityResourceEntity>();
        public DbSet<AuthorizationCodeEntity> AuthorizationCodes => Set<AuthorizationCodeEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var options = GetOptions();

            #region Token
            modelBuilder.Entity<TokenEntity>().ToTable(options.GetTableName("Tokens"));
            modelBuilder.Entity<TokenEntity>().HasKey(a => a.Code);
            modelBuilder.Entity<TokenEntity>().Property(a => a.Code).HasMaxLength(options.KeyMaxLength);
            modelBuilder.Entity<TokenEntity>().OwnsMany(a => a.Claims, x =>
            {
                x.ToTable(options.GetTableName("TokenClaims"));
                x.WithOwner().HasForeignKey("OwnerId");
            });
            #endregion

            #region Client
            modelBuilder.Entity<ClientEntity>().ToTable(options.GetTableName("Clients"));
            modelBuilder.Entity<ClientEntity>().HasKey(a => a.ClientId);
            modelBuilder.Entity<ClientEntity>().Property(a => a.ClientId).HasMaxLength(options.KeyMaxLength);
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.Secrets, x =>
            {
                x.ToTable(options.GetTableName("ClientSecrets"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.AllowedScopes, x =>
            {
                x.ToTable(options.GetTableName("ClientAllowedScopes"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.AllowedGrantTypes, x =>
            {
                x.ToTable(options.GetTableName("ClientAllowedGrantTypes"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.AllowedRedirectUris, x =>
            {
                x.ToTable(options.GetTableName("ClientAllowedRedirectUris"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.AllowedSigningAlgorithms, x =>
            {
                x.ToTable(options.GetTableName("ClientAllowedSigningAlgorithms"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.Properties, x =>
            {
                x.ToTable(options.GetTableName("ClientProperties"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            #endregion

            #region ApiScope
            modelBuilder.Entity<ApiScopeEntity>().ToTable(options.GetTableName("ApiScopes"));
            modelBuilder.Entity<ApiScopeEntity>().HasKey(a => a.Name);
            modelBuilder.Entity<ApiScopeEntity>().Property(a => a.Name).HasMaxLength(options.KeyMaxLength);
            modelBuilder.Entity<ApiScopeEntity>().OwnsMany(a => a.ClaimTypes, x =>
            {
                x.ToTable(options.GetTableName("ApiScopeClaimTypes"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            #endregion

            #region ApiResource
            modelBuilder.Entity<ApiResourceEntity>().ToTable(options.GetTableName("ApiResources"));
            modelBuilder.Entity<ApiResourceEntity>().HasKey(a => a.Name);
            modelBuilder.Entity<ApiResourceEntity>().Property(a => a.Name).HasMaxLength(options.KeyMaxLength);
            modelBuilder.Entity<ApiResourceEntity>().OwnsMany(a => a.Secrets, x =>
            {
                x.ToTable(options.GetTableName("ApiResourceSecrets"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            modelBuilder.Entity<ApiResourceEntity>().OwnsMany(a => a.ClaimTypes, x =>
            {
                x.ToTable(options.GetTableName("ApiResourceClaimTypes"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            modelBuilder.Entity<ApiResourceEntity>().OwnsMany(a => a.AllowedScopes, x =>
            {
                x.ToTable(options.GetTableName("ApiResourceAllowedScopes"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            modelBuilder.Entity<ApiResourceEntity>().OwnsMany(a => a.Properties, x =>
            {
                x.ToTable(options.GetTableName("ApiResourceProperties"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            #endregion

            #region IdentityResource
            modelBuilder.Entity<IdentityResourceEntity>().ToTable(options.GetTableName("IdentityResources"));
            modelBuilder.Entity<IdentityResourceEntity>().HasKey(a => a.Name);
            modelBuilder.Entity<IdentityResourceEntity>().Property(a => a.Name).HasMaxLength(options.KeyMaxLength);
            modelBuilder.Entity<IdentityResourceEntity>().OwnsMany(a => a.ClaimTypes, x =>
            {
                x.ToTable(options.GetTableName("IdentityResourceClaimTypes"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            #endregion

            #region AuthorizationCode
            modelBuilder.Entity<AuthorizationCodeEntity>().ToTable(options.GetTableName("AuthorizationCodes"));
            modelBuilder.Entity<AuthorizationCodeEntity>().HasKey(a => a.Code);
            modelBuilder.Entity<AuthorizationCodeEntity>().Property(a => a.Code).HasMaxLength(options.KeyMaxLength);
            modelBuilder.Entity<AuthorizationCodeEntity>().OwnsMany(a => a.Claims, x =>
            {
                x.ToTable(options.GetTableName("AuthorizationCodeClaims"));
                x.WithOwner().HasForeignKey("OwnerId");
                x.HasKey("Id");
                x.Property<int>("Id");
            });
            #endregion
        }

        private EntityFrameworkStoreOptions GetOptions()
        {
            return Database.GetService<IOptions<EntityFrameworkStoreOptions>>().Value;
        }
    }
}
