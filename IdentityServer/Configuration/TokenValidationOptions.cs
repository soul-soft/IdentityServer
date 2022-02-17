namespace IdentityServer.Configuration
{
    public class TokenValidationOptions
    {
        public bool ValidateIssuer { get; set; } = true;
        public string? Audience { get; set; }
        public bool ValidateAudience { get; set; } = false;
        public bool ValidateLifetime { get; set; } = true;
        public bool ValidateScope { get; set; } = false;
        public string? ValidScope { get; set; }
        public string[] ValidScopes { get; set; } = Array.Empty<string>();
    }
}
