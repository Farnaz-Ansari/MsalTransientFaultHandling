namespace MsalTransientFaultHandling.Configuration
{
    public class PolicyConfig : ICircuitBreakerPolicyConfig, IRetryPolicyConfig
    {
        public int RetryCount { get; set; }
        public int BreakDuration { get; set; }
        public int HandledEventCount { get; set; }
    }
}
