namespace IdentityServer.Validation
{
    public class ClientCredentialsValidation
    {
        public TokenRequestValidation Request { get; }
      
        public ClientCredentialsValidation(TokenRequestValidation request)
        {
            Request = request;
        }
    }
}
