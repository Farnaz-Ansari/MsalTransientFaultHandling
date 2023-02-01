using MsalTransientFaultHandling.Configuration;

namespace MsalTransientFaultHandling.AuthenticationProvider
{
    public interface IMsalClientService<TOptions> : IMsalClientService where TOptions : MicrosoftOptions
    {
    }
}
