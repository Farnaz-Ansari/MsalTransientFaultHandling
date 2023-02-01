using Microsoft.AspNetCore.Mvc;
using MsalTransientFaultHandling.Clients;

namespace tfhForMSAL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastClient _WeatherForecastClient;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IWeatherForecastClient WeatherForecastClient, ILogger<WeatherForecastController> logger)
        {
            ArgumentNullException.ThrowIfNull(WeatherForecastClient, nameof(WeatherForecastClient));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _WeatherForecastClient = WeatherForecastClient;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            return Ok(await _WeatherForecastClient.ListAsync(cancellationToken));
        }
    }
}