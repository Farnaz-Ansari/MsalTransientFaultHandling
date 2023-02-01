using Polly.Extensions.Http;
using Polly;

namespace MsalTransientFaultHandling.Policies
{
    public static class HttpPolicyBuilders
    {
        public static PolicyBuilder<HttpResponseMessage> GetBaseBuilder() => HttpPolicyExtensions.HandleTransientHttpError();
    }
}
