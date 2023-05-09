using System.Security.Claims;
using System.Web;
using IdentityServer.Models;
using Idp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public AccountController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            var returnModel = new AccountLoginViewModel();
            returnModel.ReturnUrl = returnUrl;
            return View(returnModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AccountLoginModel model)
        {
            var returnModel = new AccountLoginViewModel();
            if (!ModelState.IsValid)
            {
                return View(new { Error = "≤Œ ˝Œﬁ–ß" });
            }
            if (model.Username != "admin")
            {
                returnModel.Error = "’À∫≈ªÚ√‹¬Î¥ÌŒÛ";
                return View(returnModel);
            }
            if (model.Password != "admin")
            {
                returnModel.Error = "’À∫≈ªÚ√‹¬Î¥ÌŒÛ";
                return View(returnModel);
            }
            var subject = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject,model.Username)
            }, "cookie"));
            await HttpContext.SignInAsync("Cookie", subject);
            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return Redirect("/");
            }
        }
    }
}