namespace MsalTransientFaultHandling.Configuration
{
    public class MicrosoftGraph
    {
        public string TenantId { get; set; }
        public string AuthenticationUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Uri GraphUri { get; set; }
    }
}
