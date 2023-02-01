namespace MsalTransientFaultHandling.AuthenticationProvider
{
    public interface IMsalClientService
    {
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    }
}
