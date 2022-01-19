namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidationContext
    {
        public string RefreshToken { get; }
        public GrantRequest Request { get; }

        public RefreshTokenGrantValidationContext(string refreshToken, GrantRequest request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
