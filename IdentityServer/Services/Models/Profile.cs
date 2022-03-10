namespace IdentityServer.Models
{
    public class Profile
    {
        public string Name { get; private set; }
        public object? Value { get; private set; }

        public Profile(string name, object? value)
        {
            Name = name;
            Value = value;
        }
    }
}
