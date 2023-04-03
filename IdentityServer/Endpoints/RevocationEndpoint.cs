using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class RevocationEndpoint : EndpointBase
    {
        private readonly ITokenStore _tokens;
        private readonly IdentityServerOptions _options;

        public RevocationEndpoint(
            ITokenStore tokens,
            IdentityServerOptions options)
        {
            _tokens = tokens;
            _options = options;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            #region Validate Method
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasFormContentType)
            {
                return BadRequest(ValidationErrors.InvalidRequest, "Invalid contextType");
            }
            #endregion

            #region Parse Parameters
            var body = await context.Request.ReadFormAsync();
            var form = body.AsNameValueCollection();
            #endregion

            #region Parse Token
            var token = form.Get("token");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(ValidationErrors.InvalidRequest, "token is missing");
            }
            if (token.Length > _options.InputLengthRestrictions.AccessToken)
            {
                return BadRequest(ValidationErrors.InvalidRequest, "Grant type is too long");
            }
            #endregion

            #region Parse TokenTypeHint
            var tokenTypeHint = form.Get("token_type_hint");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(ValidationErrors.InvalidRequest, "token_type_hint is missing");
            }
            if (!new string[] {TokenTypes.AccessToken,TokenTypes.RefreshToken}.Contains(tokenTypeHint))
            {
                return BadRequest(ValidationErrors.InvalidRequest, "Invalid token_type_hint");
            }
            #endregion

            #region RevomeToken
            var accessToken = await _tokens.FindAccessTokenAsync(token);
            if (accessToken != null)
            {
                await _tokens.RevomeTokenAsync(accessToken);

            }
            #endregion

            return OK();
        }
    }
}
