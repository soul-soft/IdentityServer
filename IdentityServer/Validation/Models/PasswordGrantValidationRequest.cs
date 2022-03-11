namespace IdentityServer.Validation
{
    public class PasswordGrantValidationRequest
    {
        public GrantValidationRequest Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public PasswordGrantValidationRequest(GrantValidationRequest request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
