using MsalTransientFaultHandling.Configuration;

namespace MsalTransientFaultHandling.Extentions
{
    public static class ConfigurationExtensions
    {
        public static MicrosoftOptions GetMicrosoftOptions(this IConfiguration configuration)
        {
            var options = new MicrosoftOptions()
            {
                Microsoft = configuration.GetSection("Microsoft").Get<MicrosoftSection>()
            };
            return options;
        }
    }
}
