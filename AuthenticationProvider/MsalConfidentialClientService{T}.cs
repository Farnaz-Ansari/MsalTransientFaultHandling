using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MsalTransientFaultHandling.Configuration;

namespace MsalTransientFaultHandling.AuthenticationProvider
{
    public class MsalConfidentialClientService<TOptions> : MsalConfidentialClientService, IMsalClientService<TOptions> where TOptions : MicrosoftOptions
    {
        public MsalConfidentialClientService(IConfidentialClientApplication confidetialClientApplication,
                                       IOptions<TOptions> options) : base(confidetialClientApplication,
                                                                          options)
        {
        }
    }
}
