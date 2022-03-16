namespace IdentityServer.Validation
{
    public class ClientCredentialsRequestValidation
    {
        public TokenRequestValidation Request { get; }
      
        public ClientCredentialsRequestValidation(TokenRequestValidation request)
        {
            Request = request;
        }
    }
}
