namespace IdentityServer.Configuration
{
    public class TokenValidationParameters
    {
        public string? ValidAudience { get; set; }
        public bool ValidateAudience { get; set; } = false;
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;
    }
}
