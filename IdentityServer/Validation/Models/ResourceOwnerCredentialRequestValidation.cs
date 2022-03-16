namespace IdentityServer.Validation
{
    public class ResourceOwnerCredentialRequestValidation
    {
        public TokenRequestValidation Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public ResourceOwnerCredentialRequestValidation(TokenRequestValidation request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
