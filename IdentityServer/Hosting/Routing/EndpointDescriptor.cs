namespace IdentityServer.Hosting
{
    public class EndpointDescriptor
    {
        public string Name { get; set; }
        public string Pattern { get; set; }
        public Type Handler { get; set; }
        public EndpointDescriptor(string name, string path, Type handler)
        {
            Name = name;
            Pattern = path;
            Handler = handler;
        }

        public override string ToString()
        {
            return Pattern;
        }
    }
}
