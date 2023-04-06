using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EntityFramework.Configuration
{
    public class EntityFrameworkStoreOptions
    {
        public string TokenTableName { get; set; } = "Tokens";
        public string ClientTableName { get; set; } = "Clients";
        public string ApiScopeTableName { get; set; } = "ApiScopes";
        public string ApiResourceTableName { get; set; } = "ApiResources";
        public string IdentityResourceTableName { get; set; } = "IdentityResources";
        public string AuthorizationCodeTableName { get; set; } = "AuthorizationCodes";
        public Action<DbContextOptionsBuilder>? ConfigureDbContextOptions { get; set; } 
    }
}
