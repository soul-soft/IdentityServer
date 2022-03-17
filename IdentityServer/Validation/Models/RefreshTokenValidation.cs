namespace IdentityServer.Validation
{
    public class RefreshTokenValidation
    {
        public string RefreshToken { get; }
        public TokenRequestValidation Request { get; }

        public RefreshTokenValidation(string refreshToken, TokenRequestValidation request)
        {
            RefreshToken = refreshToken;
            Request = request;
        }
    }
}
