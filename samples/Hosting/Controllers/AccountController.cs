using Hosting.Models;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hosting.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        
        public IActionResult Login(string returnUrl)
        {
            return View(new { ReturnUrl  = returnUrl });
        }

        /// <summary>
        /// 第一种登入方式
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login1([FromForm]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Subject,"10")
                }, "password");
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity),new AuthenticationProperties 
                {
                    AllowRefresh= true,
                    IsPersistent=true,
                    ExpiresUtc=DateTimeOffset.UtcNow.AddMinutes(10)
                });
            }
            if (model.ReturnUrl != null)
            {
                return Redirect(model.ReturnUrl);
            }
            return Redirect("/");
        }
    }
}
