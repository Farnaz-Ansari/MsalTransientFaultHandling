using Polly;
using MsalTransientFaultHandling.Configuration;

namespace MsalTransientFaultHandling.Policies
{
    public static class HttpCircuitBreakerPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ICircuitBreakerPolicyConfig config, ILogger logger)
        {
            return HttpPolicyBuilders.GetBaseBuilder()
                .CircuitBreakerAsync(config.HandledEventCount, TimeSpan.FromSeconds(config.BreakDuration),
                onBreak: (result, timeSpan, context) =>
                {
                    logger.LogWarning("CircuitBreaker onBreak for {delay}ms",
                         timeSpan.TotalMilliseconds);
                },
                onReset: context =>
                {
                    logger.LogWarning("CircuitBreaker closed again");
                },
                onHalfOpen: () =>
                {
                    logger.LogWarning("CircuitBreaker onHalfOpen");
                });
        }
    }
}
