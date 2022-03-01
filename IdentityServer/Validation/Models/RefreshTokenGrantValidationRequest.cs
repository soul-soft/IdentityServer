namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidationRequest
    {
        public string RefreshToken { get; }
        public TokenValidationRequest Request { get; }

        public RefreshTokenGrantValidationRequest(string refreshToken, TokenValidationRequest request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
