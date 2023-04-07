namespace IdentityServer.EntityFramework.Entities
{
    public class StringEntity
    {
        public string Data { get; set; } = default!;

        protected StringEntity()
        {

        }

        public StringEntity(string data)
        {
            Data = data;
        }
    }
}
