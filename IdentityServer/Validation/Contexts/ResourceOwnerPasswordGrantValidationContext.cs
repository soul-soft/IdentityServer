namespace IdentityServer.Validation
{
    public class ResourceOwnerPasswordGrantValidationContext
    {
        public GrantRequest Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public ResourceOwnerPasswordGrantValidationContext(GrantRequest request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
