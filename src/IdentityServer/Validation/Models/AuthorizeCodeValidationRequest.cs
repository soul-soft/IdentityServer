namespace IdentityServer.Validation
{
    public class AuthorizeCodeValidationRequest : GrantValidationRequest
    {
        public string Code { get; }
        public string? CodeVerifier { get; }
        
        public AuthorizeCodeValidationRequest(string code, string? codeVerifier, GrantValidationRequest request)
            : base(request.Client, request.GrantType, request.Resources, request.Form, request.Options)
        {
            Code = code;
            CodeVerifier = codeVerifier;
        }
    }
}
