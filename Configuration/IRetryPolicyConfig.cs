namespace MsalTransientFaultHandling.Configuration
{
    public interface IRetryPolicyConfig
    {
        int RetryCount { get; set; }
    }
}
