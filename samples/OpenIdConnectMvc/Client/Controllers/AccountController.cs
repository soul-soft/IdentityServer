using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AccountController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       
        public async Task Logout()
        {
            //±¾µØÍË³ö
            var propertites = new AuthenticationProperties();
            propertites.RedirectUri = "/Home/Welcome";
            await HttpContext.SignOutAsync("Idp", propertites);
        }
    }
}