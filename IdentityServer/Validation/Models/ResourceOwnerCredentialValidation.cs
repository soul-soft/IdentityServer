namespace IdentityServer.Validation
{
    public class ResourceOwnerCredentialValidation
    {
        public TokenRequestValidation Request { get; }
      
        public string Username { get; }
        
        public string Password { get; }

        public ResourceOwnerCredentialValidation(TokenRequestValidation request, string username, string password)
        {
            Request = request;
            Username = username;
            Password = password;
        }
    }
}
