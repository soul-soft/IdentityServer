namespace IdentityServer.Hosting
{
    public class EndpointDescriptor
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Type HandlerType { get; set; }

        public EndpointDescriptor(string name, string uri, Type handler)
        {
            Name = name;
            Path = uri;
            HandlerType = handler;
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
