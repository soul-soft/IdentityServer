namespace IdentityServer.Validation
{
    public class AuthorizeCodeValidationRequest : GrantValidationRequest
    {
        public string Code { get; }

        public AuthorizeCodeValidationRequest(string code, GrantValidationRequest request) 
            : base(request.Client, request.GrantType, request.Resources, request.Form, request.Options)
        {
            Code = code;
        }
    }
}
