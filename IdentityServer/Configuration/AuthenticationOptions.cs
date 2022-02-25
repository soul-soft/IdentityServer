namespace IdentityServer.Configuration
{
    public class AuthenticationOptions
    {
        public string Scheme { get; set; } = LocalAuthenticationDefaults.Scheme;
        public string? ValidAudience { get; set; }
        public bool ValidateAudience { get; set; } = false;
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;
    }
}
