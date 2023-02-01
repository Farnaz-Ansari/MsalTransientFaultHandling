namespace MsalTransientFaultHandling.Configuration
{
    public interface ICircuitBreakerPolicyConfig
    {
        int HandledEventCount { get; set; }
        int BreakDuration { get; set; }
    }
}
