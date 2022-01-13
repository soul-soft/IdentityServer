namespace IdentityServer.Validation
{
    public class ResourceOwnerPasswordGrantValidationContext
    {
        public ValidatedRequest Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public ResourceOwnerPasswordGrantValidationContext(ValidatedRequest request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
