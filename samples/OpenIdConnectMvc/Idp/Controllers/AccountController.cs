using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Web;
using IdentityServer.Models;
using IdentityServer.Services;
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

        /// <summary>
        /// 处理登入请求
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromServices] ISessionManager manager, AccountLoginModel model)
        {
            var returnModel = new AccountLoginViewModel();
            if (!ModelState.IsValid)
            {
                return View(new { Error = "参数无效" });
            }
            if (model.Username != "admin")
            {
                returnModel.Error = "账号或密码错误";
                return View(returnModel);
            }
            if (model.Password != "admin")
            {
                returnModel.Error = "账号或密码错误";
                return View(returnModel);
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtClaimTypes.Subject, model.Username));
            var properties = new AuthenticationProperties();
            if (model.Remember == 1)
            {
                properties.IsPersistent = true;
                properties.ExpiresUtc = DateTime.UtcNow.AddDays(30);
            }
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookie"));
            await manager.SignInAsync("Cookie", subject, properties);
            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return Redirect("/");
            }
        }

        /// <summary>
        /// 处理登出请求
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task<IActionResult> Logout([FromServices] ISessionManager manager,string returnUrl)
        {
            await manager.SignOutAsync("Cookie");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect("/Account/Login");
        }
    }
}