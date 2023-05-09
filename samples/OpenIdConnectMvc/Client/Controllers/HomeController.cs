using Client.Apis;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> WeatherForecast([FromServices] ApiClient client)
        {
            var model = await client.GetWeatherForecastListAsync();
            return View(model);
        }
    }
}