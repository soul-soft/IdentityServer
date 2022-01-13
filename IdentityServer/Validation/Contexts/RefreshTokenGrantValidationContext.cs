namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidationContext
    {
        public string RefreshToken { get; }
        public ValidatedRequest Request { get; }

        public RefreshTokenGrantValidationContext(string refreshToken, ValidatedRequest request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
