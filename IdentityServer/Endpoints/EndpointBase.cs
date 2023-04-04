using Microsoft.AspNetCore.Http;
using System.Net;

namespace IdentityServer.Endpoints
{
    public abstract class EndpointBase : IEndpointHandler
    {
        public abstract Task<IEndpointResult> HandleAsync(HttpContext context);

        protected static IEndpointResult Challenge()
        {
            return new ChallengeResult();
        }

        protected static IEndpointResult Redirect(string url)
        {
            return new RedirectResult(url);
        }

        protected static IEndpointResult BadRequest(string error, string? errorDescription)
        {
            return new BadRequestResult(error, errorDescription);
        }

        protected static IEndpointResult Json(string json)
        {
            return new JsonResult(json);
        }

        protected static IEndpointResult Unauthorized(string error, string? errorDescription)
        {
            return new UnauthorizedResult(error, errorDescription);
        }

        protected static IEndpointResult MethodNotAllowed()
        {
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }

        protected static IEndpointResult OK()
        {
            return new StatusCodeResult(HttpStatusCode.OK);
        }
    }
}
