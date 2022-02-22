namespace IdentityServer.Hosting
{
    public class Endpoint
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Type HandlerType { get; set; }

        public IList<object> Metadata { get; } = new List<object>();

        public Endpoint(string name, string path, Type handler)
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
