namespace IdentityServer.EntityFramework.Entities
{
    public class PropertyEntity
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;

        protected PropertyEntity()
        {

        }
        public PropertyEntity(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
