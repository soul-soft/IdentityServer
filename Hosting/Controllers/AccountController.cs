using Hosting.Models;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hosting.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new { ReturnUrl  = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost([FromForm]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Subject,"10")
                }, "password");
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
            }
            if (model.ReturnUrl != null)
            {
                return Redirect(model.ReturnUrl);
            }
            return Ok();
        }
    }
}
