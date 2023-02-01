using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MsalTransientFaultHandling.Clients
{
    public abstract class BaseClient
    {
        protected static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        protected readonly HttpClient _httpClient;

        protected BaseClient(HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));

            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        protected virtual async Task<T> SendAsync<T>(HttpRequestMessage request,
                                                     JsonSerializerOptions options,
                                                     CancellationToken cancellationToken = default) where T : new()
        {
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new HttpRequestException($"Request to {request.RequestUri} failed with status code {response.StatusCode}:{content}");
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);
        }
    }
}
