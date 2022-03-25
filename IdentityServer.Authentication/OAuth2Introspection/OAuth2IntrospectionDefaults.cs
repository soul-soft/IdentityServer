namespace IdentityServer.Authentication
{
    public class IdentityServerAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Bearer";
        internal const string IntrospectionAuthenticationScheme = "IdentityServerAuthenticationIntrospection";
        internal const string JwtAuthenticationScheme = "IdentityServerAuthenticationJwt";
        internal const string TokenItemsKey = "idsrv4:tokenvalidation:token";
        internal const string EffectiveSchemeKey = "idsrv4:tokenvalidation:effective:";
    }
}
