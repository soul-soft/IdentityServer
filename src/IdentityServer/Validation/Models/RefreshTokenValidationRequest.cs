using System.Collections.Specialized;

namespace IdentityServer.Validation
{
    public class RefreshTokenValidationRequest: GrantValidationRequest
    {
        public string RefreshToken { get; }

        public RefreshTokenValidationRequest(string refreshToken, GrantValidationRequest request) 
            : base(request.Client, request.GrantType, request.Resources, request.Form, request.Options)
        {
            RefreshToken = refreshToken;
        }
    }
}
