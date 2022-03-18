using System.Collections.Specialized;

namespace IdentityServer.Validation
{
    public class ResourceOwnerCredentialValidationRequest : GrantValidationRequest
    {
        public string Username { get; }

        public string Password { get; }

        public ResourceOwnerCredentialValidationRequest(string username, string password, GrantValidationRequest request) 
            : base(request.Client, request.GrantType, request.Resources, request.Body, request.Options)
        {
            Username = username;
            Password = password;
        }
    }
}
