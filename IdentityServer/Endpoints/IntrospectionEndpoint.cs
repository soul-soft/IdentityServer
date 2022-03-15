using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class IntrospectionEndpoint : EndpointBase
    {
        private readonly ITokenValidator _tokenValidator;
        private readonly IApiSecretValidator _apiSecretParsers;
        private readonly IIntrospectionResponseGenerator _generator;

        public IntrospectionEndpoint(
            ITokenValidator tokenValidator,
            IApiSecretValidator apiSecretParsers,
            IIntrospectionResponseGenerator generator)
        {
            _generator = generator;
            _tokenValidator = tokenValidator;
            _apiSecretParsers = apiSecretParsers;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            #region Validate Method
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasFormContentType)
            {
                return BadRequest(OpenIdConnectErrors.InvalidRequest, "Invalid contextType");
            }
            #endregion

            #region Validate ApiSecret
            var apiResource = await _apiSecretParsers.ValidateAsync(context);
            #endregion

            #region Validate Token
            var body = await context.Request.ReadFormAsync();
            var form = body.AsNameValueCollection();
            var token = form.Get("token");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(OpenIdConnectErrors.InvalidRequest, "Token is missing");
            }
            var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(token);
            if (tokenValidationResult.IsError)
            {
                return Unauthorized(tokenValidationResult.Error, tokenValidationResult.ErrorDescription);
            }
            #endregion

            #region Response Generator
            var response = await _generator.ProcessAsync(new IntrospectionRequest(
                tokenValidationResult.IsError,
                apiResource,
                tokenValidationResult.Claims));
            #endregion

            return IntrospectionResult(response);
        }
    }
}
