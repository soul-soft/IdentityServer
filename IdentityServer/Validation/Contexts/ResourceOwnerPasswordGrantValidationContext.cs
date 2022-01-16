namespace IdentityServer.Validation
{
    public class ResourceOwnerPasswordGrantValidationContext
    {
        public TokenValidatedRequest Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public ResourceOwnerPasswordGrantValidationContext(TokenValidatedRequest request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
