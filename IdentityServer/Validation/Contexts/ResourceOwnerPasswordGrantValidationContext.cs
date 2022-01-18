namespace IdentityServer.Validation
{
    public class ResourceOwnerPasswordGrantValidationContext
    {
        public GrantValidationRequest Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public ResourceOwnerPasswordGrantValidationContext(GrantValidationRequest request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
