namespace IdentityServer.Hosting
{
    public class DefaultEndpoint
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Type HandlerType { get; set; }

        public DefaultEndpoint(string name, string path, Type handler)
        {
            Name = name;
            Path = path;
            HandlerType = handler;
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
