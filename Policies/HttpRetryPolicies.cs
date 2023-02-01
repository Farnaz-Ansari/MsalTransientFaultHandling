using Polly;
using Polly.Contrib.WaitAndRetry;
using System.Net;
using MsalTransientFaultHandling.Configuration;

namespace MsalTransientFaultHandling.Policies
{
    public static class HttpRetryPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IRetryPolicyConfig config, ILogger logger)
        {
            return HttpPolicyBuilders.GetBaseBuilder()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(ComputeDelay(config.RetryCount),
                onRetry: (outcome, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning("Delaying for {delay}ms, then making retry {retry}.", timeSpan.TotalMilliseconds, retryCount);
                });
        }

        private static IEnumerable<TimeSpan> ComputeDelay(int retryCount) => Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: retryCount);
    }
}
