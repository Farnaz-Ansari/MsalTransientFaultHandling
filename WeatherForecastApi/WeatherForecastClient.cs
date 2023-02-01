using MsalTransientFaultHandling.Clients;

namespace MsalTransientFaultHandling.WeatherForecastApi
{
    internal class WeatherForecastClient : BaseClient , IWeatherForecastClient
    {
        private readonly ILogger<WeatherForecastClient> _logger;

        public WeatherForecastClient(HttpClient httpClient, ILogger<WeatherForecastClient> logger) : base(httpClient)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _logger = logger;
        }

        public async Task<IEnumerable<WeatherForecast>> ListAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_httpClient.BaseAddress.ToString()));

                return await SendAsync<List<WeatherForecast>>(request, _jsonSerializerOptions, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while retrieving weather information.");
                throw;
            }
        }
    }
}
