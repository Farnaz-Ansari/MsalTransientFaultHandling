using Microsoft.Identity.Client;

namespace MsalTransientFaultHandling.AuthenticationProvider
{
    public class MsalApiClient : IMsalHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public MsalApiClient(HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));

            _httpClient = httpClient;
        }

        public HttpClient GetHttpClient() => _httpClient;
    }
}
