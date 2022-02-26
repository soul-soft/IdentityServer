namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidationRequest
    {
        public string RefreshToken { get; }
        public TokenGrantValidationRequest Request { get; }

        public RefreshTokenGrantValidationRequest(string refreshToken, TokenGrantValidationRequest request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
