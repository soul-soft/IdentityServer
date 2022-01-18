using IdentityServer.Authentication;
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
        [Authorize(IdentityServerAuthenticationDefaults.PolicyName)]
        public string Test()
        {
            return "111";
        }
    }
}