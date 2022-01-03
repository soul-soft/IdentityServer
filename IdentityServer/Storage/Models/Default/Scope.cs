namespace IdentityServer.Models
{
    /// <summary>
    /// 资源组
    /// </summary>
    public class Scope
    {
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public Scope(string name)
        {
            Name = name;
        }
    }
}
