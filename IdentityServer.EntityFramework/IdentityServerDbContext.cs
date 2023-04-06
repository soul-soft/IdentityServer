using IdentityServer.EntityFramework.Configuration;
using IdentityServer.EntityFramework.Entities;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System.Security.Claims;

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
            modelBuilder.Entity<TokenEntity>().Property(a => a.Code).IsRequired();
            modelBuilder.Entity<TokenEntity>().Ignore(a => a.Claims);
            modelBuilder.Entity<TokenEntity>().OwnsMany(a => a.Claims, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
                x.ToTable(options.GetTableName("TokenClaims"));
            });
            #endregion

            #region Client
            modelBuilder.Entity<ClientEntity>().HasKey(a => a.ClientId);
            modelBuilder.Entity<ClientEntity>().Ignore(a => a.ClientSecrets);
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.ClientSecrets, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.AllowedScopes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.AllowedGrantTypes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.AllowedRedirectUris, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<ClientEntity>().OwnsMany(a => a.AllowedSigningAlgorithms, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion

            #region ApiScope
            modelBuilder.Entity<ApiScopeEntity>().HasKey(a => a.Name);
            modelBuilder.Entity<ApiScopeEntity>().OwnsMany(a => a.ClaimTypes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion

            #region ApiResource
            modelBuilder.Entity<ApiResourceEntity>().HasKey(a => a.Name);
            modelBuilder.Entity<ApiResourceEntity>().OwnsMany(a => a.ApiSecrets, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<ApiResourceEntity>().OwnsMany(a => a.ClaimTypes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion

            #region IdentityResource
            modelBuilder.Entity<IdentityResourceEntity>().HasKey(a => a.Name);
            modelBuilder.Entity<IdentityResourceEntity>().OwnsMany(a => a.ClaimTypes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion

            #region AuthorizationCode
            modelBuilder.Entity<AuthorizationCodeEntity>().HasKey(a => a.Code);
            modelBuilder.Entity<AuthorizationCodeEntity>().Ignore(a => a.Claims);
            modelBuilder.Entity<AuthorizationCodeEntity>().OwnsMany(a => a.Claims, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion
        }

        private EntityFrameworkStoreOptions GetOptions()
        {
            return Database.GetService<IOptions<EntityFrameworkStoreOptions>>().Value;
        }
    }
}
