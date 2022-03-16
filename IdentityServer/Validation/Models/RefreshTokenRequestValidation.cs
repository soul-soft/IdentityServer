namespace IdentityServer.Validation
{
    public class RefreshTokenRequestValidation
    {
        public string RefreshToken { get; }
        public TokenRequestValidation Request { get; }

        public RefreshTokenRequestValidation(string refreshToken, TokenRequestValidation request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
