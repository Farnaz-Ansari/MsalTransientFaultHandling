using Microsoft.Extensions.Options;
using System.Net;
using MsalTransientFaultHandling.AuthenticationProvider;
using MsalTransientFaultHandling.Clients;
using MsalTransientFaultHandling.Configuration;
using MsalTransientFaultHandling.Extentions;
using MsalTransientFaultHandling.Handlers;
using MsalTransientFaultHandling.Policies;

namespace MsalTransientFaultHandling.WeatherForecastApi
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// WeatherForecast Api Infrastructure 
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddWeatherForecastApiInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var apiOptions = configuration.GetSection(WeatherForecastApiOptions.Position).Get<WeatherForecastApiOptions>();
            apiOptions.Validate();

            services.Configure<WeatherForecastApiOptions>(o =>
            {
                o.EndPoint = apiOptions.EndPoint;
                o.PolicyOptions = apiOptions.PolicyOptions;
            });

            services.Configure<WeatherForecastApiMicrosoftOptions>(o =>
            {
                var options = configuration.GetMicrosoftOptions();
                o.Microsoft = options.Microsoft;
                o.Microsoft.Identity = new MicrosoftIdentitySection { Client = apiOptions.Client };
            });

            services.AddSingleton<IMsalClientService<WeatherForecastApiMicrosoftOptions>, MsalConfidentialClientService<WeatherForecastApiMicrosoftOptions>>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<WeatherForecastApiMicrosoftOptions>>();
                return new MsalConfidentialClientService<WeatherForecastApiMicrosoftOptions>(sp.GetConfidentialClientApplication(options), sp.GetService<IOptions<WeatherForecastApiMicrosoftOptions>>());
            });

            services.AddTransient<AuthenticationDelegatingHandler<WeatherForecastApiMicrosoftOptions>>();

            var logger = services.BuildServiceProvider().GetService<ILogger<WeatherForecastClient>>();

            services
                .AddHttpClient<IWeatherForecastClient, WeatherForecastClient>(c => c.BaseAddress = new Uri(apiOptions.EndPoint))
                .AddHttpMessageHandler<AuthenticationDelegatingHandler<WeatherForecastApiMicrosoftOptions>>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                })
                .AddPolicyHandler(HttpRetryPolicies.GetRetryPolicy(apiOptions.PolicyOptions, logger))
                .AddPolicyHandler(HttpCircuitBreakerPolicies.GetCircuitBreakerPolicy(apiOptions.PolicyOptions, logger));

            return services;
        }
    }
}
