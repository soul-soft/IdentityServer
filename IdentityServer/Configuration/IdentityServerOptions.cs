namespace IdentityServer.Configuration
{
    public class IdentityServerOptions
    {
        public string Issuer { get; set; } = "identityserver";
        public string? IssuerUri { get; set; }
        public string IdentityProvider { get; set; } = "local";
        public bool IncludeEndpointErrorDetails { get; set; } = true;
        public InputLengthRestrictions InputLengthRestrictions { get; set; } = new InputLengthRestrictions();
        public DiscoveryOptions Discovery { get; set; } = new DiscoveryOptions();
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();
        public bool LowerCaseIssuerUri { get; set; } = true;
        public string? AccessTokenJwtType { get; set; } = "at+jwt";
        public bool EmitScopesAsSpaceDelimitedStringInJwt { get; set; } = true;
        public string TokenEndpointAuthMethod { get; set; } = TokenEndpointAuthMethods.PostBody;
        public AuthenticationOptions AuthenticationOptions { get; set; } = new AuthenticationOptions();
    }
}
