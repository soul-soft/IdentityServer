namespace IdentityServer.Hosting
{
    public class EndpointDescriptor
    {
        public string Name { get; set; }
        public string RoutePattern { get; set; }
        public Type Handler { get; set; }
        public List<Attribute> Metas { get; set; } = new List<Attribute>();

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
