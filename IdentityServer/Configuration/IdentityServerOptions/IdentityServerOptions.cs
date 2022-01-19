namespace IdentityServer.Configuration
{
    public class IdentityServerOptions
    {
        public string Issuer { get; set; } = "identityserver";
        public string? IssuerUri { get; set; }
        public string IdentityServerName { get; set; } = "local";
        public bool IncludeEndpointErrorDetails { get; set; } = true;
        public InputLengthRestrictions InputLengthRestrictions { get; set; } = new InputLengthRestrictions();
        public DiscoveryOptions Discovery { get; set; } = new DiscoveryOptions();
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();
        public bool LowerCaseIssuerUri { get; set; } = true;
        public string? AccessTokenJwtType { get; set; } = "at+jwt";
        public bool EmitScopesAsSpaceDelimitedStringInJwt { get; set; } = true;
        public string TokenEndpointAuthMethod { get; set; } = TokenEndpointAuthMethods.PostBody;
        public TokenValidationOptions TokenValidations { get; set; } = new TokenValidationOptions();
        public string AuthorizationPolicyName { get; set; } = IdentityServerAuthDefaults.PolicyName;
    }
}
