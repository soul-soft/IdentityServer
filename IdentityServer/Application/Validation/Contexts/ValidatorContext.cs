namespace IdentityServer.Application
{
    public abstract class ValidatorContext
    {
        public bool IsError { get; private set; }
        public string? Description { get; private set; }
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();
     
        public void Fail(string? description)
        {
            Description = description;
            IsError = true;
        }

        public void Success()
        {
            IsError = false;
        }
    }
}
