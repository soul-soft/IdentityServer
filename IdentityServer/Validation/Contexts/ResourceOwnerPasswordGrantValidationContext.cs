namespace IdentityServer.Validation
{
    public class ResourceOwnerPasswordGrantValidationContext
    {
        public ValidatedTokenRequest Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public ResourceOwnerPasswordGrantValidationContext(ValidatedTokenRequest request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
