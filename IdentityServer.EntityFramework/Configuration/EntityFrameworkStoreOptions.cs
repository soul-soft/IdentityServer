using Microsoft.EntityFrameworkCore;
using System.Text;
namespace IdentityServer.EntityFramework.Configuration
{
    public class EntityFrameworkStoreOptions
    {
        public bool TableNameToLower { get; set; } = true;
        public string TableNamePrefix { get; set; } = string.Empty;
        public Action<DbContextOptionsBuilder>? ConfigureDbContextOptions { get; set; }

        internal string GetTableName(string name)
        {
            if (TableNameToLower)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < name.Length; i++)
                {
                    var item = name[i];
                    if (i > 0 && char.IsUpper(item))
                    {
                        sb.Append('_');
                        sb.Append(char.ToLower(item));
                    }
                    else
                    {
                        sb.Append(item);
                    }
                }
                return $"{TableNamePrefix}{name}";
            }
            return $"{TableNamePrefix}{name}";
        }
    }
}
