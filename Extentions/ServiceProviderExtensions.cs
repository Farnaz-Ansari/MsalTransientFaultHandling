using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web.TokenCacheProviders;
using MsalTransientFaultHandling.AuthenticationProvider;
using MsalTransientFaultHandling.Configuration;

namespace MsalTransientFaultHandling.Extentions
{
    public static class ServiceProviderExtensions
    {
        public static IConfidentialClientApplication GetConfidentialClientApplication<TOptions>(this IServiceProvider sp, IOptions<TOptions> options) where TOptions : MicrosoftOptions
        {
            var msalApiClient = sp.GetRequiredService<IMsalHttpClientFactory>();
            var activeDirectoryTokenCache = sp.GetRequiredService<IActiveDirectoryTokenCache>();
            var msalTokenCacheProvider = sp.GetRequiredService<IMsalTokenCacheProvider>();

            var confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(options.Value.Microsoft.Graph.ClientId)
            .WithTenantId(options.Value.Microsoft.Identity.Client.TenantId)
            .WithAuthority(options.Value.Microsoft.Identity.Client.Instance, options.Value.Microsoft.Identity.Client.Authority)
            .WithClientSecret(options.Value.Microsoft.Graph.ClientSecret)
            .WithLegacyCacheCompatibility(false)
            .WithHttpClientFactory(msalApiClient)
            .Build();

            activeDirectoryTokenCache.EnableSerialization(confidentialClientApplication.AppTokenCache);
            msalTokenCacheProvider.Initialize(confidentialClientApplication.AppTokenCache);
            return confidentialClientApplication;
        }
    }
}
