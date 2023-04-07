namespace IdentityServer.EntityFramework.Entities
{
    public class StringEntity
    {
        public string Value { get; set; } = default!;

        protected StringEntity()
        {

        }

        public StringEntity(string value)
        {
            Value = value;
        }
    }
}
