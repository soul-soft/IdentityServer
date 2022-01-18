namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidationContext
    {
        public string RefreshToken { get; }
        public GrantValidationRequest Request { get; }

        public RefreshTokenGrantValidationContext(string refreshToken, GrantValidationRequest request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
