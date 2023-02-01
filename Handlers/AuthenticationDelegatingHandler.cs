using System.Net.Http.Headers;
using MsalTransientFaultHandling.AuthenticationProvider;
using MsalTransientFaultHandling.Configuration;

namespace MsalTransientFaultHandling.Handlers
{
    public class AuthenticationDelegatingHandler<TOptions> : DelegatingHandler where TOptions : MicrosoftOptions
    {
        private readonly IMsalClientService<TOptions> _msalClientService;

        public AuthenticationDelegatingHandler(IMsalClientService<TOptions> msalClientService)
        {
            ArgumentNullException.ThrowIfNull(msalClientService, nameof(msalClientService));

            _msalClientService = msalClientService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _msalClientService.GetAccessTokenAsync(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
