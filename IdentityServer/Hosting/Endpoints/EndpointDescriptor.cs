namespace IdentityServer.Hosting
{
    public class EndpointDescriptor
    {
        public string Name { get; set; }
        public string RoutePattern { get; set; }
        public Type Handler { get; set; }

        public IList<object> Metadata { get; } = new List<object>();

        public EndpointDescriptor(string name, string path, Type handler)
        {
            Name = name;
            RoutePattern = path;
            Handler = handler;
        }

        public override string ToString()
        {
            return RoutePattern;
        }
    }
}
