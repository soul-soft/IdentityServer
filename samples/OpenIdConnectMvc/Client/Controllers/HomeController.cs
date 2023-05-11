using System.Net;
using System.Text.Encodings.Web;
using Client.Apis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Welcome()
        {
            return View();
        }

        public async Task<IActionResult> WeatherForecast([FromServices] ApiClient client)
        {
            var model = await client.GetWeatherForecastListAsync();
            return View(model);
        }
       
    }
}