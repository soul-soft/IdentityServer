namespace IdentityServer.Models
{
    /// <summary>
    /// 资源组
    /// </summary>
    public class ApiScope
    {
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public ApiScope(string name)
        {
            Name = name;
        }
    }
}
