namespace IdentityServer.Storage.Serialization
{
    public class ClaimLite
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        public ClaimLite(string name, string value, string type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
