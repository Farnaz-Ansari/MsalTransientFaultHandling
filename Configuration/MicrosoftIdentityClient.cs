using Microsoft.Identity.Client;

namespace MsalTransientFaultHandling.Configuration
{
    public class MicrosoftIdentityClient
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public AzureCloudInstance Instance { get; set; } = AzureCloudInstance.AzurePublic;
        public AadAuthorityAudience Authority { get; set; } = AadAuthorityAudience.AzureAdMyOrg;
        public IEnumerable<string> Scopes { get; set; }
    }
}
