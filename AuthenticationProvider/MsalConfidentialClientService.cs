using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MsalTransientFaultHandling.Configuration;

namespace MsalTransientFaultHandling.AuthenticationProvider
{
    public class MsalConfidentialClientService : Configurable<MicrosoftOptions>, IMsalClientService
    {
        private readonly MicrosoftIdentityClient _microsoftIdentityClient;
        private readonly IConfidentialClientApplication _confidentialClientApplication;

        public MsalConfidentialClientService(
            IConfidentialClientApplication confidentialClientApplication,
            IOptions<MicrosoftOptions> options) : base(options.Value)
        {
            ArgumentNullException.ThrowIfNull(confidentialClientApplication, nameof(confidentialClientApplication));
            ArgumentNullException.ThrowIfNull(options.Value.Microsoft?.Identity?.Client, nameof(options.Value.Microsoft.Identity.Client));
            _microsoftIdentityClient = options.Value.Microsoft.Identity.Client;

            ArgumentNullException.ThrowIfNull(_microsoftIdentityClient.TenantId, nameof(_microsoftIdentityClient.TenantId));
            ArgumentNullException.ThrowIfNull(_microsoftIdentityClient.ClientId, nameof(_microsoftIdentityClient.ClientId));
            ArgumentNullException.ThrowIfNull(_microsoftIdentityClient.Scopes, nameof(_microsoftIdentityClient.Scopes));

            _confidentialClientApplication = confidentialClientApplication;
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            var result = await _confidentialClientApplication.AcquireTokenForClient(_microsoftIdentityClient.Scopes).ExecuteAsync(cancellationToken).ConfigureAwait(false);
            return result.AccessToken;
        }
    }
}
