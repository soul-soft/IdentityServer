namespace IdentityServer.Configuration
{
    public class LocalAuthenticationOptions
    {
        public string Scheme { get; set; } = LocalAuthenticationDefaults.Scheme;
        public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();
    }
}
