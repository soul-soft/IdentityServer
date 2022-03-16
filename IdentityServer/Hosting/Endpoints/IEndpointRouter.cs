using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    /// <summary>
    /// 终结点路由
    /// </summary>
    public interface IEndpointRouter
    {
        IEndpointHandler? Routing(HttpContext context);
    }
}
