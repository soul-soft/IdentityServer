namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidationRequest
    {
        public string RefreshToken { get; }
        public GrantValidationRequest Request { get; }

        public RefreshTokenGrantValidationRequest(string refreshToken, GrantValidationRequest request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
