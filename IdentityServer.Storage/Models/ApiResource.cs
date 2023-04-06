﻿namespace IdentityServer.Models
{
    public class ApiResource : Resource
    {
        public bool Required { get; set; } = false;

        public string Scope { get; set; } = default!;

        public ICollection<Secret> ApiSecrets { get; set; } = new HashSet<Secret>();
       
        protected ApiResource()
        {

        }

        public ApiResource(string name, string scope) : base(name)
        {
            Scope = scope;
        }
    }
}
