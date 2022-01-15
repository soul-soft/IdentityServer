namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidationContext
    {
        public string RefreshToken { get; }
        public ValidatedTokenRequest Request { get; }

        public RefreshTokenGrantValidationContext(string refreshToken, ValidatedTokenRequest request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
