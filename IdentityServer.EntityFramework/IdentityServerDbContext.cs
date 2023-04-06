using IdentityServer.EntityFramework.Configuration;
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

        public DbSet<Token> Tokens => Set<Token>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Secret> Secrets => Set<Secret>();
        public DbSet<ApiScope> ApiScopes => Set<ApiScope>();
        public DbSet<ApiResource> ApiResources => Set<ApiResource>();
        public DbSet<IdentityResource> IdentityResources => Set<IdentityResource>();
        public DbSet<AuthorizationCode> AuthorizationCodes => Set<AuthorizationCode>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var options = GetOptions();

            #region Token
            modelBuilder.Entity<Token>().HasKey(a => a.Code);
            modelBuilder.Entity<Token>().OwnsMany(a => a.Claims, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion

            #region Client
            modelBuilder.Entity<Client>().HasKey(a => a.ClientId);
            modelBuilder.Entity<Client>().OwnsMany(a => a.ClientSecrets, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<Client>().OwnsMany(a => a.AllowedScopes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<Client>().OwnsMany(a => a.AllowedGrantTypes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<Client>().OwnsMany(a => a.AllowedRedirectUris, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<Client>().OwnsMany(a => a.AllowedSigningAlgorithms, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion

            #region ApiScope
            modelBuilder.Entity<ApiScope>().HasKey(a => a.Name);
            modelBuilder.Entity<ApiScope>().OwnsMany(a => a.ClaimTypes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion

            #region ApiResource
            modelBuilder.Entity<ApiResource>().HasKey(a => a.Name);
            modelBuilder.Entity<ApiResource>().OwnsMany(a => a.ApiSecrets, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            modelBuilder.Entity<ApiResource>().OwnsMany(a => a.ClaimTypes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion

            #region IdentityResource
            modelBuilder.Entity<IdentityResource>().HasKey(a => a.Name);
            modelBuilder.Entity<IdentityResource>().OwnsMany(a => a.ClaimTypes, x =>
            {
                x.WithOwner().HasForeignKey("OwnerId");
                x.Property<int>("Id");
                x.HasKey("Id");
            });
            #endregion

            #region AuthorizationCode
            modelBuilder.Entity<AuthorizationCode>().HasKey(a => a.Code);
            modelBuilder.Entity<AuthorizationCode>().OwnsMany(a => a.Claims, x =>
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
