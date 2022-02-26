namespace IdentityServer.Validation
{
    public class PasswordGrantValidationRequest
    {
        public TokenGrantValidationRequest Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public PasswordGrantValidationRequest(TokenGrantValidationRequest request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
