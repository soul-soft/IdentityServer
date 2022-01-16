namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidationContext
    {
        public string RefreshToken { get; }
        public TokenValidatedRequest Request { get; }

        public RefreshTokenGrantValidationContext(string refreshToken, TokenValidatedRequest request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
