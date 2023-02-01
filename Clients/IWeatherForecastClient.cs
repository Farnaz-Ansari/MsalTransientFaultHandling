namespace MsalTransientFaultHandling.Clients
{
    public interface IWeatherForecastClient
    {
        Task<IEnumerable<WeatherForecast>> ListAsync(CancellationToken cancellationToken = default);
    }
}
