using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text;

namespace IdentityServer.Endpoints
{
    internal class AuthorizeResponseGenerator : IAuthorizeResponseGenerator
    {
        private readonly IAuthorizeCodeService _authorizeCodeService;

        public AuthorizeResponseGenerator(
            IAuthorizeCodeService authorizeCodeService)
        {
            _authorizeCodeService = authorizeCodeService;
        }

        public async Task<string> GenerateAsync(AuthorizeGeneratorRequest request)
        {
            var code = await _authorizeCodeService.GenerateCodeAsync(request.Client, request.Subject);
            var buffer = new StringBuilder();
            buffer.Append(request.RedirectUri);
            buffer.AppendFormat("?{0}={1}", OpenIdConnectParameterNames.Code, code);
            buffer.AppendFormat("&{0}={1}", OpenIdConnectParameterNames.State, request.State);
            return buffer.ToString();
        }
    }
}
