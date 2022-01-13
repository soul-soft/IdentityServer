namespace IdentityServer.Configuration
{
    public class IdentityServerOptions
    {
        public string IdentityProvider { get; set; } = "idsv";
        public InputLengthRestrictions InputLengthRestrictions { get; set; } = new InputLengthRestrictions();
        public DiscoveryOptions Discovery { get; set; } = new DiscoveryOptions();
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();
        public string? IssuerUri { get; set; }
        public bool LowerCaseIssuerUri { get; set; } = true;
        public string? AccessTokenJwtType { get; set; } = "at+jwt";
        public bool EmitScopesAsSpaceDelimitedStringInJwt { get; set; } = true;
        public string TokenEndpointAuthMethod { get; set; } = TokenEndpointAuthMethods.PostBody;
    }
}
