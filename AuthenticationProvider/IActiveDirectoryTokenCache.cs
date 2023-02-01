using Microsoft.Identity.Client;

namespace MsalTransientFaultHandling.AuthenticationProvider
{
    public interface IActiveDirectoryTokenCache
    {
        void EnableSerialization(ITokenCache tokenCache);
    }
}
