using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
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
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtClaimTypes.Subject,model.Username));
            var properties = new AuthenticationProperties();
            if (model.Remember == 1)
            {
                properties.IsPersistent = true;
                properties.ExpiresUtc = DateTime.UtcNow.AddDays(30);
            }
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookie"));
            await HttpContext.SignInAsync("Cookie", subject, properties);
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