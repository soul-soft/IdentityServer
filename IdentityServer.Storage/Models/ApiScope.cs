namespace IdentityServer.Models
{
    public class ApiScope : Resource, IScope
    {
        public string Scope => Name;
        public bool Required { get; set; } = false;
        public bool Emphasize { get; set; } = false;
        public ApiScope(string name) : base(name)
        {

        }
    }
}
