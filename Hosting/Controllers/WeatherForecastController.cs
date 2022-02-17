using IdentityServer.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer.IdentityServerConstants;

namespace Hosting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]        
        public string Test()
        {
            return "111";
        }
        [AllowAnonymous]
        [HttpGet("test")]
        public string Test2()
        {
            return "111";
        }
    }
}