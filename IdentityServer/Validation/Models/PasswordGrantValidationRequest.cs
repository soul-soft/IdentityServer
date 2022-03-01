namespace IdentityServer.Validation
{
    public class PasswordGrantValidationRequest
    {
        public TokenValidationRequest Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public PasswordGrantValidationRequest(TokenValidationRequest request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
